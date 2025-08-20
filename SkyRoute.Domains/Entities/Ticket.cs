using SkyRoute.Domains.Enums;

namespace SkyRoute.Domains.Entities
{
    public class Ticket
    {
        public int Id { get; set; }

        public int BookingId { get; set; }
        public Booking Booking { get; set; } = null!;

       
        public int PassengerId { get; set; }
        public Passenger Passenger { get; set; } = null!;

        
        public int FlightId { get; set; }
        public Flight Flight { get; set; } = null!;

        
        public int SeatId { get; set; }
        public Seat Seat { get; set; } = null!;

        public int MealOptionId { get; set; }
        public MealOption MealOption { get; set; } = null!;

        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        public decimal Price { get; set; }

        public TicketStatus Status { get; set; } = TicketStatus.Confirmed;

    }
}