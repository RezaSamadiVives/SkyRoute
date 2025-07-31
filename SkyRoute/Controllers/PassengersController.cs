using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyRoute.Services;
using SkyRoute.ViewModels;

namespace SkyRoute.Controllers
{
    [Authorize]
    public class PassengersController(
        IPassengerService _passengerService,
        IShoppingcartService _shoppingcartService,
        IPassengerValidator _passengerValidator) : Controller
    {

        [HttpGet]
        public IActionResult Index(int? passengersCount = null)
        {
            var shoppingCartVM = _shoppingcartService.GetShoppingCart(HttpContext.Session);

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
                _passengerService.AddPassenger(model);
                return View(model);
            }

            // Actie: Verwijder passagier
            if (action.StartsWith("remove-Passengers["))
            {
                var match = System.Text.RegularExpressions.Regex.Match(action, @"remove-Passengers\[(\d+)\]");
                if (match.Success && int.TryParse(match.Groups[1].Value, out int indexToRemove))
                {
                    _passengerService.RemovePassengerAt(model, indexToRemove);
                }

                return View(model);
            }

            var shoppingCartVM = _shoppingcartService.GetShoppingCart(HttpContext.Session);

             if (shoppingCartVM == null || shoppingCartVM.OutboundFlights?.Flights.Count == 0)
            {
                ModelState.AddModelError("", "Er zijn geen vluchtgegevens beschikbaar. Verzoek eerst een vlucht te kiezen.");
                return RedirectToAction("Index", "Home");
            }


            _passengerValidator.Validate(model, ModelState);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                ModelState.AddModelError("", "Er is geen ingelogde gebruiker gevonden");
                return RedirectToAction("Index", "Home");
            }

            if (model != null)
            {
                _passengerService.UpdatePassengerIdsAndUser(model, userId);
                _shoppingcartService.UpdatePassengerShoppingCart(model, HttpContext.Session);

            }

            return RedirectToAction("Index", "MealOption");
        }
    }
}
