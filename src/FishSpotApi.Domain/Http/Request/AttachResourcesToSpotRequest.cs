using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FishSpotApi.Domain.Http.Request;

public class AttachResourcesToSpotRequest
{
    [Required(ErrorMessage = "The files must be filled")]
    [MinLength(1, ErrorMessage = "The minimum number of file must be at least one")]
    [MaxLength(30, ErrorMessage = "The limit of files has been reached")]
    public IEnumerable<IFormFile> Files { get; set; }
    
    [Required(ErrorMessage = "The spot unique identifier must be filled")]
    public string SpotId { get; set; }
}