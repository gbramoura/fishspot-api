using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FishSpotApi.Domain.Http.Request;

public class AttachResourcesToSpotRequest
{
    [Required(ErrorMessage = "annotation_files_required")]
    [MaxLength(30, ErrorMessage = "annotation_files_max_length")]
    public List<IFormFile> Files { get; set; }
    
    [Required(ErrorMessage = "annotation_spot_required")]
    public string SpotId { get; set; }
}