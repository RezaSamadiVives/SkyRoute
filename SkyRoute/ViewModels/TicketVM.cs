using SkyRoute.Domains.Enums;

namespace SkyRoute.ViewModels
{
    public class TicketVM
    {
        public int Id { get; set; }

        public int BookingId { get; set; }
        public virtual BookingVM Booking { get; set; } = null!;

        public int PassengerId { get; set; }
        public virtual PassengerVM Passenger { get; set; } = null!;

        public int FlightId { get; set; }
        public virtual FlightVM Flight { get; set; } = null!;


        public int SeatId { get; set; }
        public virtual SeatVM Seat { get; set; } = null!;

        public int MealOptionId { get; set; }
        public virtual MealOptionVM MealOption { get; set; } = null!;

        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        public decimal Price { get; set; }

        public bool IsBusiness { get; set; } = false;

        public TicketStatus Status { get; set; } = TicketStatus.Confirmed;
    }
}