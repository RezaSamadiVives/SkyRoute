using SkyRoute.Domains.Entities;
using SkyRoute.Domains.Models;

namespace SkyRoute.Repositories.Interfaces
{
    public interface IFlightSearchDAO : IDAO<Flight>
    {
        Task<FlightSearchResult> SearchFlightsAsync(int fromCityId,int toCityId,
            DateTime departureDate,DateTime? returnDate, bool isBusiness, bool isRetour,int adultCount,int? kidCount);

        Task<FlightSegmentGroup> GetAvailableFlights(Guid segmentId, bool isBusiness, int adultCount, int? kidCount);
    }
}
