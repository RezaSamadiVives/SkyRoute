using Microsoft.AspNetCore.Mvc;
using SkyRoute.Models;
using SkyRoute.ViewModels;
using System.Diagnostics;

namespace SkyRoute.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(FlightSearchFormVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("FlightSearchResults", "FlightSearch", new
            {
                fromCityId = model.DepartureCity,
                toCityId = model.DestinationCity,
                departureDate = model.DepartureDate,
                returnDate = model.ReturnDate,
                isRetour = model.IsRetour,
                isBusiness = model.IsBusiness,
                adultPassengers = model.AdultPassengers,
                kidsPassengers = model.KidsPassengers
            });
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
