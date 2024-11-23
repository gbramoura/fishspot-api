namespace FishSpotApi.Domain.Exception;

public class InvalidImageException: System.Exception
{
    public InvalidImageException()
    {
    }

    public InvalidImageException(string message) : base(message)
    {
    }

    public InvalidImageException(string message, System.Exception inner) : base(message, inner)
    {
    }
}