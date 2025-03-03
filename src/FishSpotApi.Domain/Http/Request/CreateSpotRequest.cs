using System.ComponentModel.DataAnnotations;
using FishSpotApi.Domain.Enum;
using Microsoft.AspNetCore.Http;

namespace FishSpotApi.Domain.Http.Request;

public class UpdateSpotRequest : CreateSpotRequest;

public class CreateSpotRequest
{
    [Required(ErrorMessage = "annotation_title_required")]
    public string Title { get; set; }
    
    [MaxLength(245, ErrorMessage = "annotation_observation_max_length")]
    public string Observation { get; set; }
    
    [Required(ErrorMessage = "annotation_coordinates_required")]
    [MinLength(2, ErrorMessage = "annotation_coordinates_min_length")]
    [MaxLength(2, ErrorMessage = "annotation_coordinates_max_length")]
    public List<double> Coordinates { get; set; }
    
    public CreateSpotLocationDifficultyRequest LocationDifficulty { get; set; }
    
    public CreateSpotLocationRiskRequest LocationRisk { get; set; }
    
    [MinLength(1, ErrorMessage = "annotation_fishs_min_length")]
    public List<CreateSpotFishRequest> Fishes { get; set; }
}

public class CreateSpotLocationRiskRequest
{
    [Required(ErrorMessage = "annotation_rate_required")]
    public SpotLocationRiskRate Rate { get; set; }
    
    [MaxLength(245, ErrorMessage = "annotation_observation_max_length")]
    public string Observation { get; set; }
}

public class CreateSpotLocationDifficultyRequest
{
    [Required(ErrorMessage = "annotation_risk_required")]
    public SpotLocationDifficultyRate Rate { get; set; }
    
    [MaxLength(245, ErrorMessage = "annotation_observation_max_length")]
    public string Observation { get; set; }
}

public class CreateSpotFishRequest
{
    [Required(ErrorMessage = "annotation_name_required")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "annotation_weight_required")]
    public double Weight { get; set; }
    
    [Required(ErrorMessage = "annotation_unit_measure_required")]
    public UnitMeasureType UnitMeasure { get; set; }
    
    [MinLength(1, ErrorMessage = "annotation_lures_min_length")]
    [MaxLength(3, ErrorMessage = "annotation_lures_max_length")]
    public List<string> Lures { get; set; }
}