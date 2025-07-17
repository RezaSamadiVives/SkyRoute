namespace SkyRoute.ViewModels
{
    public class FlightMealSelectVM
    {
        public FlightVM? Flight { get; set; }
        public List<PassengerVM> Passengers { get; set; } = [];
    }
}