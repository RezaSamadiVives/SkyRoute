using Microsoft.AspNetCore.Mvc.Rendering;
using SkyRoute.Helpers;
using System.ComponentModel.DataAnnotations;

namespace SkyRoute.ViewModels
{
    public class FlightSearchFormVM : IValidatableObject
    {

        public List<SelectListItem>? Cities { get; set; }

        [Required(ErrorMessage = "Kies enkel of retour")]
        public TripType SelectedTripType { get; set; }

        [Required(ErrorMessage = "Kies economy of business")]
        public TripClass SelectedTripClass { get; set; }

        [Required(ErrorMessage = "Kies jouw vertrekdatum")]
        public DateTime DepartureDate { get; set; } = DateTime.Today.AddDays(3);

        public DateTime? ReturnDate { get; set; }

        [Required(ErrorMessage = "Vertrekstad is verplicht")]
        public int DepartureCity { get; set; }

        [Required(ErrorMessage = "Bestemming is verplicht")]
        public int DestinationCity { get; set; }

        [Required(ErrorMessage = "Reiziger is verplicht")]
        public int AdultPassengers { get; set; }

        public int? KidsPassengers { get; set; }

        public bool IsBusiness => SelectedTripClass == TripClass.Business;
        public bool IsRetour => SelectedTripType == TripType.Retour;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SelectedTripType == TripType.Retour && ReturnDate == null)
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
