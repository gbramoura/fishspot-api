namespace FishspotApi.Domain.Http.Response
{
    public class SpotResponse
    {
        public string Title { get; set; }
        public string Observation { get; set; }
        public IEnumerable<double> Coordinates { get; set; }
        public SpotRateResponse LocationDificulty { get; set; }
        public SpotRateResponse LocationRisk { get; set; }
        public IEnumerable<string> Images { get; set; }
        public IEnumerable<SpotFishResponse> Fishs { get; set; }
        public SpotUserResponse User { get; set; }
    }

    public class SpotUserResponse
    {
        public string Name { get; set; }
    }

    public class SpotRateResponse
    {
        public string Rate { get; set; }
        public string Observation { get; set; }
    }

    public class SpotFishResponse
    {
        public string Name { get; set; }
        public string UnitMeasure { get; set; }
        public double Weight { get; set; }
        public IEnumerable<string> Lures { get; set; }
    }
}