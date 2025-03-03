using System.ComponentModel.DataAnnotations;

namespace FishSpotApi.Domain.Http.Request;

public class UserRegisterRequest
{
    [Required(ErrorMessage = "annotation_name_required")]
    [MaxLength(245, ErrorMessage = "annotation_name_max_length")]
    public string Name { get; set; }

    [Required(ErrorMessage = "annotation_email_required")]
    [EmailAddress(ErrorMessage = "annotation_email_validation")]
    [MaxLength(245, ErrorMessage = "annotation_email_max_length")]
    public string Email { get; set; }

    [Required(ErrorMessage = "annotation_password_required")]
    [MinLength(8, ErrorMessage = "annotation_password_min_length")]
    public string Password { get; set; }
}