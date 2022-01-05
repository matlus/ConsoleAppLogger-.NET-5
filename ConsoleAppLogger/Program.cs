using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ConsoleAppLogger
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var configurationProvider = new ConfigurationProvider();
            var loggerProvider = new LoggerProvider(nameof(ConsoleAppLogger), configurationProvider.GetLoggingConfiguration, configurationProvider.GetAppInsightsInstrumentationKey());

            var logger = loggerProvider.CreateLogger<Program>();

            try
            {
                ////throw new Exception("This is the Exception Message");
                logger.LogDebug(1, "This is a Debug Level Log - Getting item {Id} at {RequestTime}", 40, DateTime.Now);
                logger.LogInformation(2, "This is an Informational Log - Getting item {Id} at {RequestTime}", 41, DateTime.Now);
                logger.LogWarning(3, "This is a Warning Log - Getting item {Id} at {RequestTime}", 42, DateTime.Now);
            }
            catch (Exception e)
            {
                logger.LogInformation(4, "We Should only see this when an Exception occurs and LogLevel == Error");
                logger.LogError(5, "An Exception was thrown. Exception Type: {ExceptionType} with Message: {ExceptionMessage}", e.GetType().Name, e.Message);
            }
            finally
            {
                await Task.Delay(1000);
                loggerProvider.Dispose();
            }
        }
    }
}
