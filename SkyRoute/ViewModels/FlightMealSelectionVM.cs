using System.ComponentModel.DataAnnotations;

namespace SkyRoute.ViewModels
{
    public class FlightMealSelectionVM
    {
        public required FlightVM Flight { get; set; }
        
        [Range(1, int.MaxValue,ErrorMessage = "Selecteer een maaltijd voor deze vlucht.")]
        public int SelectedMealOptionId { get; set; }
        public List<MealOptionVM> AvailableMeals { get; set; } = [];
    }
}