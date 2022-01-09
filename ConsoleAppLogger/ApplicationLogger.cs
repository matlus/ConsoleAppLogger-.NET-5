using Microsoft.Extensions.Logging;
using System;

namespace ConsoleAppLogger
{
    internal sealed class ApplicationLogger
    {
        private readonly ILogger _logger;
        public ApplicationLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void LogException<T1, T2, T3>(int eventId, Exception exception, string message, T1 param1, T2 param2, T3 param3)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(eventId, exception, message, param1, param2, param3);
            }
        }

        public void LogException<T1, T2, T3, T4>(int eventId, Exception exception, string message, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(eventId, exception, message, param1, param2, param3, param4);
            }
        }

        public void LogException<T1, T2, T3, T4, T5>(int eventId, Exception exception, string message, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(eventId, exception, message, param1, param2, param3, param4, param5);
            }
        }
    }
}
