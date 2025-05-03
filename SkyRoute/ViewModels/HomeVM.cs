using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SkyRoute.ViewModels
{
    public class HomeVM : IValidatableObject
    {
        public List<SelectListItem>? Cities { get; set; }

        [Required(ErrorMessage = "Kies enkel of retour")]
        public string SelectedTripType { get; set; }

        [Required(ErrorMessage = "Kies economy of business")]
        public string SelectedTripClass { get; set; }

        [Required(ErrorMessage = "Kies jouw vertrekdatum")]
        public DateTime DepartureDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        [Required(ErrorMessage = "Vertrekstad is verplicht")]
        public string DepartureCity { get; set; }

        [Required(ErrorMessage = "Bestemming is verplicht")]
        public string DestinationCity { get; set; }

        [Required(ErrorMessage = "Reiziger is verplicht")]
        public int AdultPassengers { get; set; }

        public int? KidsPassengers { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SelectedTripType == "retour" && ReturnDate == null)
            {
                yield return new ValidationResult("Retourdatum is verplicht bij een retourreis.", new[] { nameof(ReturnDate) });
            }

            if (ReturnDate != null && ReturnDate < DepartureDate)
            {
                yield return new ValidationResult("Retourdatum moet na vertrekdatum zijn.", new[] { nameof(ReturnDate) });
            }
        }
    }

}
