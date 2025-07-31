using SkyRoute.ViewModels;

namespace SkyRoute.Services
{
    public interface IMealOptionViewModelBuilder
    {
        Task<MealSelectionFormVM> BuildMealSelectionFormAsync(HttpContext context);
    }
}