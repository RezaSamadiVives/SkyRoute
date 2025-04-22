using Microsoft.EntityFrameworkCore;
using SkyRoute.Domains.Entities;

namespace SkyRoute.Domains.Data
{
    public class SkyRouteDbContext : DbContext
    {
        public SkyRouteDbContext(DbContextOptions<SkyRouteDbContext> options) : base(options)
        {
        }

        
        public DbSet<City> Cities { get; set; }
        public DbSet<FlightRoute> FlightRoutes { get; set; }
        public DbSet<RouteStopover> RouteStops { get; set; }
        public DbSet<Airline> Airlines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // FlightRoute -> City (From)
            modelBuilder.Entity<FlightRoute>()
                .HasOne(fr => fr.FromCity)
                .WithMany()
                .HasForeignKey(fr => fr.FromCityId)
                .OnDelete(DeleteBehavior.Restrict);

            // FlightRoute -> City (To)
            modelBuilder.Entity<FlightRoute>()
                .HasOne(fr => fr.ToCity)
                .WithMany()
                .HasForeignKey(fr => fr.ToCityId)
                .OnDelete(DeleteBehavior.Restrict);

            // RouteStopover -> FlightRoute
            modelBuilder.Entity<RouteStopover>()
                .HasOne(rs => rs.Route)
                .WithMany(fr => fr.Stopovers)
                .HasForeignKey(rs => rs.RouteId)
                .OnDelete(DeleteBehavior.Cascade);

            // RouteStopover -> City (StopoverCity)
            modelBuilder.Entity<RouteStopover>()
                .HasOne(rs => rs.StopoverCity)
                .WithMany()
                .HasForeignKey(rs => rs.StopoverCityId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
