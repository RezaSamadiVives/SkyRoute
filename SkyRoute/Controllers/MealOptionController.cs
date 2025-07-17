using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyRoute.Domains.Entities;
using SkyRoute.Extensions;
using SkyRoute.Services.Interfaces;
using SkyRoute.ViewModels;

namespace SkyRoute.Controllers
{
    [Authorize]
    public class MealOptionController(IMealOptionService mealOptionService, IService<Flight> flightService, IMapper mapper) : Controller
    {
        private readonly IMealOptionService _mealOptionService = mealOptionService;
        private readonly IService<Flight> _flightService = flightService;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var shoppingCartVM = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");

            if (shoppingCartVM == null || shoppingCartVM.OutboundFlights?.Flights.Count == 0)
            {
                ModelState.AddModelError("", "Er zijn geen vluchtgegevens beschikbaar. Verzoek eerst een vlucht te kiezen.");
                return RedirectToAction("Index", "Home");
            }

            var viewModel = new FlightMealSelectListVM();

            if (shoppingCartVM.OutboundFlights?.Flights != null)
            {
                foreach (var flightId in shoppingCartVM.OutboundFlights.Flights)
                {
                    var flight = await _flightService.FindByIdAsync(flightId);

                    var flightMealSelect = new FlightMealSelectVM
                    {
                        Flight = _mapper.Map<FlightVM>(flight)
                    };

                    viewModel.OutboundFlightMealSelectLists.Add(flightMealSelect);
                }

            }

            return View(viewModel);
        }

    }
}