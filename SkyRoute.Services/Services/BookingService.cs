using SkyRoute.Domains.Entities;
using SkyRoute.Domains.Models;
using SkyRoute.Repositories.Interfaces;
using SkyRoute.Services.Interfaces;

namespace SkyRoute.Services.Services
{
    public class BookingService(IBookingDAO _bookingDAO) : BaseService<Booking>(_bookingDAO), IBookingService
    {

        public async Task<Booking> CreateBookingAsync(BookingRequest bookingRequest)
        {
            return await _bookingDAO.CreateBookingAsync(bookingRequest);
        }

        public async Task<List<Booking>> GetAllBookingAsyncByUser(string userId)
        {
            return await _bookingDAO.GetAllBookingAsyncByUser(userId);
        }
        
         public async Task CancelBookingAsync(int bookingId)
        {
           await _bookingDAO.CancelBookingAsync(bookingId);
        }

    }
}