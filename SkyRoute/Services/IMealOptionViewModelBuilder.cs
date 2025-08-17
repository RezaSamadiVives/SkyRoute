using SkyRoute.ViewModels;

namespace SkyRoute.Services
{
    public interface IMealOptionViewModelBuilder
    {
        Task<MealSelectionFormVM> BuildMealSelectionFormAsync(HttpContext context);
        Task<List<(FlightVM, List<(PassengerVM, MealOptionVM)>)>> GetSelectedMealsAsync(HttpContext context);
    }
}