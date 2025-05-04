using SkyRoute.Domains.Entities;
using SkyRoute.Repositories.Interfaces;

namespace SkyRoute.Services.Services
{
    public class FlightSearchService : BaseService<Flight>, IFlightSearchDAO
    {
        private readonly IDAO<Flight> _dao;

        public FlightSearchService(IDAO<Flight> dao): base(dao)
        {
            _dao = dao;
        }
        public Task<IEnumerable<Flight>> SearchFlightsAsync(string fromCity, string toCity, DateTime departureDate, 
            DateTime? returnDate, string tripClass, string tripType, int adultCount, int? kidCount)
        {
            throw new NotImplementedException();
        }
    }
}
