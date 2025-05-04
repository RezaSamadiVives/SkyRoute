using SkyRoute.Domains.Entities;

namespace SkyRoute.Repositories.Interfaces
{
    public interface IFlightSearchDAO : IDAO<Flight>
    {
        Task<IEnumerable<Flight>> SearchFlightsAsync(string fromCity,string toCity,
            DateTime departureDate,DateTime? returnDate, string tripClass, string tripType,int adultCount,int? kidCount);
    }
}
