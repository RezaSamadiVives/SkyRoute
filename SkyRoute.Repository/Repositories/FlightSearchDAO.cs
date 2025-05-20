using Microsoft.EntityFrameworkCore;
using SkyRoute.Domains.Data;
using SkyRoute.Domains.Entities;
using SkyRoute.Repositories.Interfaces;
using SkyRoute.Repositories.Models;

namespace SkyRoute.Repositories.Repositories
{
    public class FlightSearchDAO : BaseDAO<Flight>, IFlightSearchDAO
    {
        private readonly SkyRouteDbContext _context;

        public FlightSearchDAO(SkyRouteDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<FlightSearchResult> SearchFlightsAsync(
            int fromCityId,
            int toCityId,
            DateTime departureDate,
            DateTime? returnDate,
            bool isBusiness,
            bool retour,
            int adultCount,
            int? kidCount)
        {
            var result = new FlightSearchResult
            {
                OutboundFlights = [],
                ReturnFlights = []
            };

            int passengersCount = adultCount + (kidCount ?? 0);

            var outboundFlights = await GetFlightsForRoute(fromCityId, toCityId, departureDate, isBusiness, passengersCount);
            result.OutboundFlights.AddRange(outboundFlights);

            if (retour && returnDate.HasValue)
            {
                var returnFlights = await GetFlightsForRoute(toCityId, fromCityId, returnDate.Value, isBusiness, passengersCount);
                result.ReturnFlights.AddRange(returnFlights);
            }

            return result;
        }

        public async Task<IEnumerable<FlightSegmentGroup>> GetFlightsForRoute(
            int fromCityId,
            int toCityId,
            DateTime departureDate,
            bool isBusiness,
            int passengersCount)
        {
            var flightRoute = await _context.FlightRoutes
                .FirstOrDefaultAsync(r => r.FromCityId == fromCityId && r.ToCityId == toCityId);

            if (flightRoute == null)
                return Enumerable.Empty<FlightSegmentGroup>();

            var flights =  await _context.Flights
                .Include(f => f.Seats)
                .Include(f => f.MealOptions)
                .Where(f =>
                    f.FlightRouteId == flightRoute.Id &&
                    f.FlightDate == departureDate.Date &&
                    f.Seats.Count(s => s.IsAvailable && s.IsBusiness == isBusiness) >= passengersCount)
                .ToListAsync();

            return flights
                .GroupBy(f => f.SegmentId)
                .Select(g => new FlightSegmentGroup
                {
                    SegmentId = g.Key,
                    Flights = g.OrderBy(f => f.DepartureTime).ToList(),
                    TotalPrice = isBusiness ? g.Sum(f => f.PriceBusiness): g.Sum(f => f.PriceEconomy)})
                .OrderBy(g => g.Flights.First().Airline).ToList();

        }
    }
}
