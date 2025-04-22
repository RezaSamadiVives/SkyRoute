namespace SkyRoute.Domains.Entities
{
    public class RouteStopover
    {
        public int Id { get; set; }
        public int RouteId { get; set; }

        public int StopoverCityId { get; set; }

        public int Order { get; set; }

        public FlightRoute Route { get; set; } = null!;
        public City StopoverCity { get; set; } = null!;
    }
}
