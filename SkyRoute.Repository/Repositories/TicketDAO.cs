using Microsoft.EntityFrameworkCore;
using SkyRoute.Domains.Data;
using SkyRoute.Domains.Entities;
using SkyRoute.Repositories.Interfaces;

namespace SkyRoute.Repositories.Repositories
{
    public class TicketDAO(SkyRouteDbContext context) : BaseDAO<Ticket>(context), ITicketDAO
    {
        public async Task<List<Ticket>> GetAllTicketsByBooking(int bookingId)
        {
             var tickets = await _context.Tickets
                .Where(t => t.BookingId == bookingId)
                .Include(t => t.Passenger)             // Haal passagier op
                .Include(t => t.Flight)                // Haal vlucht op
                    .ThenInclude(f => f.FromCity)      // Van vlucht: vertrekstad
                .Include(t => t.Flight)
                    .ThenInclude(f => f.ToCity)        // Van vlucht: aankomststad
                .Include(t => t.Seat)                   // Haal stoel op
                .Include(t => t.MealOption)            // Haal maaltijdoptie op
                .Include(t => t.Booking)               // Haal booking op
                .ToListAsync();

            return tickets;
        }

    }
}
