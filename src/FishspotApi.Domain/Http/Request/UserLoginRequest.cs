using System.ComponentModel.DataAnnotations;

namespace FishspotApi.Domain.Http.Request
{
    public class UserLoginRequest
    {
        [Required(ErrorMessage = "The e-mail name must be filled")]
        [EmailAddress(ErrorMessage = "E-mail it not valid")]
        [MaxLength(245, ErrorMessage = "The limit of characters 245 has been reached")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The password name must be filled")]
        [MinLength(8, ErrorMessage = "The password must have more than 8 characters")]
        public string Password { get; set; }
    }
}