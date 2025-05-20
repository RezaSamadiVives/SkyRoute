using SkyRoute.Domains.Entities;
using SkyRoute.Repositories.Models;

namespace SkyRoute.Services.Interfaces
{
    public interface IFlightSearchService : IService<Flight>
    {
        Task<FlightSearchResult> SearchFlightsAsync(int fromCityId, int toCityId,
            DateTime departureDate, DateTime? returnDate, bool isBusiness, bool Retou, int adultCount, int? kidCount);
    }
}
