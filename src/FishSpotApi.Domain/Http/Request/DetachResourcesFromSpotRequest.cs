using System.ComponentModel.DataAnnotations;

namespace FishSpotApi.Domain.Http.Request;

public class DetachResourcesFromSpotRequest 
{
    [Required(ErrorMessage = "annotation_spot_required")]
    public string SpotId { get; set; }
    
    [Required(ErrorMessage = "annotation_files_required")]
    [MinLength(1, ErrorMessage = "annotation_files_min_length")]
    public List<string> Files { get; set; }
}