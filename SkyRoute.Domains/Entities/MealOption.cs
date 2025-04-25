namespace SkyRoute.Domains.Entities
{
    public class MealOption
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public bool IsLocalMeal { get; set; }
        public ICollection<FlightMealOption> FlightMeals { get; set; }
    }
}
