namespace SkyRoute.ViewModels
{
    public class FlightSegmentGroupVM
    {
        public Guid SegmentId { get; set; }
        public List<FlightVM> Flights { get; set; } = [];
        public TimeSpan TotalDuration { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
