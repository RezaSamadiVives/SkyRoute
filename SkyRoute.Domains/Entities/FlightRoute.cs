namespace SkyRoute.Domains.Entities
{
    public class FlightRoute
    {
        public int Id { get; set; }
        public int FromCityId { get; set; }
        public int ToCityId { get; set; }
        public bool HasStopOver {  get; set; }
        
        public virtual City FromCity { get; set; } = null!;
        public virtual City ToCity { get; set; } = null!;

        public List<RouteStopover> Stopovers { get; set; } = [];
    }
}
