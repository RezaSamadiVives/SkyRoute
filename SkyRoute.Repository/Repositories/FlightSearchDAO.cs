using SkyRoute.Domains.Data;
using SkyRoute.Domains.Entities;
using SkyRoute.Repositories.Interfaces;

namespace SkyRoute.Repositories.Repositories
{
    public class FlightSearchDAO(SkyRouteDbContext context) : BaseDAO<Flight>(context),IFlightSearchDAO
    {
        public async Task<IEnumerable<Flight>> SearchFlightsAsync(string fromCity, string toCity, DateTime departureDate, DateTime? returnDate, string tripClass, string tripType, int adultCount, int? kidCount)
        {
            throw new NotImplementedException();
        }

    }
}
