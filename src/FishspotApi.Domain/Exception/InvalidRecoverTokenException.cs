namespace FishspotApi.Domain.Exception
{
    public class InvalidRecoverTokenException : System.Exception
    {
        public InvalidRecoverTokenException()
        {
        }

        public InvalidRecoverTokenException(string message) : base(message)
        {
        }

        public InvalidRecoverTokenException(string message, System.Exception inner) : base(message, inner)
        {
        }
    }
}