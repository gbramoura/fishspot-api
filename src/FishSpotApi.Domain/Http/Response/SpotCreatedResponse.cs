namespace FishSpotApi.Domain.Http.Response;

public class SpotCreatedResponse
{
    public string Id { get; set; }
    public IEnumerable<double> Coordinates { get; set; }
}