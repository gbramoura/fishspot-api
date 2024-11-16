namespace FishspotApi.Domain.Http.Response
{
    public class UserLoginResponse
    {
        public  string Name { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
