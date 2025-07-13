using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyRoute.Extensions;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(PassengerListVM model)
        {

            if (model.Passengers == null || model.Passengers.Count == 0)
            {
                ModelState.AddModelError("", "Je moet minstens één passagier toevoegen.");
            }

            int hoofdpassagiers = model.Passengers.Count(p => p.IsFellowPassenger);

            if (hoofdpassagiers == 0)
            {
                ModelState.AddModelError("", "Geef aan wie de hoofdpassagier is.");
            }
            else if (hoofdpassagiers > 1)
            {
                ModelState.AddModelError("", "Er mag maar één hoofdpassagier zijn.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var shoppingCartVM = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart") ?? new ShoppingCartVM();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            foreach (var passenger in model.Passengers)
            {
                passenger.UserId = userId;
            }

            shoppingCartVM.Passengers = model.Passengers;
            HttpContext.Session.SetObject("ShoppingCart", shoppingCartVM);

            return RedirectToAction("Index","MealOption");
        }
    }
}
