using Microsoft.AspNetCore.Mvc;

namespace SkyRoute.Controllers
{
    public class PassengersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
