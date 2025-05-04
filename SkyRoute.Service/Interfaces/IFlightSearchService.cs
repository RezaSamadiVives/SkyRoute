using SkyRoute.Domains.Entities;

namespace SkyRoute.Services.Interfaces
{
    public interface IFlightSearchService : IService<Flight>
    {
        Task<IEnumerable<Flight>> SearchFlightsAsync(string fromCity, string toCity,
            DateTime departureDate, DateTime? returnDate, string tripClass, string tripType, int adultCount, int? kidCount);
    }
}
