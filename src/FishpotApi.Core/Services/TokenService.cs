﻿using FishSpotApi.Core.Repository;
using FishSpotApi.Domain.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FishSpotApi.Core.Services;

public class TokenService(IConfiguration config, TokenRepository auth)
{
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

    public bool IsValidCurrentToken(string token)
    {
        var key = Encoding.ASCII.GetBytes(config["Authentication:AccessTokenSecret"] ?? "");
        var mySecurityKey = new SymmetricSecurityKey(key);
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = mySecurityKey,
                ValidateIssuer = false,
                ValidateAudience = false,
            }, out SecurityToken validatedToken);
        }
        catch
        {
            return false;
        }

        return true;
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
            throw new SecurityTokenException("Invalid Token");
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

    public bool IsValidRefreshToken(string refreshToken, string token)
    {
        return auth.IsValidFields(refreshToken, token);
    }
}