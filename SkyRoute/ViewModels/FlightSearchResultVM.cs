namespace SkyRoute.ViewModels
{
    public class FlightSearchResultVM
    {
        public List<FlightSegmentGroupVM>? OutboundFlights { get; set; }
        public List<FlightSegmentGroupVM>? ReturnFlights { get; set; }
        public FlightSearchFormVM? FormModel { get; internal set; }
    }
}
