namespace FishspotApi.Domain.Exception
{
    public class RefreshTokenInvalidException : System.Exception
    {
        public RefreshTokenInvalidException()
        {
        }

        public RefreshTokenInvalidException(string message) : base(message)
        {
        }

        public RefreshTokenInvalidException(string message, System.Exception inner) : base(message, inner)
        {
        }
    }
}