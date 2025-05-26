namespace SkyRoute.ViewModels
{
    public class FlightSegmentSessionVM
    {
        public Guid SegmentId { get; set; }
        public List<int> Flights { get; set; } = [];
        public TimeSpan TotalDuration { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
