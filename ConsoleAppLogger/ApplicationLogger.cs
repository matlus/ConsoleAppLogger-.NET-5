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

        public void LogError<T1, T2, T3>(int eventId, Exception exception, string message, T1 param1, T2 param2, T3 param3)
        {
            _logger.LogError(eventId, exception, message, param1, param2, param3);
        }

        public void LogWarning<T1, T2>(int eventId, string message, T1 param1, T2 param2)
        {
            if (_logger.IsEnabled(LogLevel.Warning))
            {
                _logger.LogWarning(eventId, message, param1, param2);
            }
        }
        
        public void LogInformation<T1>(int eventId, string message, T1 param1)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(eventId, message, param1);
            }
        }

        public void LogInformation<T1, T2>(int eventId, string message, T1 param1, T2 param2)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(eventId, message, param1, param2);
            }
        }

        public void LogInformation<T1, T2, T3>(int eventId, string message, T1 param1, T2 param2, T3 param3)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(eventId, message, param1, param2, param3);
            }
        }

        public void LogDebug<T1, T2, T3>(int eventId, string message, T1 param1, T2 param2, T3 param3)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug(eventId, message, param1, param2, param3);
            }
        }
    }
}
