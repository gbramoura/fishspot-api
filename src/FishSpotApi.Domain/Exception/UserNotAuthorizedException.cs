namespace FishSpotApi.Domain.Exception;

public class UserNotAuthorizedException: System.Exception
{
    public UserNotAuthorizedException()
    {
    }

    public UserNotAuthorizedException(string message) : base(message)
    {
    }

    public UserNotAuthorizedException(string message, System.Exception inner) : base(message, inner)
    {
    }
}