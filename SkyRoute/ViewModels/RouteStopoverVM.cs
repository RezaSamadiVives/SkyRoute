namespace SkyRoute.ViewModels
{
    public class RouteStopoverVM
    {
        public int Id { get; set; }
        public int RouteId { get; set; }

        public int StopoverCityId { get; set; }

        public int Order { get; set; }

        public required FlightRouteVM Route { get; set; }
        public required CityVM StopoverCity { get; set; }
    }
}