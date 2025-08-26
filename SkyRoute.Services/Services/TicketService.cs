using SkyRoute.Domains.Entities;
using SkyRoute.Domains.Models;
using SkyRoute.Repositories.Interfaces;
using SkyRoute.Services.Interfaces;

namespace SkyRoute.Services.Services
{
    public class TicketService(ITicketDAO _ticketDAO) : BaseService<Ticket>(_ticketDAO), ITicketService
    {
        public async Task<List<Ticket>> GetAllTicketsByBooking(int bookingId)
        {
            return await _ticketDAO.GetAllTicketsByBooking(bookingId);
        }
    }
}