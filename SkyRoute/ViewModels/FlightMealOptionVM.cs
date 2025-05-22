using SkyRoute.Domains.Entities;

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
