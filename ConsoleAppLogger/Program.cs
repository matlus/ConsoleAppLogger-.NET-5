using Microsoft.Extensions.Logging;
using System;

namespace ConsoleAppLogger
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var executionStep = ExecutionStep.Entered;
            
            var configurationProvider = new ConfigurationProvider();
            var loggerProvider = new LoggerProvider("YouTube.Logs", configurationProvider.GetLoggingConfiguration, configurationProvider.GetAppInsightsInstrumentationKey());
            var logger = loggerProvider.CreateLogger();
            var appLogger = new ApplicationLogger(logger);  

            executionStep = ExecutionStep.LoggerCreated;

            try
            {
                executionStep = ExecutionStep.Step1;
                throw new IntentionalException("This is the Exception Message");
                logger.LogDebug(1, "In {MethodName}. In ExecutionStep: {ExecutionStep}. This is a Debug Level Log - Getting item {Id}", nameof(Main), executionStep, 40);
                
                executionStep = ExecutionStep.Step2;
                logger.LogInformation(2, "In ExecutionStep: {ExecutionStep}. This is an Informational Log - Getting item {Id}", executionStep, 41);

                executionStep = ExecutionStep.Step3;
                logger.LogWarning(3, "In ExecutionStep: {ExecutionStep}. This is a Warning Log - Getting item {Id}", executionStep, 42);

                executionStep = ExecutionStep.Completed;
            }
            catch (Exception e)
            {   
                logger.LogInformation(4,"In ExecutionStep: {ExecutionStep}. We Should only see this when an Exception occurs and LogLevel == Error", executionStep);                
                logger.LogError(5, "An Exception occured during the processing of Blah. In ExecutionStep: {ExecutionStep}. Exception Type: {ExceptionType} with Message: {ExceptionMessage}", executionStep, e.GetType().Name, e.Message);
            }
            finally
            {
                loggerProvider.Dispose();
            }
        }
    }

    internal enum ExecutionStep { Entered, LoggerCreated, Step1, Step2, Step3, Completed }
}