﻿namespace SkyRoute.ViewModels
{
    public class MealOptionVM
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public bool IsLocalMeal { get; set; }
        public string? ImageUrl { get; set; } 
    }
}