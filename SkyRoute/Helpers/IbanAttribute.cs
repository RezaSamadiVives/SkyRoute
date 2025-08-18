using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SkyRoute.Helpers
{
    public class IbanAttribute : ValidationAttribute
    {
        public IbanAttribute()
        {
            ErrorMessage = "Voer een geldig IBAN in.";
        }

        public override bool IsValid(object? value)
        {
            if (value == null) return true; // Required moet apart gecheckt worden
            var iban = value.ToString()?.Replace(" ", "").ToUpper();

            if (string.IsNullOrWhiteSpace(iban)) return false;

            // IBAN moet minimaal 15 en maximaal 34 tekens hebben
            if (iban.Length < 15 || iban.Length > 34) return false;

            // Check met regex of het alleen letters en cijfers zijn
            if (!Regex.IsMatch(iban, "^[A-Z0-9]+$")) return false;

            // Extra: rekenkundige check (mod 97)
            return ValidateIbanChecksum(iban);
        }

        private bool ValidateIbanChecksum(string iban)
        {
            // Verplaats eerste 4 karakters naar het einde
            var rearranged = iban.Substring(4) + iban.Substring(0, 4);

            // Zet letters om naar cijfers (A=10, B=11, … Z=35)
            var numericIban = "";
            foreach (char c in rearranged)
            {
                if (char.IsLetter(c))
                    numericIban += (c - 'A' + 10).ToString();
                else
                    numericIban += c;
            }

            // Grote getallen in stukken mod97 nemen
            int chunkSize = 9;
            long total = 0;
            for (int i = 0; i < numericIban.Length; i += chunkSize)
            {
                var part = total.ToString() + numericIban.Substring(i, Math.Min(chunkSize, numericIban.Length - i));
                total = long.Parse(part) % 97;
            }

            return total == 1;
        }
    }
}
