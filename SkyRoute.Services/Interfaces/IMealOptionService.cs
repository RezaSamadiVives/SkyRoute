using SkyRoute.Domains.Entities;
using SkyRoute.Domains.Models;

namespace SkyRoute.Services.Interfaces
{
    public interface IMealOptionService : IService<MealOption>
    {
        Task<MealOptionList> GetMealOptionListAsync(int flightId);
    }
}