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
    public class MealOptionController(
        IFlightSearchService flightSearchService,
        IMealOptionService mealOptionService,
        IService<Flight> flightService,
        IMapper mapper) : Controller
    {

        private readonly IFlightSearchService _flightSearchService = flightSearchService;
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

            var mealSelectionForm = new MealSelectionFormVM();

            if (shoppingCartVM.OutboundFlights?.Flights.Count > 0)
            {
                foreach (var passenger in shoppingCartVM.Passengers)
                {
                    var passengerVM = new PassengerMealChoiceVM
                    {
                        Passenger = passenger
                    };

                    foreach (var flightId in shoppingCartVM.OutboundFlights.Flights)
                    {
                        var mealOptions = await _mealOptionService.GetMealOptionListAsync(flightId);

                        passengerVM.FlightMeals.Add(new FlightMealSelectionVM
                        {
                            Flight = _mapper.Map<FlightVM>(await _flightSearchService.FindFlightWithDetailsAsync(flightId)),
                            AvailableMeals = _mapper.Map<List<MealOptionVM>>(mealOptions.MealOptionsList.ToList())
                        });

                    }
                    mealSelectionForm.PassengerMeals.Add(passengerVM);
                }
            }



            return View(mealSelectionForm);
        }

    }
}