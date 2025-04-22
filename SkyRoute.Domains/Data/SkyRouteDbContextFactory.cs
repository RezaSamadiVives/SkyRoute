using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SkyRoute.Domains.Data;

namespace SkyRoute.Domains
{
    public class SkyRouteDbContextFactory : IDesignTimeDbContextFactory<SkyRouteDbContext>
    {
        public SkyRouteDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SkyRouteDbContext>();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new SkyRouteDbContext(optionsBuilder.Options);
        }
    }
}
