using Microsoft.AspNetCore.Mvc;
using SkyRoute.Domains.Entities;
using SkyRoute.Models;
using SkyRoute.Services.Interfaces;
using SkyRoute.ViewModels;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SkyRoute.Controllers
{
    public class HomeController(IService<City> _cityService) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(FlightSearchFormVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var toCity = await _cityService.FindByIdAsync(model.DestinationCity);
            var fromCity = await _cityService.FindByIdAsync(model.DepartureCity);

            return RedirectToAction("FlightSearchResults", "FlightSearch", new
            {
                fromCityId = model.DepartureCity,
                fromCity = fromCity?.Name,
                toCityId = model.DestinationCity,
                toCity = toCity?.Name,
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
