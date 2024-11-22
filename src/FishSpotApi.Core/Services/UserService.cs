using FishSpotApi.Core.Repository;
using FishSpotApi.Core.Utils;
using FishSpotApi.Domain.Entity;
using FishSpotApi.Domain.Exception;
using FishSpotApi.Domain.Http.Request;
using FishSpotApi.Domain.Http.Response;
using System.Security.Claims;
using System.Text;

namespace FishSpotApi.Core.Services;

public class UserService(UserRepository userRepository, RecoverPasswordRepository recoverRepository, TokenService tokenService, MailService mailService)
{
    public UserResponse RegisterUser(UserRegisterRequest payload)
    {
        var user = userRepository.Insert(new UserEntity
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

    public bool IsUniqueEmail(string email) => userRepository.GetByEmail(email).Any();

    public UserLoginResponse LoginUser(UserLoginRequest payload)
    {
        var user = userRepository.GetByEmail(payload.Email).FirstOrDefault();

        if (user is null)
        {
            throw new UserNotFoundException("User not found");
        }

        if (!PasswordUtils.VerifyPassword(user.Password, payload.Password))
        {
            throw new IncorrectPasswordException("Password is not correct");
        }

        var userRefreshToken = tokenService.GenerateRefreshToken();
        var userToken = tokenService.GenerateToken(new List<Claim>()
        {
            new Claim("code", user.Id.ToString()),
            new Claim("name", user.Name),
            new Claim("login", user.Email),
            new Claim("token", user.UniqueIdentifierToken),
            new Claim(ClaimTypes.Role, "user"),
        });

        tokenService.DeleteRefreshToken(user.UniqueIdentifierToken);
        tokenService.SaveRefreshToken(user.UniqueIdentifierToken, userRefreshToken, userToken);

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
        var claims = tokenService.GetPrincipalFromExpiredToken(payload.Token).Claims;
        var claim = claims.FirstOrDefault((claim) => claim.Type == "token");
        var value = claim.Value;

        var savedRefreshToken = tokenService.GetRefreshToken(value);
        if (savedRefreshToken != payload.RefreshToken)
        {
            throw new RefreshTokenInvalidException("Invalid refresh token");
        }

        var newJwtToken = tokenService.GenerateToken(claims);
        var newRefreshToken = tokenService.GenerateRefreshToken();

        tokenService.DeleteRefreshToken(value);
        tokenService.SaveRefreshToken(value, newRefreshToken, newJwtToken);

        return new RefreshTokenResponse
        {
            RefreshToken = newJwtToken,
            Token = newRefreshToken
        };
    }

    public void GenerateRecoverToken(RecoverPasswordRequest payload)
    {
        var user = userRepository.GetByEmail(payload.Email).FirstOrDefault();

        if (user is null)
        {
            throw new UserNotFoundException("User not found");
        }

        mailService.SendRecoverPasswordMail(user.Email, user.Name, GenerateToken(user.Email));
    }

    public void ChangePassword(ChangePasswordRequest payload)
    {
        var user = userRepository.GetByEmail(payload.Email).FirstOrDefault();

        if (user is null)
        {
            throw new UserNotFoundException("User not found");
        }

        if (VerifyToken(payload.Token, payload.Email))
        {
            throw new InvalidRecoverTokenException("The token is invalid");
        }

        user.Password = PasswordUtils.EncryptPassword(payload.NewPassword);

        userRepository.Update(user);
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

        recoverRepository.Insert(new RecoverPasswordEntity
        {
            Email = email,
            Token = strBuilder.ToString(),
            ExpirationDate = DateTime.Now.AddDays(1)
        });

        return strBuilder.ToString();
    }

    private bool VerifyToken(string token, string email)
    {
        var recoverToken = recoverRepository.GetByTokenAndEmail(token, email);
        var date = DateTime.Now;

        if (recoverToken is null)
        {
            return false;
        }

        var isRecoverTokenValid =
            recoverToken.ExpirationDate < date.AddMinutes(-5) &&
            recoverToken.ExpirationDate > date.AddMinutes(5);

        recoverRepository.Delete(recoverToken.Id);

        return isRecoverTokenValid;
    }
}