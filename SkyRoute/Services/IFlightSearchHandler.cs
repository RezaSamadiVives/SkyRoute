using SkyRoute.ViewModels;

namespace SkyRoute.Services
{
    public interface IFlightSearchHandler
    {
        Task<FlightSearchResultVM> HandleFlightSearchAsync(HttpContext context, int? fromCityId, string? fromCity, int? toCityId, string? toCity,
            DateTime? departureDate, DateTime? returnDate, bool? isRetour, bool? isBusiness, int? adultPassengers, int? kidsPassengers);

        Task<object> GetSelectedFlightSegmentAsync(FlightSelectionVM selection, ISession session);

    }
}