namespace SkyRoute.ViewModels
{
    public class AirlineVM
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
    }
}
