using Microsoft.AspNetCore.Mvc.Rendering;

namespace SkyRoute.ViewModels
{
    public class FlightMealOptionVM
    {
        public int FlightId { get; set; }
        public required FlightVM Flight { get; set; }

        public int MealOptionId { get; set; }
        public required MealOptionVM MealOption { get; set; }

    }
}
