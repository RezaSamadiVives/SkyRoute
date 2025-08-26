using Microsoft.EntityFrameworkCore;
using SkyRoute.Domains.Data;
using SkyRoute.Domains.Entities;
using SkyRoute.Domains.Enums;
using SkyRoute.Domains.Models;
using SkyRoute.Repositories.Interfaces;
using SkyRoute.Repositories.Services;

namespace SkyRoute.Repositories.Repositories
{
    public class BookingDAO(
        SkyRouteDbContext _context,
        ISeatAllocatorService _seatAllocatorService) : BaseDAO<Booking>(_context), IBookingDAO
    {
        public async Task<Booking> CreateBookingAsync(BookingRequest bookingRequest)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var outboundFlights = await _context.Flights
                .Include(f => f.Seats)
                .Where(f => f.SegmentId == bookingRequest.SegmentIdOutbound).ToListAsync();



                if (outboundFlights.Count == 0)
                {
                    throw new InvalidOperationException("Geen heenvluchten gevonden");
                }

                List<Flight> retourFlights = [];

                if (string.IsNullOrEmpty(bookingRequest.SegmentIdRetour.ToString()))
                {
                    retourFlights = await _context.Flights
                    .Include(f => f.Seats)
                    .Where(f => f.SegmentId == bookingRequest.SegmentIdRetour).ToListAsync();

                    if (retourFlights.Count == 0)
                    {
                        throw new InvalidOperationException("Geen terugvluchten gevonden");
                    }
                }

                var booking = new Booking
                {
                    UserId = bookingRequest.UserId,
                    BookingDate = DateTime.UtcNow
                };

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                var alleFlights = outboundFlights.Concat(retourFlights).ToList();
                var flightSeatsList = new List<(Flight, List<Seat>)>();

                foreach (var flight in alleFlights)
                {
                    var seats = await _seatAllocatorService.AllocateSeatsAsync(flight.Id, bookingRequest.PassengersCount, bookingRequest.IsBusiness);
                    flightSeatsList.Add((flight, seats));
                }

                foreach (var passengerModel in bookingRequest.PassengerFlightMeals)
                {
                    var passenger = await GetOrCreatePassengerAsync(passengerModel, bookingRequest);

                    foreach (var flight in outboundFlights)
                    {

                        var mealChoice = passengerModel.MealChoics.First(f => f.FlightId == flight.Id);
                        var seat = flightSeatsList.First(x => x.Item1.Id == flight.Id)
                        .Item2.First(x => x.IsAvailable == true);

                        seat.IsAvailable = false;

                        var ticket = new Ticket
                        {
                            BookingId = booking.Id,
                            PassengerId = passenger.Id,
                            FlightId = flight.Id,
                            SeatId = seat.Id,
                            MealOptionId = mealChoice.MealOptionsId,
                            Price = bookingRequest.IsBusiness ? flight.PriceBusiness : flight.PriceEconomy,
                            Status = TicketStatus.Confirmed,
                            IsBusiness = bookingRequest.IsBusiness
                        };

                        _context.Tickets.Add(ticket);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return booking;

            }

            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<List<Booking>> GetAllBookingAsyncByUser(string userId)
        {
            var bookings = await _context.Bookings
                    .Where(b => b.UserId == userId)
                    .Include(b => b.Tickets) 
                        .ThenInclude(t => t.Passenger) 
                    .Include(b => b.Tickets)
                        .ThenInclude(t => t.Flight) 
                            .ThenInclude(f => f.FromCity) 
                    .Include(b => b.Tickets)
                        .ThenInclude(t => t.Flight)
                            .ThenInclude(f => f.ToCity) 
                    .Include(b => b.Tickets)
                        .ThenInclude(t => t.Seat) 
                    .Include(b => b.Tickets)
                        .ThenInclude(t => t.MealOption)
                    .ToListAsync();

            return bookings;

        }


        public async Task CancelBookingAsync(int bookingId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var booking = await _context.Bookings
                    .Include(b => b.Tickets)
                        .ThenInclude(t => t.Seat)
                    .FirstOrDefaultAsync(b => b.Id == bookingId);

                if (booking == null)
                {
                    throw new InvalidOperationException("Booking niet gevonden.");
                }

                booking.Status = BookingStatus.cancelled;
                _context.Bookings.Update(booking);

                foreach (var ticket in booking.Tickets)
                {
                    ticket.Status = TicketStatus.cancelled;

                    if (ticket.Seat != null)
                    {
                        ticket.Seat.IsAvailable = true;
                        _context.Seats.Update(ticket.Seat);
                    }

                    _context.Tickets.Update(ticket);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task<Passenger> GetOrCreatePassengerAsync(PassengerFlightMeals passengerModel, BookingRequest bookingRequest)
        {
            var passenger = await _context.Passengers
                .FirstOrDefaultAsync(p =>
                    p.UserId == bookingRequest.UserId &&
                    p.FirstName == passengerModel.FirstName &&
                    p.LastName == passengerModel.LastName &&
                    p.Birthday == passengerModel.Birthday);

            if (passenger != null) return passenger;

            passenger = new Passenger
            {
                UserId = bookingRequest.UserId,
                FirstName = passengerModel.FirstName,
                MiddelName = passengerModel.MiddelName,
                LastName = passengerModel.LastName,
                Birthday = passengerModel.Birthday,
                IsFellowPassenger = passengerModel.IsFellowPassenger
            };

            _context.Passengers.Add(passenger);
            await _context.SaveChangesAsync();

            return passenger;
        }
    }
}