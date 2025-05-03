using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SkyRoute.Domains.Entities;
using SkyRoute.Models;
using SkyRoute.Services.Interfaces;
using SkyRoute.ViewModels;
using System.Diagnostics;

namespace SkyRoute.Controllers
{
    public class HomeController(IService<City> cityService) : Controller
    {
        private readonly IService<City> cityService = cityService;

        public async Task<IActionResult> Index()
        {
            var model = new HomeVM();
            await PopulateCities(model);
            model.DepartureDate = DateTime.Now.AddDays(3);
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(HomeVM model)
        {
            await PopulateCities(model);

            if (!ModelState.IsValid) { 
            return View(model);
            }

            return RedirectToAction("SearchFlights", "FlightSearch", new
            {
                departureCity = model.DepartureCity,
                destinationCity = model.DestinationCity,
                departureDate = model.DepartureDate,
                returnDate = model.ReturnDate,
                tripType = model.SelectedTripType,
                tripClass = model.SelectedTripClass,
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

        private async Task PopulateCities(HomeVM model)
        {
            var cities = await cityService.GetAllAsync();
            model.Cities =
            [
                .. from city in cities
                                      select new SelectListItem { Value = city.Id.ToString(), Text = city.Name },
            ];
        }

    }
}
