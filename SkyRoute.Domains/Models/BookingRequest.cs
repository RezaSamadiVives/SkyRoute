namespace SkyRoute.Domains.Models
{
    public class BookingRequest
    {
        public required string UserId { get; set; }
        public required Guid SegmentId { get; set; }
        public bool IsBusiness { get; set; }
        public List<PassengerFlightMeals> PassengerFlightMeals { get; set; } = [];
    }
}