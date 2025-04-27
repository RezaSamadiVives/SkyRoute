using SkyRoute.Domains.Entities;
using SkyRoute.Repositories.Interfaces;
using SkyRoute.Services.Interfaces;

namespace SkyRoute.Services.Services
{
    public class AirlineService(IDAO<Airline> airlineDAO) : IService<Airline>
    {
        private readonly IDAO<Airline> _airlineDAO = airlineDAO;

        public async Task AddAsync(Airline entity)
        {
            await _airlineDAO.AddAsync(entity);
        }

        public async Task DeleteAsync(Airline entity)
        {
            await _airlineDAO.DeleteAsync(entity);
        }

        public async Task<Airline?> FindByIdAsync(int Id)
        {
            return await _airlineDAO.FindByIdAsync(Id);
        }

        public async Task<IEnumerable<Airline>?> GetAllAsync()
        {
            return await _airlineDAO.GetAllAsync();
        }

        public async Task UpdateAsync(Airline entity)
        {
            await _airlineDAO.UpdateAsync(entity);
        }
    }
}
