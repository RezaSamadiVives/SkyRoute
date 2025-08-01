using SkyRoute.Domains.Entities;
using SkyRoute.Domains.Models;

namespace SkyRoute.Repositories.Interfaces
{
    public interface IMealOptionDAO : IDAO<MealOption>
    {
        Task<MealOptionList> GetMealOptionListAsync(int flightId);

    }
}