using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleAppLogger
{
    internal class Program
    {
        private static TelemetryConfiguration telemetryConfiguration = new TelemetryConfiguration("fac3c5d0-e9a5-420a-b854-2d627340f23b", new InMemoryChannel());
        private static TelemetryClient telemetryClient = new TelemetryClient(telemetryConfiguration);

        static async Task Main(string[] args)
        {
            var configurationProvider = new ConfigurationProvider();
            var loggerProvider = new LoggerProvider(nameof(ConsoleAppLogger), configurationProvider.GetLoggingConfiguration, configurationProvider.GetAppInsightsInstrumentationKey());

            var logger = loggerProvider.CreateLogger();

            try
            {
                ////throw new IntentionalException("This is the Exception Message");
                logger.LogDebug(Logger.EventId.Step1, $"In {{{Logger.Properties.MethodName}}} This is a Debug Level Log - Getting item {{{Logger.Properties.Id}}} at {{{Logger.Properties.RequestTime}}}", nameof(Main), 40, DateTime.Now);
                logger.LogInformation(2, "This is an Informational Log - Getting item {Id} at {RequestTime}", 41, DateTime.Now);
                logger.LogWarning(3, "This is a Warning Log - Getting item {Id} at {RequestTime}", 42, DateTime.Now);
            }
            catch (Exception e)
            {
                var eventProperties = new Dictionary<string, string> { { "Property1", "Value1" }, { "Property2", "Value2" } };

                telemetryClient.TrackEvent("Exception:Main", eventProperties);
                logger.LogInformation(4, "We Should only see this when an Exception occurs and LogLevel == Error");
                logger.LogError(5, e, "An Exception was thrown. Exception Type: {ExceptionType} with Message: {ExceptionMessage}", e.GetType().Name, e.Message);
            }
            finally
            {
                telemetryClient.Flush();
                await Task.Delay(1000);
                telemetryConfiguration.Dispose();
                loggerProvider.Dispose();
            }
        }

        internal static class Logger
        {
            public static class EventId
            {
                public const int Step1 = 1;
                public const int Step2 = 2;
                public const int Step3 = 3;
                public const int Step4 = 4;
                public const int Step5 = 5;
            }

            public static class Properties
            {
                public const string MethodName = "MethodName";
                public const string RequestTime = "RequestTime";
                public const string Id = "Id";
            }
        }
    }
}
