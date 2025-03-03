using System.ComponentModel.DataAnnotations;

namespace FishSpotApi.Domain.Http.Request;

public class RecoverPasswordRequest
{
    [Required(ErrorMessage = "annotation_email_required")]
    [EmailAddress(ErrorMessage = "annotation_email_validation")]
    [MaxLength(245, ErrorMessage = "annotation_email_max_length")]
    public string Email { get; set; }
}