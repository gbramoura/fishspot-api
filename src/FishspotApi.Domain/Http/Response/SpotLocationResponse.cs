namespace FishspotApi.Domain.Http.Response
{
    public class SpotLocationResponse
    {
        public string Id { get; set; }
        public IEnumerable<double> Coordinates { get; set; }
    }
}