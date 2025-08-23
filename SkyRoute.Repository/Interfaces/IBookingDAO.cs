using SkyRoute.Domains.Entities;
using SkyRoute.Domains.Models;

namespace SkyRoute.Repositories.Interfaces
{
    public interface IBookingDAO : IDAO<Booking>
    {
        Task<Booking> GetBooking(BookingRequest bookingRequest);
    }
}