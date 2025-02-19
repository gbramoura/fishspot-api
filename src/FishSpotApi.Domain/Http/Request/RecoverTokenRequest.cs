using System.ComponentModel.DataAnnotations;

namespace FishSpotApi.Domain.Http.Request;

public class RecoverTokenRequest
{
    [Required(ErrorMessage = "The token must be filled")]
    [EmailAddress(ErrorMessage = "Token it not valid")]
    [MaxLength(5, ErrorMessage = "The limit of characters 5 has been reached")]
    public string Token { get; set; }
    
    [Required(ErrorMessage = "The e-mail must be filled")]
    [EmailAddress(ErrorMessage = "E-mail it not valid")]
    [MaxLength(245, ErrorMessage = "The limit of characters 245 has been reached")]
    public string Email { get; set; }
}