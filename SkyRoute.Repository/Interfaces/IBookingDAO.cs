using SkyRoute.Domains.Entities;
using SkyRoute.Domains.Models;

namespace SkyRoute.Repositories.Interfaces
{
    public interface IBookingDAO : IDAO<Booking>
    {
        Task<Booking> CreateBookingAsync(BookingRequest bookingRequest);
        Task<List<Booking>> GetAllBookingAsyncByUser(string userId);
        Task CancelBookingAsync(int bookingId);
    }
}