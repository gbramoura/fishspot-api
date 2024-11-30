namespace FishSpotApi.Domain.Http.Response;

public class SpotLocationResponse
{
    public string Id { get; set; }
    public List<double> Coordinates { get; set; }
}