namespace SkyRoute.Domains.Entities
{
    public class Flight
    {
        public int Id { get; set; }

        public int FromCityId { get; set; }
        public City FromCity { get; set; } = null!;

        public int ToCityId { get; set; }
        public City ToCity { get; set; } = null!;

        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        public string FlightNumber { get; set; } = null!;
        public bool HasStopover { get; set; }

        public string? StopoverCities { get; set; }
    }

}
