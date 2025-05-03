using Microsoft.EntityFrameworkCore;
using SkyRoute.Domains.Data;
using SkyRoute.Domains.Entities;
using SkyRoute.Repositories.Interfaces;

namespace SkyRoute.Repositories.Repositories
{
    public class CityDAO(SkyRouteDbContext context) : IDAO<City>
    {
        private readonly SkyRouteDbContext _context = context;

        public async Task AddAsync(City entity)
        {
            _context.Entry(entity).State = EntityState.Added;
            try
            {

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task DeleteAsync(City entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
            try
            {

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<City?> FindByIdAsync(int Id)
        {
            try
            {
                return await _context.Cities.Where(a => a.Id == Id).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<City>?> GetAllAsync()
        {
            try
            {
                return await _context.Cities.ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAsync(City entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
