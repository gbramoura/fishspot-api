using System.ComponentModel.DataAnnotations;

namespace FishSpotApi.Domain.Http.Request;

public class RecoverTokenRequest
{
    [Required(ErrorMessage = "annotation_token_required")]
    [MaxLength(5, ErrorMessage = "annotation_token_max_length")]
    public string Token { get; set; }
    
    [Required(ErrorMessage = "annotation_email_required")]
    [EmailAddress(ErrorMessage = "annotation_email_validation")]
    [MaxLength(245, ErrorMessage = "annotation_email_max_length")]
    public string Email { get; set; }
}