using System.ComponentModel.DataAnnotations;
using SkyRoute.Helpers;

namespace SkyRoute.ViewModels
{
    public class PaymentVM
    {
        [Required(ErrorMessage = "IBAN is verplicht")]
        [Iban]
        public string IBAN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Naam is verplicht")]
        public string Rekeninghouder { get; set; } = string.Empty;

        [Required(ErrorMessage = "Naam van de bank is verplicht")]
        public string Bank { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bedrag is verplicht")]
        [Range(0.01, 1000000, ErrorMessage = "Bedrag moet positief zijn")]
        public decimal Amount { get; set; }

    }
}