using System.ComponentModel.DataAnnotations;

namespace FishSpotApi.Domain.Http.Request;

public class UserUpdateRequest
{
    [Required(ErrorMessage = "annotation_name_required")]
    [MaxLength(245, ErrorMessage = "annotation_name_max_length")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "annotation_description_required")]
    [MaxLength(245, ErrorMessage = "annotation_description_max_length")]
    public string Description { get; set; }
    
    [MaxLength(20, ErrorMessage = "annotation_username_max_length")]
    public string Username { get; set; }
    
    public UserAddressRequest? Address { get; set; }
}

public class UserAddressRequest
{
    
    [MaxLength(245, ErrorMessage = "annotation_street_max_length")]
    public string Street { get; set; }
    
    public int Number { get; set; }
    
    [MaxLength(245, ErrorMessage = "annotation_neighborhood_max_length")]
    public string Neighborhood { get; set; }
    
    [MaxLength(8, ErrorMessage = "annotation_zip_code_max_length")]
    public string ZipCode { get; set; }
}