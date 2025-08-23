using SkyRoute.Domains.Enums;

namespace SkyRoute.Domains.Entities
{
    public class Ticket
    {
        public int Id { get; set; }

        public int BookingId { get; set; }
        public virtual Booking Booking { get; set; } = null!;

        public int PassengerId { get; set; }
        public virtual Passenger Passenger { get; set; } = null!;
        
        public int FlightId { get; set; }
        public virtual Flight Flight { get; set; } = null!;

        
        public int SeatId { get; set; }
        public virtual Seat Seat { get; set; } = null!;

        public int MealOptionId { get; set; }
        public virtual MealOption MealOption { get; set; } = null!;

        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        public decimal Price { get; set; }

        public TicketStatus Status { get; set; } = TicketStatus.Confirmed;

    }
}