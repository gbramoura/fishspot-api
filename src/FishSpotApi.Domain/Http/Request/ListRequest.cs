using System.ComponentModel.DataAnnotations;

namespace FishSpotApi.Domain.Http.Request;

public class ListRequest
{
    [Required(ErrorMessage = "The page size is required")]
    [Range(1, int.MaxValue, ErrorMessage = "The page size must be higher than 0")]
    public int PageSize { get; set; }
        
    [Required(ErrorMessage = "The page number is required")]
    [Range(1, int.MaxValue, ErrorMessage = "The page number must be higher than 0")]
    public int PageNumber { get; set; }
}