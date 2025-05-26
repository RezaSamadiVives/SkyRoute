namespace SkyRoute.ViewModels
{
    public class FlightSelectionVM
    {
        public Guid SegmentId { get; set; }
        public bool IsBusiness { get; set; }
        public bool IsRetour { get; set; }
        public int AdultPassengers { get; set; }
        public int? KidsPassengers { get; set; }
    }
}
