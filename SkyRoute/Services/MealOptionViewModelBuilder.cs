using AutoMapper;
using SkyRoute.Helpers;
using SkyRoute.Services.Interfaces;
using SkyRoute.ViewModels;

namespace SkyRoute.Services
{
    public class MealOptionViewModelBuilder(
        IFlightSearchService _flightSearchService,
        IMealOptionService _mealOptionService,
        IShoppingcartService _shoppingcartService,
        IMapper _mapper
    ) : IMealOptionViewModelBuilder
    {
        public async Task<MealSelectionFormVM> BuildMealSelectionFormAsync(HttpContext context)
        {
            var shoppingCart = _shoppingcartService.GetShoppingCart(context.Session);
            var vm = new MealSelectionFormVM();

            if (shoppingCart?.OutboundFlights?.Flights?.Count > 0)
            {
                var outbound = await GeneratePassengerMealChoices(shoppingCart,shoppingCart.Passengers, shoppingCart.OutboundFlights.Flights, TripType.Enkel);
                vm.PassengerMeals.AddRange(outbound);
            }

            if (shoppingCart?.RetourFlights?.Flights?.Count > 0)
            {
                var retour = await GeneratePassengerMealChoices(shoppingCart,shoppingCart.Passengers, shoppingCart.RetourFlights.Flights, TripType.Retour);
                vm.PassengerMeals.AddRange(retour);
            }

            foreach (var passenger in vm.PassengerMeals)
                passenger.FlightMeals = [.. passenger.FlightMeals.OrderBy(f => f.TripType)];

            return vm;
        }


        private async Task<List<PassengerMealChoiceVM>> GeneratePassengerMealChoices(ShoppingCartVM shoppingCartVM,
            List<PassengerVM> passengers,
            List<int> flightIds,
            TripType tripType)
        {
            var list = new List<PassengerMealChoiceVM>();
            var existingChoices = shoppingCartVM.MealChoicePassengerSessions;

            foreach (var passenger in passengers)
            {
                var pm = new PassengerMealChoiceVM { Passenger = passenger };

                foreach (var flightId in flightIds)
                {
                    var flight = await _flightSearchService.FindFlightWithDetailsAsync(flightId);
                    var meals = await _mealOptionService.GetMealOptionListAsync(flightId);
                    var existingChoice = existingChoices?.FirstOrDefault(f => f.FlightId == flightId);

                    pm.FlightMeals.Add(new FlightMealSelectionVM
                    {
                        Flight = _mapper.Map<FlightVM>(flight),
                        AvailableMeals = _mapper.Map<List<MealOptionVM>>(meals.MealOptionsList),
                        TripType = tripType,
                        SelectedMealOptionId = existingChoice == null ? 0 : existingChoice.MealOptionId,
                    });
                }

                list.Add(pm);
            }

            return list;
        }
    }
}