using SkyRoute.Helpers;

namespace SkyRoute.ViewModels
{
    public class OrderSummaryVM
    {
        public FlightSegmentGroupVM? OutboundFlight { get; set; }
        public FlightSegmentGroupVM? ReturnFlight { get; set; }
        public PassengerListVM? PassengerListVM { get; set; }
        public MealSelectionFormVM? MealSelection { get; set; }

        public TripClass TripClass { get; set; }
        public TripType TripType { get; set; }
    }
}