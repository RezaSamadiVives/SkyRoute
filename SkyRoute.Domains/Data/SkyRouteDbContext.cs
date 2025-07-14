using Microsoft.EntityFrameworkCore;
using SkyRoute.Domains.Entities;

namespace SkyRoute.Domains.Data
{
    public class SkyRouteDbContext(DbContextOptions<SkyRouteDbContext> options) : DbContext(options)
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<FlightRoute> FlightRoutes { get; set; }
        public DbSet<RouteStopover> RouteStops { get; set; }
        public DbSet<Airline> Airlines { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<MealOption> MealOptions { get; set; }
        public DbSet<FlightMealOption> FlightMealOptions { get; set; }
        public DbSet<Passenger> Passengers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Flight -> FromCity
            modelBuilder.Entity<Flight>()
                .HasOne(f => f.FromCity)
                .WithMany()
                .HasForeignKey(f => f.FromCityId)
                .OnDelete(DeleteBehavior.Restrict);

            // Flight -> ToCity
            modelBuilder.Entity<Flight>()
                .HasOne(f => f.ToCity)
                .WithMany()
                .HasForeignKey(f => f.ToCityId)
                .OnDelete(DeleteBehavior.Restrict);

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
                
            modelBuilder.Entity<Passenger>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            base.OnModelCreating(modelBuilder);
        }
    }
}
