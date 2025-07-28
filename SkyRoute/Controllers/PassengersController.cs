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
        public IActionResult Index(int? passengersCount = null)
        {
            var shoppingCartVM = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");

            var model = new PassengerListVM();

            // Als er al passagiers in sessie zijn: laad die
            if (shoppingCartVM?.Passengers != null && shoppingCartVM.Passengers.Count > 0)
            {
                model.Passengers = shoppingCartVM.Passengers;
            }
            // Anders: begin met leeg formulier gebaseerd op meegegeven count
            else if (passengersCount.HasValue && passengersCount.Value > 0)
            {
                for (int i = 0; i < passengersCount.Value; i++)
                {
                    model.Passengers.Add(new PassengerVM());
                }
            }
            else
            {
                ModelState.AddModelError("", "Er is geen passagiersinformatie beschikbaar.");
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(PassengerListVM model)
        {
            var action = Request.Form["action"].ToString();

            // Actie: Voeg passagier toe
            if (action == "add")
            {
                model.Passengers.Add(new PassengerVM());
                return View(model);
            }

            // Actie: Verwijder passagier
            if (action.StartsWith("remove-Passengers["))
            {
                var match = System.Text.RegularExpressions.Regex.Match(action, @"remove-Passengers\[(\d+)\]");
                if (match.Success && int.TryParse(match.Groups[1].Value, out int indexToRemove))
                {
                    if (indexToRemove >= 0 && indexToRemove < model.Passengers.Count)
                    {
                        model.Passengers.RemoveAt(indexToRemove);
                    }
                }

                return View(model);
            }

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

            if (shoppingCartVM == null || shoppingCartVM.OutboundFlights?.Flights.Count == 0)
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
                for (int i = 0; i < model.Passengers.Count; i++)
                {
                    model.Passengers[i].UserId = userId;
                    model.Passengers[i].Id = i + 1;
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
