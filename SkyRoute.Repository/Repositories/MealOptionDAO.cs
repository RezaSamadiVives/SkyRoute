using Microsoft.EntityFrameworkCore;
using SkyRoute.Domains.Data;
using SkyRoute.Domains.Entities;
using SkyRoute.Domains.Models;
using SkyRoute.Repositories.Interfaces;

namespace SkyRoute.Repositories.Repositories
{
    public class MealOptionDAO(SkyRouteDbContext context) : BaseDAO<MealOption>(context), IMealOptionDAO
    {

        public async Task<MealOptionList> GetMealOptionListAsync(int flightId)
        {
            var result = new MealOptionList();
            var mealOptionsIds = await _context.FlightMealOptions
            .Where(f => f.FlightId == flightId)
            .Select(m => m.MealOptionId)
            .ToListAsync();

            if (mealOptionsIds.Count == 0)
            {
                return result;
            }

            var mealOptions = await _context.MealOptions.Where(f => mealOptionsIds.Contains(f.Id)).ToListAsync();
            if (mealOptions.Count == 0)
            {
                return result;
            }

            result.MealOptionsList.AddRange(mealOptions);
            return result;
        }

    }
}