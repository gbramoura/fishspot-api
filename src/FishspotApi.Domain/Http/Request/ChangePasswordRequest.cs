using System.ComponentModel.DataAnnotations;

namespace FishspotApi.Domain.Http.Request
{
    public class ChangePasswordRequest
    {
        [Required(ErrorMessage = "The e-mail name must be filled")]
        [EmailAddress(ErrorMessage = "E-mail it not valid")]
        [MaxLength(245, ErrorMessage = "The limit of characters 245 has been reached")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The validation token should be filled")]
        public string Token { get; set; }

        [Required(ErrorMessage = "The new password should be filled")]
        public string NewPassword { get; set; }
    }
}