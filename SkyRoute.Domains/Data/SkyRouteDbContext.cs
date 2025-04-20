using Microsoft.EntityFrameworkCore;
using SkyRoute.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyRoute.Domains.Data
{
    public class SkyRouteDbContext : DbContext
    {
        public SkyRouteDbContext(DbContextOptions<SkyRouteDbContext> options) : base(options)
        {
        }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<City> Cities { get; set; }
    }
}
