using Microsoft.AspNetCore.Mvc;

namespace SkyRoute.Controllers
{
    public class FlightSearchController : Controller
    {
        public IActionResult SearchFlights(string departureCity, string destinationCity, 
            DateTime departureDate, DateTime? returnDate, string tripType, string tripClass, int adultPassengers, int? kidsPassengers)
        {
            return View();
        }
    }
}
