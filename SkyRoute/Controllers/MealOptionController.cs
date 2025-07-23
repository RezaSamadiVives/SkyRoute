using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyRoute.Domains.Models;
using SkyRoute.Extensions;
using SkyRoute.Helpers;
using SkyRoute.Services.Interfaces;
using SkyRoute.ViewModels;

namespace SkyRoute.Controllers
{
    [Authorize]
    public class MealOptionController(
        IFlightSearchService flightSearchService,
        IMealOptionService mealOptionService,
        IMapper mapper) : Controller
    {

        private readonly IFlightSearchService _flightSearchService = flightSearchService;
        private readonly IMealOptionService _mealOptionService = mealOptionService;
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
                var outboundMeals = await GeneratePassengerMealChoices(shoppingCartVM.Passengers, shoppingCartVM.OutboundFlights.Flights, TripType.Enkel);
                mealSelectionForm.PassengerMeals.AddRange(outboundMeals);
            }

            if (shoppingCartVM.RetourFlights?.Flights.Count > 0)
            {
                var retourMeals = await GeneratePassengerMealChoices(shoppingCartVM.Passengers, shoppingCartVM.RetourFlights.Flights, TripType.Retour);
                mealSelectionForm.PassengerMeals.AddRange(retourMeals);
            }

            return View(mealSelectionForm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveMealOption([FromBody] MealSelectionPassengerVM selection)
        {
            if (!IsValidSelection(selection))
            {
                return Json(new { success = false, message = "Ongeldige keuze" });
            }

            var shoppingCartVM = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            if (shoppingCartVM == null
                            || shoppingCartVM.OutboundFlights?.Flights.Count == 0
                            || shoppingCartVM.Passengers.Count == 0)
            {
                return Json(new { success = false, message = "Er zijn geen vlucht - en passagier informatie beschikbaar." });
            }

            var passengers = shoppingCartVM.Passengers;
            var selectedPassenger = passengers.FirstOrDefault(f => f.Id == selection.TempPassengerId);
            if (selectedPassenger == null)
            {
                return Json(new { success = false, message = "Geen passagier gevonden met deze informatie." });
            }

            var flight = await _flightSearchService.FindByIdAsync(selection.FlightId);
            if (flight == null)
            {
                return Json(new { success = false, message = "Er is geen vlucht met deze gegevens gevonden." });
            }

            var mealOptions = await GetMealOptionList(flight.Id);
            var mealOption = mealOptions?.MealOptionsList.FirstOrDefault(m => m.Id == selection.MealOptionId);
            if (mealOption == null)
            {
                return Json(new { success = false, message = "Er is geen maaltijd met deze informatie gevonden." });
            }

            bool alreadySelected = shoppingCartVM.MealChoicePassengerSessions.Any(x =>
                                x.PassengerId == selection.TempPassengerId &&
                                x.FlightId == selection.FlightId);

            if (alreadySelected)
            {
                var mealChoicePassengerSession = shoppingCartVM.MealChoicePassengerSessions.First(x =>
                                x.PassengerId == selection.TempPassengerId &&
                                x.FlightId == selection.FlightId);
                mealChoicePassengerSession.MealOptionId = mealOption.Id;

            }
            else
            {
                shoppingCartVM.MealChoicePassengerSessions.Add(
                    new MealChoicePassengerSession
                    {
                        PassengerId = selection.TempPassengerId,
                        FlightId = flight.Id,
                        MealOptionId = mealOption.Id
                    });
            }

            HttpContext.Session.SetObject("ShoppingCart", shoppingCartVM);
            return Json(new { success = true, message = $"Maaltijdkeuze is opgeslagen voor {selectedPassenger.FirstName} {selectedPassenger.LastName}." });
        }

        private static bool IsValidSelection(MealSelectionPassengerVM selection)
        {
            return selection != null
                && selection.TempPassengerId > 0
                && selection.FlightId > 0
                && selection.MealOptionId > 0;
        }

        private async Task<List<PassengerMealChoiceVM>> GeneratePassengerMealChoices(List<PassengerVM> passengers, List<int> flightIds, TripType tripType)
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
                        AvailableMeals = _mapper.Map<List<MealOptionVM>>(mealOptions.MealOptionsList.ToList())
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