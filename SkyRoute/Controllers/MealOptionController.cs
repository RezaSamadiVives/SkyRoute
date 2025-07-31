using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyRoute.Domains.Models;
using SkyRoute.Extensions;
using SkyRoute.Helpers;
using SkyRoute.Services;
using SkyRoute.Services.Interfaces;
using SkyRoute.ViewModels;

namespace SkyRoute.Controllers
{
    [Authorize]
    public class MealOptionController(
        IFlightSearchService _flightSearchService,
        IMealOptionService _mealOptionService,
        IShoppingcartService _shoppingcartService,
        IMealOptionViewModelBuilder _viewModelBuilder,
        IMealOptionSelectionService _mealoptionSelectionService,
        IMapper _mapper) : Controller
    {


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var shoppingCartVM = _shoppingcartService.GetShoppingCart(HttpContext.Session);

            if (shoppingCartVM == null || shoppingCartVM.OutboundFlights?.Flights.Count == 0)
            {
                ModelState.AddModelError("", "Er zijn geen vluchtgegevens beschikbaar. Verzoek eerst een vlucht te kiezen.");
                return RedirectToAction("Index", "Home");
            }

            var vm = await _viewModelBuilder.BuildMealSelectionFormAsync(HttpContext);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveMealOption([FromBody] MealSelectionPassengerVM selection)
        {
          var (success, message) = await _mealoptionSelectionService.SaveMealOptionAsync(selection, HttpContext);
        return Json(new { success, message });
        }

        private static bool IsValidSelection(MealSelectionPassengerVM selection)
        {
            return selection != null
                && selection.TempPassengerId > 0
                && selection.FlightId > 0
                && selection.MealOptionId > 0;
        }

        private async Task<List<PassengerMealChoiceVM>> GeneratePassengerMealChoices(
            List<PassengerVM> passengers,
            List<int> flightIds,
            TripType tripType)
        {
            var passengerMeals = new List<PassengerMealChoiceVM>();

            for (int i = 0; i < passengers.Count; i++)
            {
                var passenger = passengers[i];

                var passengerVM = new PassengerMealChoiceVM
                {
                    Passenger = passenger
                };

                foreach (var flightId in flightIds)
                {
                    var mealOptions = await GetMealOptionList(flightId);

                    passengerVM.FlightMeals.Add(new FlightMealSelectionVM
                    {
                        Flight = _mapper.Map<FlightVM>(await _flightSearchService.FindFlightWithDetailsAsync(flightId)),
                        AvailableMeals = _mapper.Map<List<MealOptionVM>>(mealOptions.MealOptionsList.ToList()),
                        TripType = tripType
                    });
                }

                passengerMeals.Add(passengerVM);
            }

            return passengerMeals;
        }


        private async Task<MealOptionList> GetMealOptionList(int flightId)
        {
            return await _mealOptionService.GetMealOptionListAsync(flightId); ;
        }

    }
}