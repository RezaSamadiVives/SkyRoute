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
            if (model == null || model?.Passengers == null || model.Passengers.Count == 0)
            {
                ModelState.AddModelError("", "Je moet minstens één passagier toevoegen.");
            }

            int hoofdpassagiers = model != null ? model.Passengers.Count(p => !p.IsFellowPassenger) : 0;

            if (hoofdpassagiers == 0)
            {
                ModelState.AddModelError("", "Geef aan wie de hoofdpassagier is.");
            }
            else if (hoofdpassagiers > 1)
            {
                ModelState.AddModelError("", "Er mag maar één hoofdpassagier zijn.");
            }

            var shoppingCartVM = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");

            if (shoppingCartVM?.OutboundFlights?.Flights.Count == 0)
            {
                ModelState.AddModelError("", "Er zijn geen vluchtgegevens beschikbaar. Verzoek eerst een vlucht te kiezen.");
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (model != null)
            {
                foreach (var passenger in model.Passengers)
                {
                    passenger.UserId = userId;
                }

                if (shoppingCartVM != null)
                {
                    shoppingCartVM.Passengers = model.Passengers;
                    HttpContext.Session.SetObject("ShoppingCart", shoppingCartVM);
                }


            }

            return RedirectToAction("Index", "MealOption");
        }
    }
}
