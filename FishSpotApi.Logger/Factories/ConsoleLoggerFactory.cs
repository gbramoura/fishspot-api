using FishSpotApi.Logger.Enums;
using FishSpotApi.Logger.Interfaces;

namespace FishSpotApi.Logger.Factories
{
    public class ConsoleLoggerFactory : ILogger
    {
        private Dictionary<LoggerType, string> _loggerTypeDescription = new Dictionary<LoggerType, string>
        {
            {LoggerType.Info, "info" },
            {LoggerType.Warning, "warning" },
            {LoggerType.Trace, "trace" },
            {LoggerType.Error, "error" },
        };

        private string GetLoggerType(LoggerType type)
        {
            var logType = _loggerTypeDescription[type];

            if (logType == null)
            {
                logType = "None";
            }

            return logType;
        }

        public void Log(LoggerType type, string message, Exception? ex)
        {
            var logType = GetLoggerType(type);

            Console.WriteLine($"{logType}: {message}");

            if (type == LoggerType.Error && ex != null)
            {
                Console.WriteLine($"{logType}: Exception: {ex.Message}");
            }
        }
    }
}