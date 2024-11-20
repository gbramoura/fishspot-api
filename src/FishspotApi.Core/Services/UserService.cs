using FishspotApi.Core.Repository;
using FishspotApi.Core.Utils;
using FishspotApi.Domain.Entity;
using FishspotApi.Domain.Exception;
using FishspotApi.Domain.Http.Request;
using FishspotApi.Domain.Http.Response;
using System.Security.Claims;
using System.Text;

namespace FishspotApi.Core.Services
{
    public class UserService(UserRepository user, RecoverPasswordRepository recover, TokenService token, MailService mail)
    {
        private readonly RecoverPasswordRepository _recoverPassword = recover;
        private readonly UserRepository _user = user;
        private readonly TokenService _token = token;
        private readonly MailService _mail = mail;

        public UserResponse RegisterUser(UserRegisterRequest payload)
        {
            var user = _user.Insert(new UserEntity
            {
                Email = payload.Email,
                Name = payload.Name,
                Password = PasswordUtils.EncryptPassword(payload.Password),
                UniqueIdentifierToken = Guid.NewGuid().ToString()
            });

            return new UserResponse
            {
                Email = user.Email,
                Name = user.Name,
                Id = user.Id
            };
        }

        public bool IsUniqueEmail(string email) => _user.GetByEmail(email).Any();

        public UserLoginResponse LoginUser(UserLoginRequest payload)
        {
            var user = _user.GetByEmail(payload.Email).FirstOrDefault();

            if (user is null)
            {
                throw new UserNotFoundException("User not found");
            }

            if (!PasswordUtils.VerifyPassword(user.Password, payload.Password))
            {
                throw new IncorrectPasswordException("Password is not correct");
            }

            var userRefreshToken = _token.GenerateRefreshToken();
            var userToken = _token.GenerateToken(new List<Claim>()
            {
                new Claim("code", user.Id.ToString()),
                new Claim("name", user.Name),
                new Claim("login", user.Email),
                new Claim("token", user.UniqueIdentifierToken),
                new Claim(ClaimTypes.Role, "user"),
            });

            _token.DeleteRefreshToken(user.UniqueIdentifierToken);
            _token.SaveRefreshToken(user.UniqueIdentifierToken, userRefreshToken, userToken);

            return new UserLoginResponse
            {
                Name = user.Name,
                Email = user.Email,
                Token = userToken,
                RefreshToken = userRefreshToken
            };
        }

        public RefreshTokenResponse RefreshToken(RefreshTokenRequest payload)
        {
            var claims = _token.GetPrincipalFromExpiredToken(payload.Token).Claims;
            var claim = claims.FirstOrDefault((claim) => claim.Type == "token");
            var value = claim.Value;

            var savedRefreshToken = _token.GetRefreshToken(value);
            if (savedRefreshToken != payload.RefreshToken)
            {
                throw new RefreshTokenInvalidException("Invalid refresh token");
            }

            var newJwtToken = _token.GenerateToken(claims);
            var newRefreshToken = _token.GenerateRefreshToken();

            _token.DeleteRefreshToken(value);
            _token.SaveRefreshToken(value, newRefreshToken, newJwtToken);

            return new RefreshTokenResponse
            {
                RefreshToken = newJwtToken,
                Token = newRefreshToken
            };
        }

        public void GenerateRecoverToken(RecoverPasswordRequest payload)
        {
            var user = _user.GetByEmail(payload.Email).FirstOrDefault();

            if (user is null)
            {
                throw new UserNotFoundException("User not found");
            }

            _mail.SendRecoverPasswordMail(user.Email, user.Name, GenerateToken(user.Email));
        }

        public void ChangePassword(ChangePasswordRequest payload)
        {
            var user = _user.GetByEmail(payload.Email).FirstOrDefault();

            if (user is null)
            {
                throw new UserNotFoundException("User not found");
            }

            if (VerifyToken(payload.Token, payload.Email))
            {
                throw new InvalidRecoverTokenException("The token is invalid");
            }

            user.Password = PasswordUtils.EncryptPassword(payload.NewPassword);

            _user.Update(user);
        }

        private string GenerateToken(string email)
        {
            var strBuilder = new StringBuilder();
            var random = new Random();
            char letter;

            for (int i = 0; i < 5; i++)
            {
                int shift = Convert.ToInt32(Math.Floor(25 * random.NextDouble()));
                letter = Convert.ToChar(shift + 65);
                strBuilder.Append(letter);
            }

            _recoverPassword.Insert(new RecoverPasswordEntity
            {
                Email = email,
                Token = strBuilder.ToString(),
                ExpirationDate = DateTime.Now.AddDays(1)
            });

            return strBuilder.ToString();
        }

        private bool VerifyToken(string token, string email)
        {
            var recoverToken = _recoverPassword.GetByTokenAndEmail(token, email);
            var date = DateTime.Now;

            if (recoverToken is null)
            {
                return false;
            }

            var isRecoverTokenValid =
                recoverToken.ExpirationDate < date.AddMinutes(-5) &&
                recoverToken.ExpirationDate > date.AddMinutes(5);

            _recoverPassword.Delete(recoverToken.Id);

            return isRecoverTokenValid;
        }
    }
}