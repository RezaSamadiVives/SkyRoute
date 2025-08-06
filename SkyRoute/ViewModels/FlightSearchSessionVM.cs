using SkyRoute.Helpers;

namespace SkyRoute.ViewModels
{
    public class FlightSearchSessionVM
    {
        public required string ToCity { get; set; }
        public required int ToCityId { get; set; }
        public required string FromCity { get; set; }
        public required int FromCityId { get; set; }
        public DateTime OutboundFlightDate { get; set; }
        public DateTime? ReturnFlightDate { get; set; }
        public int AdultPassengers { get; set; }
        public int? KidsPassengers { get; set; }
        public TripClass TripClass { get; set; }
        public TripType TripType { get; set; }
        
    }
}