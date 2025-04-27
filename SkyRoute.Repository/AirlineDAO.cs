using Microsoft.EntityFrameworkCore;
using SkyRoute.Domains.Data;
using SkyRoute.Domains.Entities;
using SkyRoute.Repositories.Interfaces;

namespace SkyRoute.Repositories
{
    public class AirlineDAO(SkyRouteDbContext context) : IDAO<Airline>
    {
        private readonly SkyRouteDbContext _context = context;

        public async Task AddAsync(Airline entity)
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

        public async Task DeleteAsync(Airline entity)
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

        public async Task<Airline?> FindByIdAsync(int Id)
        {
            try
            {
                return await _context.Airlines.Where(a => a.Id == Id).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Airline>?> GetAllAsync()
        {
            try
            {
                return await _context.Airlines.ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAsync(Airline entity)
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
