namespace FishSpotApi.Domain.Exception;

public class ImageNotFoundException: System.Exception
{
    public ImageNotFoundException()
    {
    }

    public ImageNotFoundException(string message) : base(message)
    {
    }

    public ImageNotFoundException(string message, System.Exception inner) : base(message, inner)
    {
    }
}