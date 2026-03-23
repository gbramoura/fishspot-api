using FishSpotApi.Logger.Enums;

namespace FishSpotApi.Logger.Interfaces
{
    public interface ILogger
    {
        void Log(LoggerType type, string message, Exception? ex);
    }
}