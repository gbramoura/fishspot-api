using System.ComponentModel.DataAnnotations;

namespace FishSpotApi.Domain.Http.Request;

public class RefreshTokenRequest
{
    [Required(ErrorMessage = "annotation_token_required")]
    public string Token { get; set; }

    [Required(ErrorMessage = "annotation_refresh_token_required")]
    public string RefreshToken { get; set; }
}