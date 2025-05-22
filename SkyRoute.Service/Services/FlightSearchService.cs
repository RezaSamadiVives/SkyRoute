using SkyRoute.Domains.Entities;
using SkyRoute.Domains.Models;
using SkyRoute.Repositories.Interfaces;
using SkyRoute.Services.Interfaces;

namespace SkyRoute.Services.Services
{
    public class FlightSearchService(IFlightSearchDAO flightSearchDAO) : BaseService<Flight>(flightSearchDAO), IFlightSearchService
    {
        private readonly IFlightSearchDAO _flightSearchDAO = flightSearchDAO;

        public async Task<FlightSearchResult> SearchFlightsAsync(int fromCityId, int toCityId, DateTime departureDate, 
            DateTime? returnDate, bool isBusiness, bool isRetour, int adultCount, int? kidCount)
        {
            return await _flightSearchDAO.SearchFlightsAsync(fromCityId, toCityId, departureDate, returnDate, isBusiness, isRetour, adultCount, kidCount);
        }
    }
}
