using SkyRoute.Domains.Entities;
using SkyRoute.Domains.Models;
using SkyRoute.Repositories.Interfaces;
using SkyRoute.Services.Interfaces;

namespace SkyRoute.Services.Services
{
    public class BookingService(IBookingDAO _bookingDAO) : BaseService<Booking>(_bookingDAO), IBookingService
    {
        public async Task<Booking> GetBooking(BookingRequest bookingRequest)
        {
            return await _bookingDAO.GetBooking( bookingRequest );
        }
    }
}