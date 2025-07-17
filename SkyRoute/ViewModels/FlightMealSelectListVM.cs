namespace SkyRoute.ViewModels
{
    public class FlightMealSelectListVM
    {
        public List<FlightMealSelectVM> OutboundFlightMealSelectLists { get; set; } = [];
        public List<FlightMealSelectVM> RetourFlightMealSelectLists { get; set; } = [];
    }
}