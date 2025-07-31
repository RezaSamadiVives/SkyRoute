using SkyRoute.Services.Interfaces;
using SkyRoute.ViewModels;

namespace SkyRoute.Services
{
    public class MealOptionSelectionService(
        IFlightSearchService _flightSearchService,
        IMealOptionService _mealOptionService,
        IShoppingcartService _shoppingcartService) : IMealOptionSelectionService
    {
        public async Task<(bool Success, string Message)> SaveMealOptionAsync(MealSelectionPassengerVM selection, HttpContext context)
        {
            if (!IsValidSelection(selection))
                return await Task.FromResult((false, "Ongeldige keuze"));

            var shoppingCartVM = _shoppingcartService.GetShoppingCart(context.Session);
            if (shoppingCartVM == null || shoppingCartVM.OutboundFlights?.Flights.Count == 0 || shoppingCartVM.Passengers.Count == 0)
                return await Task.FromResult((false, "Er zijn geen vlucht - en passagier informatie beschikbaar."));

            var passenger = shoppingCartVM.Passengers.FirstOrDefault(p => p.Id == selection.TempPassengerId);
            if (passenger == null)
                return await Task.FromResult((false, "Geen passagier gevonden met deze informatie."));

            var flight = await _flightSearchService.FindByIdAsync(selection.FlightId);
            if (flight == null)
                return (false, "Er is geen vlucht met deze gegevens gevonden.");

            var mealOptions = await _mealOptionService.GetMealOptionListAsync(flight.Id);
            var selectedMeal = mealOptions?.MealOptionsList.FirstOrDefault(m => m.Id == selection.MealOptionId);
            if (selectedMeal == null)
                return (false, "Er is geen maaltijd met deze informatie gevonden.");

            var existing = shoppingCartVM.MealChoicePassengerSessions
                .FirstOrDefault(x => x.PassengerId == selection.TempPassengerId && x.FlightId == selection.FlightId);

            if (existing != null)
                existing.MealOptionId = selectedMeal.Id;
            else
                shoppingCartVM.MealChoicePassengerSessions.Add(new MealChoicePassengerSession
                {
                    PassengerId = selection.TempPassengerId,
                    FlightId = selection.FlightId,
                    MealOptionId = selectedMeal.Id
                });

            _shoppingcartService.SetShoppingObject(shoppingCartVM, context.Session);
            return await Task.FromResult((true, $"Maaltijdkeuze is opgeslagen voor {passenger.FirstName} {passenger.LastName}."));
        }

        private static bool IsValidSelection(MealSelectionPassengerVM selection) =>
        selection != null && selection.TempPassengerId > 0 && selection.FlightId > 0 && selection.MealOptionId > 0;
    }
}