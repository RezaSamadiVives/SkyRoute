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

        [Required(ErrorMessage = "Geboortedatum is verplicht.")]
        [DataType(DataType.Date)]
        [Display(Name = "Geboortedatum")]
        [CustomValidation(typeof(PassengerVM), nameof(ValidateBirthday))]
        public DateTime Birthday { get; set; }

        [Display(Name = "Is Meereizende?")]
        public bool IsFellowPassenger { get; set; }
        public string? UserId { get; set; }

        public static ValidationResult? ValidateBirthday(DateTime? birthday, ValidationContext context)
        {
            if (!birthday.HasValue)
                return new ValidationResult("Geboortedatum is verplicht.");

            if (birthday.Value > DateTime.Today)
                return new ValidationResult("Geboortedatum kan niet in de toekomst liggen.");

            return ValidationResult.Success;
        }

    }
}
