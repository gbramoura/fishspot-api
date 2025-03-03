using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FishSpotApi.Domain.Http.Request;

public class AttachResourceToUserRequest
{ 
        [Required(ErrorMessage = "annotation_user_file_required")]
        public IFormFile File { get; set; }
}