using FishspotApi.Core.Repository;
using FishspotApi.Core.Utils;
using FishspotApi.Domain.Entity;
using FishspotApi.Domain.Http.Request;
using FishspotApi.Domain.Http.Response;
using System.Security.Claims;
using FishspotApi.Domain.Exception;

namespace FishspotApi.Core.Services
{
    public class UserService
    {
        private readonly UserRepository _user;
        private readonly TokenService _token;

        public UserService(UserRepository user, TokenService token)
        {
            _user = user;
            _token = token;
        }

        public UserResponse RegisterUser(UserRegister registerUser)
        {
            var user = _user.Insert(new UserEntity
            {
                Email = registerUser.Email,
                Name = registerUser.Name,
                Password = PasswordUtils.EncryptPassword(registerUser.Password),
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

        public UserLoginResponse LoginUser(UserLogin loginUser)
        {
            var user = _user.GetByEmail(loginUser.Email).FirstOrDefault();

            if (user is null)
            {
                throw new LoginUserException("User not found");
            }

            if (!PasswordUtils.VerifyPassword(user.Password, loginUser.Password))
            {
                throw new LoginUserException("Password is not correct");
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
    }
}
