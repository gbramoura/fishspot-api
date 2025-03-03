using FishSpotApi.Core.Repository;
using FishSpotApi.Domain.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FishSpotApi.Domain.Resources;
using Microsoft.Extensions.Localization;

namespace FishSpotApi.Core.Services;

public class TokenService(IConfiguration config, TokenRepository auth, IStringLocalizerFactory factory)
{
    private readonly IStringLocalizer _localizer = factory.Create(typeof(FishSpotResource));
    public string GenerateToken(IEnumerable<Claim> claims)
    {
        var key = Encoding.ASCII.GetBytes(config["Authentication:AccessTokenSecret"] ?? "");
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(Int32.Parse(config["Authentication:AccessTokenExpirationMinutes"] ?? "")),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["Authentication:AccessTokenSecret"] ?? "")),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException(_localizer["token_invalid"]);
        }

        return principal;
    }

    public string GenerateRefreshToken() => Guid.NewGuid().ToString();

    public void SaveRefreshToken(string actor, string refreshToken, string token)
    {
        auth.Insert(new TokenEntity()
        {
            Actor = actor,
            RefreshToken = refreshToken,
            Token = token
        });
    }

    public string GetRefreshToken(string actor)
    {
        var token = auth.GetByActor(actor);
        return (token != null ? token.RefreshToken : string.Empty);
    }

    public void DeleteRefreshToken(string actor)
    {
        var token = auth.GetByActor(actor);
        if (token is not null)
        {
            auth.Delete(token.Id);
        }
    }
}