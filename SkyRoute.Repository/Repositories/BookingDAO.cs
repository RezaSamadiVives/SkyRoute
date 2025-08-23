using Microsoft.EntityFrameworkCore;
using SkyRoute.Domains.Data;
using SkyRoute.Domains.Entities;
using SkyRoute.Domains.Models;
using SkyRoute.Repositories.Interfaces;

namespace SkyRoute.Repositories.Repositories
{
    public class BookingDAO(SkyRouteDbContext context) : BaseDAO<Booking>(context), IBookingDAO
    {
        public Task<Booking> GetBooking(BookingRequest bookingRequest)
        {
            throw new NotImplementedException();
        }
    }
}