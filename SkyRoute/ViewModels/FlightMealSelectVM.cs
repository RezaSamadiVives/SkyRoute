namespace SkyRoute.ViewModels
{
    public class FlightMealSelectVM
    {
        public FlightVM? Flight { get; set; }
        public List<MealOptionVM> MealOption{ get; set; } = [];
        
    }
}