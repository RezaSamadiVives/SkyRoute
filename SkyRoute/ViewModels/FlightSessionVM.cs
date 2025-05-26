namespace SkyRoute.ViewModels
{
    public class FlightSessionVM
    {
        public int Id { get; set; }
        public required string FlightNumber { get; set; }
        public int FromCityId { get; set; }
        public int ToCityId { get; set; }
        public int AirlineId { get; set; }
        public int FlightRouteId { get; set; }
        public Guid SegmentId { get; set; }
        public DateTime FlightDate { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public DateTime ArrivalDate { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public decimal PriceEconomy { get; set; }
        public decimal PriceBusiness { get; set; }

    }
}
