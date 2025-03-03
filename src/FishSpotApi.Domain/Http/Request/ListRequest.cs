using System.ComponentModel.DataAnnotations;

namespace FishSpotApi.Domain.Http.Request;

public class ListRequest
{
    [Required(ErrorMessage = "annotation_page_size_required")]
    [Range(1, int.MaxValue, ErrorMessage = "annotation_page_size_range")]
    public int PageSize { get; set; }
        
    [Required(ErrorMessage = "annotation_page_number_required")]
    [Range(1, int.MaxValue, ErrorMessage = "annotation_page_number_range")]
    public int PageNumber { get; set; }
}