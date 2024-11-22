namespace FishSpotApi.Domain.Exception;

public class SpotNotFoundException : System.Exception
{
    public SpotNotFoundException()
    {
    }

    public SpotNotFoundException(string message) : base(message)
    {
    }

    public SpotNotFoundException(string message, System.Exception inner) : base(message, inner)
    {
    }
}