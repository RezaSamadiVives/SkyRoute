using System.ComponentModel.DataAnnotations;

namespace SkyRoute.ViewModels
{
    public class PassengerVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Voornaam")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Tussennaam")]
        public string? MiddelName { get; set; }

        [Required]
        [Display(Name = "Achternaam")]
        public string LastName { get; set; } = null!;

        [Display(Name = "Geboortedatum")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Display(Name = "Is Meereizende?")]
        public bool IsFellowPassenger { get; set; }
        public string? UserId { get; set; }

    }
}
