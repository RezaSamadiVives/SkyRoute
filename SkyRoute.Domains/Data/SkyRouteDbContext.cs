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
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

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

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Passenger)
                .WithMany(p => p.Tickets)
                .HasForeignKey(t => t.PassengerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Booking)
                .WithMany(b => b.Tickets)
                .HasForeignKey(t => t.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Seat)
                .WithMany()
                .HasForeignKey(t => t.SeatId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.MealOption)
                .WithMany()
                .HasForeignKey(t => t.MealOptionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Flight)
                .WithMany(f => f.Tickets)
                .HasForeignKey(t => t.FlightId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUser>(b =>
              {
                  b.ToTable("AspNetUsers");
              });


            base.OnModelCreating(modelBuilder);
        }
    }
}
