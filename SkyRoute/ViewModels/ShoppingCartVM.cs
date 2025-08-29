namespace SkyRoute.ViewModels
{
    public class ShoppingCartVM
    {
        public FlightSegmentSessionVM? OutboundFlights { get; set; } = new FlightSegmentSessionVM();
        public FlightSegmentSessionVM? RetourFlights { get; set; } = new FlightSegmentSessionVM();
        public List<PassengerVM> Passengers { get; set; } = [];
        public List<MealChoicePassengerSession> MealChoicePassengerSessions { get; set; } = [];
        public FlightSearchSessionVM? FlightSearchSessionVM { get; set; }
        public bool IsConfirmed { get; set; }
        public PaymentVM? PaymentDetail { get; set; }
        public decimal Total
        {
            get
            {
                decimal price = 0;

                if (OutboundFlights?.Flights?.Count > 0)
                    price += Passengers.Count > 0 ? OutboundFlights.TotalPrice * Passengers.Count : OutboundFlights.TotalPrice;

                if (RetourFlights?.Flights?.Count > 0)
                    price += Passengers.Count > 0 ? RetourFlights.TotalPrice * Passengers.Count : RetourFlights.TotalPrice ;

                return price;
            }
        }

    }
}
