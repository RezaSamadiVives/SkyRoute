using SkyRoute.Domains.Entities;
using SkyRoute.Domains.Models;

namespace SkyRoute.Services.Interfaces
{
    public interface IBookingService : IService<Booking>
    {
        Task<Booking> CreateBookingAsync(BookingRequest bookingRequest);
        Task<List<Booking>> GetAllBookingAsyncByUser(string userId);
        Task CancelBookingAsync(int bookingId);
    }
}