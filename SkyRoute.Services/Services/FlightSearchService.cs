using SkyRoute.Domains.Entities;
using SkyRoute.Domains.Models;
using SkyRoute.Repositories.Interfaces;
using SkyRoute.Services.Interfaces;

namespace SkyRoute.Services.Services
{
    public class FlightSearchService(IFlightSearchDAO _flightSearchDAO) : BaseService<Flight>(_flightSearchDAO), IFlightSearchService
    {

        public async Task<FlightSegmentGroup> GetAvailableFlights(Guid segmentId, bool isBusiness, int adultCount, int? kidCount)
        {
            return await _flightSearchDAO.GetAvailableFlights(segmentId, isBusiness, adultCount, kidCount);
        }

        public async Task<FlightSearchResult> SearchFlightsAsync(int fromCityId, int toCityId, DateTime departureDate,
            DateTime? returnDate, bool isBusiness, bool isRetour, int adultCount, int? kidCount)
        {
            return await _flightSearchDAO.SearchFlightsAsync(fromCityId, toCityId, departureDate, returnDate, isBusiness, isRetour, adultCount, kidCount);
        }

        public async Task<Flight?> FindFlightWithDetailsAsync(int flightId)
        {
            return await _flightSearchDAO.FindFlightWithDetailsAsync(flightId);
        }
    }
}
