namespace SkyRoute.Domains.Models
{
    public class BookingRequest
    {
        public required string UserId { get; set; }
        public required Guid SegmentIdOutbound { get; set; }
        public Guid? SegmentIdRetour { get; set; }
        public bool IsBusiness { get; set; }
        public int PassengersCount { get; set; }
        public List<PassengerFlightMeals> PassengerFlightMeals { get; set; } = [];
    }
}