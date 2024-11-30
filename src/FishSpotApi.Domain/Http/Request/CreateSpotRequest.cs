using System.ComponentModel.DataAnnotations;
using FishSpotApi.Domain.Enum;
using Microsoft.AspNetCore.Http;

namespace FishSpotApi.Domain.Http.Request;

public class UpdateSpotRequest : CreateSpotRequest;

public class CreateSpotRequest
{
    [Required(ErrorMessage = "The title must be filled")]
    public string Title { get; set; }
    
    [MaxLength(245, ErrorMessage = "The limit of characters 245 has been reached")]
    public string Observation { get; set; }
    
    [Required(ErrorMessage = "The coordinates must be filled")]
    [MinLength(2, ErrorMessage = "The coordinates must be at least 2 characters long")]
    [MaxLength(2, ErrorMessage = "The coordinates must be at most 2 characters long")]
    public IEnumerable<double> Coordinates { get; set; }
    
    public CreateSpotLocationDifficultyRequest LocationDifficulty { get; set; }
    
    public CreateSpotLocationRiskRequest LocationRisk { get; set; }
    
    [MinLength(1, ErrorMessage = "Minimum length of fishes required")]
    public IEnumerable<CreateSpotFishRequest> Fishes { get; set; }
}

public class CreateSpotLocationRiskRequest
{
    [Required(ErrorMessage = "The location difficulty rate must be filled")]
    public SpotLocationRiskRate Rate { get; set; }
    
    [MaxLength(245, ErrorMessage = "The limit of characters 245 has been reached")]
    public string Observation { get; set; }
}

public class CreateSpotLocationDifficultyRequest
{
    [Required(ErrorMessage = "The location risk rate must be filled")]
    public SpotLocationDifficultyRate Rate { get; set; }
    
    [MaxLength(245, ErrorMessage = "The limit of characters 245 has been reached")]
    public string Observation { get; set; }
}

public class CreateSpotFishRequest
{
    [Required(ErrorMessage = "The name must be filled")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "The weight must be filled")]
    public double Weight { get; set; }
    
    [Required(ErrorMessage = "The unit measure must be filled")]
    public UnitMeasureType UnitMeasure { get; set; }
    
    [MinLength(1, ErrorMessage = "The minimum number of lures must be at least one")]
    [MaxLength(3, ErrorMessage = "The limit of lures has been reached")]
    public IEnumerable<string> Lures { get; set; }
}