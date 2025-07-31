using SkyRoute.ViewModels;

namespace SkyRoute.Services
{
    public interface IMealOptionSelectionService
    {
        Task<(bool Success, string Message)> SaveMealOptionAsync(MealSelectionPassengerVM selection, HttpContext context);
    }
}