using SkyRoute.Domains.Entities;
using SkyRoute.Repositories.Models;

namespace SkyRoute.Repositories.Interfaces
{
    public interface IFlightSearchDAO : IDAO<Flight>
    {
        Task<FlightSearchResult> SearchFlightsAsync(int fromCityId,int toCityId,
            DateTime departureDate,DateTime? returnDate, bool IsEconomy, bool Retour,int adultCount,int? kidCount);
    }
}
