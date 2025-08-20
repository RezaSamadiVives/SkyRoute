using Microsoft.AspNetCore.Identity;
using SkyRoute.Domains.Enums;

namespace SkyRoute.Domains.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public string Reference { get; set; } = Guid.NewGuid().ToString("N")[..8].ToUpper();
        public DateTime BookingDate { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Confirmed;
        public required string UserId { get; set; }
        public virtual IdentityUser User { get; set; } = null!;

        public ICollection<Ticket> Tickets { get; set; } = [];
    }
}