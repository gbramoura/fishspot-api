namespace FishSpotApi.Domain.Projection;

public class SpotLocationProjection
{
    public string Id { get; set; }
    public List<double> Coordinates { get; set; }
}