using Microsoft.EntityFrameworkCore;
using SkyRoute.Domains.Data;
using SkyRoute.Domains.Entities;
using SkyRoute.Domains.Models;
using SkyRoute.Repositories.Interfaces;

namespace SkyRoute.Repositories.Repositories
{
    public class FlightSearchDAO(SkyRouteDbContext context) : BaseDAO<Flight>(context), IFlightSearchDAO
    {
        public async Task<FlightSearchResult> SearchFlightsAsync(
            int fromCityId,
            int toCityId,
            DateTime departureDate,
            DateTime? returnDate,
            bool isBusiness,
            bool isRetour,
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

            if (isRetour && returnDate.HasValue)
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

            int requiredFlightCount = await _context.RouteStops
                .Where(s => s.RouteId == flightRoute.Id)
                .CountAsync() + 1;

            var startDate = departureDate.Date;

            var validSegmentIds = await _context.Flights
                .Where(f =>
                    f.FlightRouteId == flightRoute.Id &&
                    f.FlightDate >= startDate)
                .GroupBy(f => f.SegmentId)
                .Where(g => g.Count() == requiredFlightCount)
                .OrderBy(g => g.Min(f => f.FlightDate))
                .Select(g => g.Key)
                .Take(3)
                .ToListAsync();

            if (validSegmentIds.Count == 0)
                return Enumerable.Empty<FlightSegmentGroup>();

            var flights = await _context.Flights
                .Where(f => validSegmentIds.Contains(f.SegmentId))
                .Include(f => f.Airline)
                .Include(f => f.FromCity)
                .Include(f => f.ToCity)
                .Include(f => f.FlightRoute)
                .Include(f => f.Seats)
                .Include(f => f.MealOptions)
                .ToListAsync();

            return flights
                .GroupBy(f => f.SegmentId)
                .Where(g => g.Count() == requiredFlightCount &&
                 g.All(f => f.Seats.Count(s => s.IsAvailable && s.IsBusiness == isBusiness) >= passengersCount))
                 .OrderBy(g => g.Min(f => f.FlightDate))
                 .Take(3)
                .Select(g =>
                {
                    var ordered = g.OrderBy(f => f.FlightDate).ThenBy(f => f.DepartureTime).ToList();

                    return new FlightSegmentGroup
                    {
                        SegmentId = g.Key,
                        Flights = ordered,
                        TotalPrice = isBusiness
                            ? ordered.Sum(f => f.PriceBusiness) * passengersCount
                            : ordered.Sum(f => f.PriceEconomy) * passengersCount
                    };
                })
                .ToList();
        }

        public async Task<FlightSegmentGroup> GetAvailableFlights(Guid segmentId, bool isBusiness, int adultCount, int? kidCount)
        {
            int passengersCount = adultCount + (kidCount ?? 0);
            var result = new FlightSegmentGroup() { SegmentId = segmentId};

            var flights = await _context.Flights
               .Where(f => f.SegmentId == segmentId)
               .Include(f => f.Airline)
               .Include(f => f.FromCity)
               .Include(f => f.ToCity)
               .Include(f => f.FlightRoute)
               .Include(f => f.Seats)
               .Include(f => f.MealOptions)
               .ToListAsync();

            bool allFlightsHaveEnoughSeats = flights.Count > 0 && 
                flights.All(f =>f.Seats.Count(s => s.IsAvailable && s.IsBusiness == isBusiness) >= passengersCount);

            if (allFlightsHaveEnoughSeats)
            {
                result.Flights = flights;
                result.TotalPrice = isBusiness ? flights.Sum(f => f.PriceBusiness) : flights.Sum(f => f.PriceEconomy);
            }

            return result;
        }
    }
}
