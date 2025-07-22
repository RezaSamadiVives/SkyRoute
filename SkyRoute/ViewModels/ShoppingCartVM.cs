namespace SkyRoute.ViewModels
{
    public class ShoppingCartVM
    {
        public FlightSegmentSessionVM? OutboundFlights { get; set; } = new FlightSegmentSessionVM();
        public FlightSegmentSessionVM? RetourFlights { get; set; } = new FlightSegmentSessionVM();
        public List<PassengerVM> Passengers { get; set; } = [];
        public List<MealChoicePassengerSession> MealChoicePassengerSessions { get; set; } = [];

        public decimal Total
        {
            get
            {
                decimal price = 0;

                if (OutboundFlights?.Flights?.Count > 0)
                    price += OutboundFlights.TotalPrice;

                if (RetourFlights?.Flights?.Count > 0)
                    price += RetourFlights.TotalPrice;

                return price;
            }
        }

    }
}
