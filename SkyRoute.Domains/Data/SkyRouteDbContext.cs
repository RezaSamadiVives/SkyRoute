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
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<MealOption> MealOptions { get; set; }
        public DbSet<FlightMealOption> FlightMealOptions { get; set; }

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

            modelBuilder.Entity<FlightMealOption>()
                .HasKey(fm => new { fm.FlightId, fm.MealOptionId });

            modelBuilder.Entity<FlightMealOption>()
                .HasOne(fm => fm.Flight)
                .WithMany(f => f.MealOptions)
                .HasForeignKey(fm => fm.FlightId);

            modelBuilder.Entity<FlightMealOption>()
                .HasOne(fm => fm.MealOption)
                .WithMany(m => m.FlightMeals)
                .HasForeignKey(fm => fm.MealOptionId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
