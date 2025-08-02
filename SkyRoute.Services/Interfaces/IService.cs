using SkyRoute.Domains.Entities;
using SkyRoute.Domains.Models;

namespace SkyRoute.Services.Interfaces
{
    public interface IService<T> where T : class
    {
        Task<IEnumerable<T>?> GetAllAsync();
        Task AddAsync(T entity);
        Task DeleteAsync(T entity);

        Task UpdateAsync(T entity);
        Task<T?> FindByIdAsync(int Id);
    }
}
