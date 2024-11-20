using FishspotApi.Core.Repository;
using FishspotApi.Domain.Exception;
using FishspotApi.Domain.Http.Response;

namespace FishspotApi.Core.Services
{
    public class SpotService(SpotRepository spotRepository)
    {
        private readonly SpotRepository _spotRepository = spotRepository;

        public IEnumerable<SpotLocationResponse> GetNearLocations()
        {
            // TODO: include the near location
            var locations = _spotRepository.GetLocations();
            var response = locations.Select(location => new SpotLocationResponse
            {
                Id = location.Id,
                Coordinates = location.Coordinates
            });

            return response;
        }

        public SpotResponse GetSpot(string id)
        {
            var spot = _spotRepository.Get(id);

            if (spot is null)
            {
                throw new SpotNotFoundException("Spot not found");
            }

            var response = new SpotResponse()
            {
                Title = spot.Title,
                Observation = spot.Observation,
                Coordinates = spot.Coordinates,
                Images = spot.Images,
                User = new SpotUserResponse()
                {
                    Name = spot.User.Name,
                },
                LocationRisk = new SpotRateResponse
                {
                    Rate = spot.LocationRisk.Rate.ToString(),
                    Observation = spot.LocationRisk.Observation
                },
                LocationDificulty = new SpotRateResponse
                {
                    Rate = spot.LocationDificulty.Rate.ToString(),
                    Observation = spot.LocationDificulty.Observation
                },
                Fishs = spot.Fishs.Select(fish => new SpotFishResponse
                {
                    Name = fish.Name,
                    Weight = fish.Weight,
                    UnitMeasure = fish.UnitMeasure,
                    Lures = fish.Lures,
                })
            };

            return response;
        }

        public void DeleteSpot(string id)
        {
            var spot = _spotRepository.Get(id);

            if (spot is null)
            {
                throw new SpotNotFoundException("Spot not found");
            }

            _spotRepository.Delete(id);
        }
    }
}