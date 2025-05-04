using SkyRoute.Repositories.Interfaces;
using SkyRoute.Services.Interfaces;

namespace SkyRoute.Services.Services
{
    public class BaseService<T>(IDAO<T> dao) : IService<T> where T : class
    {
        private readonly IDAO<T> _dao = dao;

        public async Task<IEnumerable<T>?> GetAllAsync() => await _dao.GetAllAsync();
        public async Task<T?> FindByIdAsync(int id) => await _dao.FindByIdAsync(id);
        public async Task AddAsync(T entity) => await _dao.AddAsync(entity);
        public async Task UpdateAsync(T entity) => await _dao.UpdateAsync(entity);
        public async Task DeleteAsync(T entity) => await _dao.DeleteAsync(entity);
    }
}
