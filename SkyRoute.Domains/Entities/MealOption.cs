﻿namespace SkyRoute.Domains.Entities
{
    public class MealOption
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public bool IsLocalMeal { get; set; }
        public string? ImageUrl { get; set; }
        public virtual ICollection<FlightMealOption> FlightMeals { get; set; } = new List<FlightMealOption>();
    }
}
