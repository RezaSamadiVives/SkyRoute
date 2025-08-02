using SkyRoute.Domains.Entities;
using SkyRoute.Repositories.Interfaces;
using SkyRoute.Services.Interfaces;

namespace SkyRoute.Services.Services
{
    public class CityService(IDAO<City> cityDAO) : IService<City>
    {
        private readonly IDAO<City> _cityDAO = cityDAO;

        public async Task AddAsync(City entity)
        {
            await _cityDAO.AddAsync(entity);
        }

        public async Task DeleteAsync(City entity)
        {
            await _cityDAO.DeleteAsync(entity);
        }

        public async Task<City?> FindByIdAsync(int Id)
        {
            return await _cityDAO.FindByIdAsync(Id);
        }

        public async Task<IEnumerable<City>?> GetAllAsync()
        {
            return await _cityDAO.GetAllAsync();
        }

        public async Task UpdateAsync(City entity)
        {
            await _cityDAO.UpdateAsync(entity);
        }
    }
}
