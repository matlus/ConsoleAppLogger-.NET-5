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
            var executionStep = ExecutionStep.Entered;
            
            var configurationProvider = new ConfigurationProvider();
            var loggerProvider = new LoggerProvider(nameof(ConsoleAppLogger), configurationProvider.GetLoggingConfiguration, configurationProvider.GetAppInsightsInstrumentationKey());
            var logger = loggerProvider.CreateLogger();

            executionStep = ExecutionStep.LoggerCreated;

            try
            {
                executionStep = ExecutionStep.Step1;
                ////throw new IntentionalException("This is the Exception Message");
                logger.LogDebug(1, "In {MethodName}. In ExecutionStep: {ExecutionStep}. This is a Debug Level Log - Getting item {Id}", nameof(Main), executionStep, 40);
                
                executionStep = ExecutionStep.Step2;
                logger.LogInformation(2, "In ExecutionStep: {ExecutionStep}. This is an Informational Log - Getting item {Id}", executionStep, 41);

                executionStep = ExecutionStep.Step3;
                logger.LogWarning(3, "In ExecutionStep: {ExecutionStep}. This is a Warning Log - Getting item {Id}", executionStep, 42);

                executionStep = ExecutionStep.Completed;
            }
            catch (Exception e)
            {   
                var eventProperties = new Dictionary<string, string> { { "Property1", "Value1" }, { "Property2", "Value2" } };
                telemetryClient.TrackEvent("Exception:Main", eventProperties);

                logger.LogInformation(4, "In ExecutionStep: {ExecutionStep}. We Should only see this when an Exception occurs and LogLevel == Error", executionStep);                
                logger.LogError(5, e, "An Exception occured during the processing of Blah. In ExecutionStep: {ExecutionStep}. Exception Type: {ExceptionType} with Message: {ExceptionMessage}", executionStep, e.GetType().Name, e.Message);             
            }
            finally
            {
                telemetryClient.Flush();
                await Task.Delay(1000);
                telemetryConfiguration.Dispose();
                loggerProvider.Dispose();
            }
        }
    }

    internal enum ExecutionStep { Entered, LoggerCreated, Step1, Step2, Step3, Completed }
}
