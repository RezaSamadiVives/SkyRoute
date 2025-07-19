namespace SkyRoute.ViewModels
{
    public class PassengerMealChoiceVM
    {
        public required PassengerVM Passenger { get; set; }
        public List<FlightMealSelectionVM> FlightMeals { get; set; } = [];
    }

}