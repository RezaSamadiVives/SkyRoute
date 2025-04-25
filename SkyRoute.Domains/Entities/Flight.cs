namespace SkyRoute.Domains.Entities
{
    public class Flight
    {
        public int Id { get; set; }
        public required string FromCity { get; set; }
        public required string ToCity { get; set; }
        public required string Airline { get; set; }
        public required DateTime FlightDate { get; set; }
        public required TimeSpan DepartureTime { get; set; }
        public required DateTime ArrivalTime { get; set; }
        public required decimal PriceEconomy { get; set; }
        public required decimal PriceBusiness { get; set; }

        public required ICollection<Seat> Seats { get; set; }
        public ICollection<FlightMealOption> MealOptions { get; set; }
    }
}
