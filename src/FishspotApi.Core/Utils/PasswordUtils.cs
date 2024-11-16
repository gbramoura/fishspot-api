namespace FishspotApi.Core.Utils
{
    public static class PasswordUtils
    {
        public static string EncryptPassword(string password)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt(13);
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        public static bool VerifyPassword(string hashPassword, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashPassword);
        }

        public static bool ComparePassword(string password, string confirmPassword)
        {
            return password.Trim().Equals(confirmPassword.Trim());
        }
    }
}
