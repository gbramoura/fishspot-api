namespace FishSpotApi.Domain.Projection;

public class SpotLocationProjection
{
    public string Id { get; set; }
    public IEnumerable<double> Coordinates { get; set; }
}