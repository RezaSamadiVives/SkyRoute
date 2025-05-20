namespace SkyRoute.Domains.Entities
{
    public class FlightMealOption
    {
        public int FlightId { get; set; }
        public virtual required Flight Flight { get; set; }

        public int MealOptionId { get; set; }
        public virtual MealOption MealOption { get; set; }  = null!;

    }
}
