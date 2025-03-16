namespace FishSpotApi.Domain.Projection;

public class SpotUserLocationProjection
{
    public string Id { get; set; }
    public string Title { get; set; }
    public List<double> Coordinates { get; set; }
    public string Image { get; set; }
}