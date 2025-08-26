using SkyRoute.Domains.Entities;

namespace SkyRoute.Services.Interfaces
{
    public interface ITicketService
    {
        Task<List<Ticket>> GetAllTicketsByBooking(int bookingId);

    }
}