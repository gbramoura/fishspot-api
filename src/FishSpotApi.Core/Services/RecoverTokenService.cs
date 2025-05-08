using System.Text;
using FishSpotApi.Core.Repository;
using FishSpotApi.Domain.Entity;
using FishSpotApi.Domain.Resources;
using Microsoft.Extensions.Localization;

namespace FishSpotApi.Core.Services;

public class RecoverTokenService(RecoverPasswordRepository recoverRepository, IStringLocalizerFactory factory)
{
    private readonly IStringLocalizer _localizer = factory.Create(typeof(FishSpotResource));
    
    public string GenerateToken(string email)
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
            ExpirationDate = DateTime.UtcNow.AddMinutes(5),
        });

        return strBuilder.ToString();
    }

    public bool VerifyToken(string token, string email)
    {
        var entity = recoverRepository.GetByTokenAndEmail(token, email);
        var date = DateTime. UtcNow;

        if (entity is null)
        {
            return false;
        }

        var minusFiveMinutes = date.AddMinutes(-5);
        var plusFiveMinutes = date.AddMinutes(5);
        
        return entity.ExpirationDate >= minusFiveMinutes && entity.ExpirationDate <= plusFiveMinutes;
    }
    
    public void DeleteToken(string token, string email)
    {
        var recoverToken = recoverRepository.GetByTokenAndEmail(token, email);

        if (recoverToken is null)
        {
            throw new Exception(_localizer["token_not_found"]);
        }

        recoverRepository.Delete(recoverToken.Id);
    }
}