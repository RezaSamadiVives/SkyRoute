namespace SkyRoute.Domains.Models
{
    public class PassengerFlightMeals
    {
        public required string FirstName { get; set; }
        public string? MiddelName { get; set; }
        public required string LastName { get; set; }
        public DateTime Birthday { get; set; }

        // True = hoofdpassagier (de ingelogde user), False = meereizende
        public bool IsFellowPassenger { get; set; }

        public List<FlightMealChoice> MealChoics { get; set; } = [];
    }
}