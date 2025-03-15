namespace FishSpotApi.Domain.Projection;

public class SpotLocationProjection
{
    public string Id { get; set; }
    public string Title { get; set; }
    public List<double> Coordinates { get; set; }
}