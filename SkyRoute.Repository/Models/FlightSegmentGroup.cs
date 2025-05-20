using SkyRoute.Domains.Entities;

namespace SkyRoute.Repositories.Models
{
    public class FlightSegmentGroup
    {
        public Guid SegmentId { get; set; }
        public List<Flight> Flights { get; set; } = [];
        public TimeSpan TotalDuration => CalculateTotalDuration();
        public decimal TotalPrice { get; set; }

        private TimeSpan CalculateTotalDuration()
        {
            if (Flights.Count == 0)
                return TimeSpan.Zero;

            var first = Flights.First();
            var last = Flights.Last();

            var departure = first.FlightDate + first.DepartureTime;
            var arrival = last.ArrivalDate + last.ArrivalTime;

            return arrival - departure;
        }
    }
}
