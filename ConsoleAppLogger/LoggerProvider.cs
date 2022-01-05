using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using System;
using System.Collections.Concurrent;

namespace ConsoleAppLogger
{
    internal sealed class LoggerProvider : IDisposable
    {
        ////private readonly ILoggerFactory loggerFactory;
        private readonly string applicationNamespaceName;
        private readonly IServiceProvider serviceProvider;
        private readonly ITelemetryChannel telemetryChannel;
        private readonly ConcurrentDictionary<Type, ILogger> loggersDictionary = new ConcurrentDictionary<Type, ILogger>();        

        public LoggerProvider(string applicationNamespaceName, Func<IConfiguration> loggingConfigurationSectionDelegate)
        {
            this.applicationNamespaceName = applicationNamespaceName;

            telemetryChannel = new InMemoryChannel();

            var serviceCollection = new ServiceCollection();
            serviceCollection.Configure<TelemetryConfiguration>(config => config.TelemetryChannel = telemetryChannel);

            serviceCollection.AddLogging(builder =>
            {
                builder
                .ClearProviders()
                .AddConfiguration(loggingConfigurationSectionDelegate())
#if DEBUG
                            .AddDebug()
#endif
                            .AddConsole()
                .AddEventSourceLogger()
                .AddEventLog(new EventLogSettings { LogName = applicationNamespaceName, SourceName = applicationNamespaceName })
                .AddApplicationInsights(loggingConfigurationSectionDelegate()["ApplicationInsights:InstrumentationKey"]);
            });

            serviceProvider = serviceCollection.BuildServiceProvider();

            #region If you don't need Application Insights Logging, replace all of the code above (after the first) line with code below

            ////            loggerFactory = LoggerFactory.Create(builder =>
            ////            {
            ////                builder
            ////                .ClearProviders()
            ////                .AddConfiguration(loggingConfigurationSectionDelegate())
            ////#if DEBUG
            ////                .AddDebug()
            ////#endif
            ////                .AddConsole()
            ////                .AddEventSourceLogger()
            ////                .AddEventLog(new EventLogSettings { LogName = applicationNamespaceName, SourceName = applicationNamespaceName });
            ////            });
            #endregion
        }

        public ILogger CreateLogger<T>()
        {
            ////return loggersDictionary.GetOrAdd(typeof(T), loggerFactory.CreateLogger<T>());
            return loggersDictionary.GetOrAdd(typeof(T), serviceProvider.GetRequiredService<ILogger<T>>());
        }

        public void Dispose()
        {
            telemetryChannel.Flush();
            telemetryChannel.Dispose();
        }
    }
}
