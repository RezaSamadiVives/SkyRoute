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
        private List<(FlightVM Flight, List<MealOptionVM> Meals)>? _flightMealsCache;
        public async Task<MealSelectionFormVM> BuildMealSelectionFormAsync(HttpContext context)
        {
            var shoppingCart = _shoppingcartService.GetShoppingCart(context.Session);
            await EnsureFlightMealsLoadedAsync(shoppingCart);
            var vm = new MealSelectionFormVM();

            if (shoppingCart?.OutboundFlights?.Flights?.Count > 0)
            {
                var outbound = GeneratePassengerMealChoices(shoppingCart, TripType.Enkel);
                vm.PassengerMeals.AddRange(outbound);
            }

            if (shoppingCart?.RetourFlights?.Flights?.Count > 0)
            {
                var retour = GeneratePassengerMealChoices(shoppingCart, TripType.Retour);
                vm.PassengerMeals.AddRange(retour);
            }

            foreach (var passenger in vm.PassengerMeals)
                passenger.FlightMeals = [.. passenger.FlightMeals.OrderBy(f => f.TripType)];

            return vm;
        }

        public async Task<List<(FlightVM, List<(PassengerVM, MealOptionVM)>)>> GetSelectedMealsAsync(HttpContext context)
        {
            var shoppingCart = _shoppingcartService.GetShoppingCart(context.Session);
            await EnsureFlightMealsLoadedAsync(shoppingCart);
            var vm = new List<(FlightVM, List<(PassengerVM, MealOptionVM)>)>();
            if (shoppingCart.Passengers?.Count <= 0 || shoppingCart.OutboundFlights?.Flights.Count <= 0 ||
            shoppingCart.MealChoicePassengerSessions.Count <= 0 || _flightMealsCache == null)
            {
                return vm;
            }
            if (shoppingCart.Passengers != null)
            {
                foreach (var flight in _flightMealsCache)
                {
                    List<(PassengerVM, MealOptionVM)> passengerMeal = [];

                    var passengerChoicesForThisFlight = shoppingCart.MealChoicePassengerSessions
                    .Where(x => x.FlightId == flight.Flight.Id);

                    foreach (var passengermeal in passengerChoicesForThisFlight)
                    {
                        var passenger = shoppingCart.Passengers.First(p => p.Id == passengermeal.PassengerId);
                        var meal = _flightMealsCache.First(f => f.Flight.Id == passengermeal.FlightId)
                            .Meals.First(m => m.Id == passengermeal.MealOptionId);

                        passengerMeal.Add((passenger,meal));
                    }
                    
                    vm.Add((flight.Flight, passengerMeal));
                }
            }

            return vm;
        }

        private List<PassengerMealChoiceVM> GeneratePassengerMealChoices(ShoppingCartVM shoppingCartVM,
            TripType tripType)
        {
            var list = new List<PassengerMealChoiceVM>();
            var existingChoices = shoppingCartVM.MealChoicePassengerSessions;
            var passengers = shoppingCartVM.Passengers;
            var flightIds = tripType == TripType.Enkel ? shoppingCartVM?.OutboundFlights?.Flights : shoppingCartVM?.RetourFlights?.Flights;

            if (passengers == null || flightIds == null || _flightMealsCache == null)
            {
                return list;
            }

            foreach (var passenger in passengers)
            {
                var pm = new PassengerMealChoiceVM { Passenger = passenger };

                foreach (var flightId in flightIds)
                {
                    (FlightVM, List<MealOptionVM>) flightMeals = _flightMealsCache.FirstOrDefault(x => x.Item1.Id == flightId);
                    var existingChoice = existingChoices?.FirstOrDefault(f => f.FlightId == flightId && f.PassengerId == passenger.Id);

                    pm.FlightMeals.Add(new FlightMealSelectionVM
                    {
                        Flight = flightMeals.Item1,
                        AvailableMeals = flightMeals.Item2,
                        TripType = tripType,
                        SelectedMealOptionId = existingChoice == null ? 0 : existingChoice.MealOptionId,
                    });
                }

                list.Add(pm);
            }

            return list;
        }


        private async Task EnsureFlightMealsLoadedAsync(ShoppingCartVM shoppingCart)
        {
            if (_flightMealsCache != null)
                return; // al geladen

            _flightMealsCache = [];

            var flightIds = (shoppingCart?.OutboundFlights?.Flights ?? [])
                .Concat(shoppingCart?.RetourFlights?.Flights ?? [])
                .ToList();

            foreach (var flightId in flightIds)
            {
                var flight = await _flightSearchService.FindFlightWithDetailsAsync(flightId);
                var meals = await _mealOptionService.GetMealOptionListAsync(flightId);

                if (flight != null)
                    _flightMealsCache.Add((
                        _mapper.Map<FlightVM>(flight),
                        _mapper.Map<List<MealOptionVM>>(meals.MealOptionsList)
                        )
                        );
            }
        }


    }
}