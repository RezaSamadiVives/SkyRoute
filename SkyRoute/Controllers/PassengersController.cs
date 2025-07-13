using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyRoute.ViewModels;

namespace SkyRoute.Controllers
{
    [Authorize]
    public class PassengersController : Controller
    {
        [HttpGet]
        public IActionResult Index(int passengersCount)
        {
            var model = new PassengerListVM();

            for (int i = 0; i < passengersCount; i++)
            {
                model.Passengers.Add(new PassengerVM());
            }

            return View(model);
        }
    }
}
