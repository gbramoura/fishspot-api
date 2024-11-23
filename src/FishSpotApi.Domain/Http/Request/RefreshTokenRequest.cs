using System.ComponentModel.DataAnnotations;

namespace FishSpotApi.Domain.Http.Request;

public class RefreshTokenRequest
{
    [Required(ErrorMessage = "The token must be filled")]
    public string Token { get; set; }

    [Required(ErrorMessage = "The refresh token must be filled")]
    public string RefreshToken { get; set; }
}