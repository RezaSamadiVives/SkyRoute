namespace SkyRoute.Repositories.Interfaces
{
    public interface IDAO<T> where T: class
    {
        Task<IEnumerable<T>?> GetAllAsync();
        Task AddAsync(T entity);
        Task DeleteAsync(T entity);

        Task UpdateAsync(T entity);
        Task<T?> FindByIdAsync(int id);
    }
}
