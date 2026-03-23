using FishSpotApi.Logger.Enums;
using FishSpotApi.Logger.Factories;
using FishSpotApi.Logger.Interfaces;

namespace FishSpotApi.Logger
{
    public static class LoggerFactory
    {
        private static ILogger _logger = new ConsoleLoggerFactory();

        public static void Trace(string message)
        {
            Log(LoggerType.Trace, message);
        }

        public static void Warning(string message)
        {
            Log(LoggerType.Warning, message);
        }

        public static void Info(string message)
        {
            Log(LoggerType.Info, message);
        }

        public static void Error(string message, Exception ex)
        {
            Log(LoggerType.Error, message, ex);
        }

        private static void Log(LoggerType level, string message, Exception? ex = null)
        {
            _logger.Log(level, message, ex);
        }
    }
}