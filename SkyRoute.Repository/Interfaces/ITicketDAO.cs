using SkyRoute.Domains.Entities;

namespace SkyRoute.Repositories.Interfaces
{
    public interface ITicketDAO : IDAO<Ticket>
    {
        Task<List<Ticket>> GetAllTicketsByBooking(int bookingId);
    }
}