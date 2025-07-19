namespace SkyRoute.ViewModels
{
    public class FlightMealSelectionVM
    {
        public required FlightVM Flight { get; set; }
        public int SelectedMealOptionId { get; set; }
        public List<MealOptionVM> AvailableMeals { get; set; } = [];
    }
}