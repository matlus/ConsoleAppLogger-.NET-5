using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace ConsoleAppLogger
{
    internal sealed class LoggerProvider : IDisposable
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly string eventLogName;
        private readonly ITelemetryChannel telemetryChannel;
        private readonly ConcurrentDictionary<Type, ILogger> loggersDictionary = new ConcurrentDictionary<Type, ILogger>();

        public LoggerProvider(string eventLogName, Func<IConfiguration> loggingConfigurationDelegate, string appInsightsInstrumentationKey = null)
        {
            this.eventLogName = eventLogName;

            telemetryChannel = new InMemoryChannel();

            loggerFactory = new LoggerFactory();
            loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.Services.Configure<TelemetryConfiguration>(config => config.TelemetryChannel = telemetryChannel);
                builder
                .ClearProviders()
                .AddConfiguration(loggingConfigurationDelegate())
#if DEBUG
                            .AddDebug()
#endif
                            .AddConsole()
                .AddEventSourceLogger()
                /*
                 * If the IntrumentationKey is null/Empty, no problem. No Application Insights logging
                 * will occur. So local machine doesn't need this setting. When deployed to Azure
                 * need to ensure ApplicationInsights:InstrumentationKey setting is present and valid
                 */
                .AddApplicationInsights(appInsightsInstrumentationKey);

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    builder.AddEventLog(new EventLogSettings { LogName = eventLogName, SourceName = eventLogName });
                }
            });
        }

        public ILogger CreateLogger<T>()
        {
            return loggersDictionary.GetOrAdd(typeof(T), loggerFactory.CreateLogger<T>());
        }

        public void Dispose()
        {
            telemetryChannel.Flush();
            telemetryChannel.Dispose();
        }
    }
}
