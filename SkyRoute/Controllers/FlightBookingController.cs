using Microsoft.AspNetCore.Mvc;

namespace SkyRoute.Controllers
{
    public class FlightBookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
