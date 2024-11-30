namespace FishSpotApi.Domain.Http.Response;

public class SpotCreatedResponse
{
    public string Id { get; set; }
    public List<double> Coordinates { get; set; }
}