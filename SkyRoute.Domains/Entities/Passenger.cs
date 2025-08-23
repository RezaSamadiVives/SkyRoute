using Microsoft.AspNetCore.Identity;

namespace SkyRoute.Domains.Entities
{
    public class Passenger
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public string? MiddelName { get; set; }
        public required string LastName { get; set; }
        public DateTime Birthday { get; set; }

        // True = hoofdpassagier (de ingelogde user), False = meereizende
        public bool IsFellowPassenger { get; set; }

        public required string UserId { get; set; }
        public virtual IdentityUser User { get; set; } = null!;

        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}