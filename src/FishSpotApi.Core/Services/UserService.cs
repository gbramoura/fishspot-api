using FishSpotApi.Core.Repository;
using FishSpotApi.Core.Utils;
using FishSpotApi.Domain.Entity;
using FishSpotApi.Domain.Exception;
using FishSpotApi.Domain.Http.Request;
using FishSpotApi.Domain.Http.Response;
using System.Security.Claims;
using System.Text;
using FishSpotApi.Domain.Resources;
using Microsoft.Extensions.Localization;

namespace FishSpotApi.Core.Services;

public class UserService(UserRepository userRepository, TokenService tokenService, MailService mailService, RecoverTokenService recoverTokenService, IStringLocalizerFactory factory)
{
    private readonly IStringLocalizer _localizer = factory.Create(typeof(FishSpotResource));
    public void RegisterUser(UserRegisterRequest payload)
    {
        userRepository.Insert(new UserEntity
        {
            Email = payload.Email,
            Name = payload.Name,
            Password = PasswordUtils.EncryptPassword(payload.Password),
            UniqueIdentifierToken = Guid.NewGuid().ToString()
        });
    }

    public bool IsUniqueEmail(string email) => userRepository.GetByEmail(email).Any();

    public UserLoginResponse LoginUser(UserLoginRequest payload)
    {
        var user = userRepository.GetByEmail(payload.Email).FirstOrDefault();
        if (user is null)
        {
            throw new UserNotFoundException(_localizer["user_not_found"]);
        }

        if (!PasswordUtils.VerifyPassword(user.Password, payload.Password))
        {
            throw new IncorrectPasswordException(_localizer["user_password_incorrect"]);
        }

        var userRefreshToken = tokenService.GenerateRefreshToken();
        var userToken = tokenService.GenerateToken(new List<Claim>()
        {
            new Claim("id", user.Id),
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
            throw new RefreshTokenInvalidException(_localizer["token_refresh_invalid"]);
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
            throw new UserNotFoundException(_localizer["user_not_found"]);
        }

        mailService.SendRecoverPasswordMail(user.Email, user.Name, recoverTokenService.GenerateToken(user.Email));
    }

    public void ChangePassword(ChangePasswordRequest payload)
    {
        var user = userRepository.GetByEmail(payload.Email).FirstOrDefault();
        if (user is null)
        {
            throw new UserNotFoundException(_localizer["user_not_found"]);
        }

        if (recoverTokenService.VerifyToken(payload.Token, payload.Email))
        {
            throw new InvalidRecoverTokenException(_localizer["token_invalid"]);
        }
            
        recoverTokenService.DeleteToken(payload.Token, payload.Email);
        user.Password = PasswordUtils.EncryptPassword(payload.NewPassword);

        userRepository.Update(user);
    }

    public bool ValidateRecoverToken(RecoverTokenRequest payload)
    {
        var user = userRepository.GetByEmail(payload.Email).FirstOrDefault();
        if (user is null)
        {
            throw new UserNotFoundException(_localizer["user_not_found"]);
        }

        return recoverTokenService.VerifyToken(payload.Token, payload.Email);
    }
}