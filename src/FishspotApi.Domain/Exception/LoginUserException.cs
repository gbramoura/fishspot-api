namespace FishspotApi.Domain.Exception
{
    public class LoginUserException : System.Exception
    {
        public LoginUserException()
        {
        }

        public LoginUserException(string message) : base(message)
        {
        }

        public LoginUserException(string message, System.Exception inner) : base(message, inner)
        {
        }
    }
}
