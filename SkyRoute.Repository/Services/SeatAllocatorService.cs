using Microsoft.EntityFrameworkCore;
using SkyRoute.Domains.Data;
using SkyRoute.Domains.Entities;

namespace SkyRoute.Repositories.Services
{
    public class SeatAllocatorService(SkyRouteDbContext _context) : ISeatAllocatorService
    {

        public async Task<List<Seat>> AllocateSeatsAsync(int flightId, int passengerCount, bool isBusiness)
        {

            var seatsList = await _context.Seats
                .Where(s =>
                    s.FlightId == flightId &&
                    s.IsAvailable &&
                    s.IsBusiness == isBusiness)
                .ToListAsync();

            var seats = seatsList
                .OrderBy(s => ExtractRow(s.SeatNumber))
                .ThenBy(s => ExtractColumn(s.SeatNumber))
                .ToList();

            if (seats.Count == 0 || seats.Count < passengerCount)
                throw new InvalidOperationException("Geen stoelen beschikbaar voor deze vlucht/klasse");


            var groupedByRow = seats.GroupBy(s => ExtractRow(s.SeatNumber));

            foreach (var rowGroup in groupedByRow)
            {
                var rowSeats = rowGroup.OrderBy(s => ExtractColumn(s.SeatNumber)).ToList();

                if (rowSeats.Count >= passengerCount)
                {

                    return [.. rowSeats.Take(passengerCount)];
                }
            }

            return [.. seats.Take(passengerCount)];
        }

        private static int ExtractRow(string seatNumber)
        {
            var digits = new string([.. seatNumber.TakeWhile(char.IsDigit)]);
            return int.Parse(digits);
        }

        private static char ExtractColumn(string seatNumber)
        {
            return seatNumber.First(c => char.IsLetter(c));
        }
    }
}