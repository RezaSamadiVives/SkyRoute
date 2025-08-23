using SkyRoute.Domains.Entities;
using SkyRoute.Domains.Models;
using SkyRoute.Repositories.Interfaces;
using SkyRoute.Services.Interfaces;

namespace SkyRoute.Services.Services
{
    public class MealOptionService(IMealOptionDAO _mealOptionDAO) : BaseService<MealOption>(_mealOptionDAO), IMealOptionService
    {
        public async Task<MealOptionList> GetMealOptionListAsync(int flightId)
        {
            return await _mealOptionDAO.GetMealOptionListAsync(flightId);
        }
    }
}