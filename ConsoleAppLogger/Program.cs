using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using System;
using System.Diagnostics;

namespace ConsoleAppLogger
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new TextWriterTraceListener("TextWriterOutput.log", "myListener"));
            Trace.TraceInformation("Test message.");
            // You must close or flush the trace to empty the output buffer.
            Trace.Flush();

            var configurationProvider = new ConfigurationProvider();
            LoggerProvider loggerProvider = new LoggerProvider(nameof(ConsoleAppLogger), configurationProvider.GetLoggingSection);

            var logger = loggerProvider.CreateLogger();

            try
            {
                throw new Exception("This is the Exception Message");
                logger.LogDebug(1, "This is a Debug Level Log - Getting item {Id} at {RequestTime}", 40, DateTime.Now);
                logger.LogInformation(2, "This is an Informational Log - Getting item {Id} at {RequestTime}", 41, DateTime.Now);
                logger.LogWarning(2, "This is a Warning Log - Getting item {Id} at {RequestTime}", 41, DateTime.Now);
            }
            catch (Exception e)
            {
                logger.LogInformation(3, "We Should only see this when an Exception occurs and LogLevel == Error");
                logger.LogError(4, "An Exception was thrown. Exception Type: {ExceptionType} with Message: {ExceptionMessage}", e.GetType().Name, e.Message);
            }
        }
    }
}
