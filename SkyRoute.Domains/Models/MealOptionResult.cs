namespace SkyRoute.Domains.Models
{
    public class MealOptionResult
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public bool IsLocalMeal { get; set; }
    }
}