using FishSpotApi.Domain.Enum;
using Microsoft.AspNetCore.Http;

namespace FishSpotApi.Domain.Http.Request;

public class CreateSpotRequest
{
    public string UserId { get; set; }
    public string Title { get; set; }
    public string Observation { get; set; }
    public IEnumerable<double> Coordinates { get; set; }
    public CreateSpotLocationDifficultyRequest LocationDifficulty { get; set; }
    public CreateSpotLocationRiskRequest LocationRisk { get; set; }
    public IEnumerable<IFormFile> Images { get; set; }
    public IEnumerable<CreateSpotFishRequest> Fishs { get; set; }
}

public class CreateSpotLocationRiskRequest
{
    public SpotLocationDifficultyRate Rate { get; set; }
    public string Observation { get; set; }
}

public class CreateSpotLocationDifficultyRequest
{
    public SpotLocationRiskRate Rate { get; set; }
    public string Observation { get; set; }
}

public class CreateSpotFishRequest
{
    public string Name { get; set; }
    public double Weight { get; set; }
    public string UnitMeasure { get; set; }
    public IEnumerable<string> Lures { get; set; }
}