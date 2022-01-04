using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Extensions.Logging.EventLog;
using System;

namespace ConsoleAppLogger
{
    internal sealed class LoggerProvider
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly string applicationNamespaceName;
        private ILogger logger;

        public LoggerProvider(string applicationNamespaceName, Func<IConfiguration> loggingConfigurationSectionDelegate)
        {
            this.applicationNamespaceName = applicationNamespaceName;

            loggerFactory = LoggerFactory.Create(builder =>
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
                .AddApplicationInsights("fac3c5d0-e9a5-420a-b854-2d627340f23b", applicationInsightsLoggerOptions =>
                {
                    applicationInsightsLoggerOptions.FlushOnDispose = true;
                    applicationInsightsLoggerOptions.TrackExceptionsAsExceptionTelemetry = true;
                });
            });
        }

        public ILogger CreateLogger()
        {
            if (logger == null)
            {
                logger = loggerFactory.CreateLogger(applicationNamespaceName);
            }

            return logger;
        }

        public ILogger CreateLogger<T>()
        {
            return logger = CreateLogger();
        }
    }
}
