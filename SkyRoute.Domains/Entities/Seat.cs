namespace SkyRoute.Domains.Entities
{
    public class Seat
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public required string SeatNumber { get; set; }
        public required bool IsBusiness { get; set; }
        public required bool IsAvailable { get; set; }

        public virtual required Flight Flight { get; set; }
    }
}
