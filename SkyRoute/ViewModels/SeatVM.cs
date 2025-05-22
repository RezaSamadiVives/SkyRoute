namespace SkyRoute.ViewModels
{
    public class SeatVM
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public required string SeatNumber { get; set; }
        public required bool IsBusiness { get; set; }
        public required bool IsAvailable { get; set; }

        public required FlightVM Flight { get; set; }
    }
}
