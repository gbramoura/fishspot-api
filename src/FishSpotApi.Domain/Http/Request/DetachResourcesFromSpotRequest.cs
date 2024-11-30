using System.ComponentModel.DataAnnotations;

namespace FishSpotApi.Domain.Http.Request;

public class DetachResourcesFromSpotRequest 
{
    [Required(ErrorMessage = "The spot unique identifier must be filled")]
    public string SpotId { get; set; }
    
    [Required(ErrorMessage = "The list with file must be filled")]
    [MinLength(1, ErrorMessage = "The minimum number of file id must be at least one")]
    public IEnumerable<string> Files { get; set; }
}