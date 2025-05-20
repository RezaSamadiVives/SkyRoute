namespace SkyRoute.Repositories.Models
{
    public class FlightSearchResult
    {
        public  List<FlightSegmentGroup>? OutboundFlights { get; set; } = [];
        public List<FlightSegmentGroup>? ReturnFlights { get; set; } = [];
    }
}
