namespace SkyRoute.ViewModels
{
    public class FlightRouteVM
    {
        public int Id { get; set; }
        public int FromCityId { get; set; }
        public int ToCityId { get; set; }
        public bool HasStopOver { get; set; }

        public virtual CityVM FromCity { get; set; } = null!;
        public virtual CityVM ToCity { get; set; } = null!;

        public List<RouteStopoverVM> Stopovers { get; set; } = [];
    }
}
