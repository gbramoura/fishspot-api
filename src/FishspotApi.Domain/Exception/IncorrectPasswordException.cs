namespace FishspotApi.Domain.Exception
{
    public class IncorrectPasswordException : System.Exception
    {
        public IncorrectPasswordException()
        {
        }

        public IncorrectPasswordException(string message) : base(message)
        {
        }

        public IncorrectPasswordException(string message, System.Exception inner) : base(message, inner)
        {
        }
    }
}