using Microsoft.EntityFrameworkCore;
using SkyRoute.Domains.Entities;
using System;

namespace SkyRoute.Domains.Data
{
    public static class SeedHelper
    {
        private static readonly Random _random = new();
        public static async Task SeedRoutesAsync(SkyRouteDbContext context)
        {
            if (context.FlightRoutes.Any()) return;

            var cityNames = new[] { "New York", "Londen", "Tokio", "Dubai", "Sydney", "Kaapstad", "Singapore" };

            var cities = cityNames.Select(name => new City { Name = name }).ToList();

            context.Cities.AddRange(cities);
            await context.SaveChangesAsync();

            var cityDict = cities.ToDictionary(c => c.Name, c => c.Id);

            var stopoverMap = new Dictionary<(string From, string To), List<string>>
        {
            { ("New York", "Tokio"), new List<string> { "Londen", "Dubai" } },
            { ("New York", "Sydney"), new List<string> { "Londen", "Dubai" } },
            { ("New York", "Singapore"), new List<string> { "Londen", "Dubai" } },
            { ("New York", "Kaapstad"), new List<string> { "Londen", "Dubai" } },
            { ("Londen", "Tokio"), new List<string> { "Dubai" } },
            { ("Londen", "Sydney"), new List<string> { "Dubai" } },
            { ("Londen", "Kaapstad"), new List<string> { "Dubai" } },
            { ("Tokio", "Sydney"), new List<string> { "Dubai" } },
            { ("Tokio", "Kaapstad"), new List<string> { "Dubai" } },

            // Geen overstap
            { ("New York", "Londen"), new List<string>() },
            { ("Londen", "Singapore"), new List<string>() },
            { ("Tokio", "Singapore"), new List<string>() },
            { ("Sydney", "Singapore"), new List<string>() }
        };

            var allRoutes = new List<FlightRoute>();

            foreach (var from in cityNames)
            {
                foreach (var to in cityNames)
                {
                    if (from == to) continue;

                    var forwardKey = (from, to);
                    var reverseKey = (to, from);

                    foreach (var key in new[] { forwardKey, reverseKey })
                    {
                        if (allRoutes.Any(r => r.FromCityId == cityDict[key.Item1] && r.ToCityId == cityDict[key.Item2]))
                            continue;

                        var route = new FlightRoute
                        {
                            FromCityId = cityDict[key.Item1],
                            ToCityId = cityDict[key.Item2],
                            HasStopOver = stopoverMap.TryGetValue(key, out var stops) && stops.Count > 0,
                            Stopovers = stopoverMap.TryGetValue(key, out var stopovers)
                                ? stopovers.Select((s, i) => new RouteStopover
                                {
                                    StopoverCityId = cityDict[s],
                                    Order = i
                                }).ToList()
                                : new List<RouteStopover>()
                        };

                        allRoutes.Add(route);
                    }
                }
            }

            context.FlightRoutes.AddRange(allRoutes);
            await context.SaveChangesAsync();
        }

        public static async Task SeedAirlinesAsync(SkyRouteDbContext context)
        {
            if (context.Airlines.Any()) return;

            var airlines = new List<Airline>
            {
                new() {
                    Name = "Emirates",
                    Description = "Emirates is de grootste luchtvaartmaatschappij in het Midden-Oosten en de nationale luchtvaartmaatschappij van de Verenigde Arabische Emiraten. Opgericht in 1985, opereert Emirates vanuit haar hub op Dubai International Airport en bedient meer dan 150 steden in 80 landen op zes continenten. De maatschappij staat bekend om haar moderne vloot van uitsluitend wide-body vliegtuigen, waaronder de grootste Airbus A380- en Boeing 777-vloot ter wereld. Emirates is een dochteronderneming van The Emirates Group, eigendom van de regering van Dubai.",
                    LogoUrl = "/images/airlines/emirates.png"
                },
                new() {
                    Name = "Lufthansa",
                    Description = "Lufthansa, opgericht in 1953, is de grootste luchtvaartmaatschappij van Duitsland en een van de oprichters van de Star Alliance. Met hubs in Frankfurt en München biedt Lufthansa een uitgebreid netwerk van vluchten naar 229 bestemmingen wereldwijd. De Lufthansa Group omvat meerdere luchtvaartmaatschappijen, waaronder SWISS, Austrian Airlines en Brussels Airlines, en speelt een leidende rol in de Europese luchtvaartmarkt.",
                    LogoUrl = "/images/airlines/Lufthansa.png"
                },
                new() {
                    Name = "Singapore Airlines",
                    Description = "Singapore Airlines is de nationale luchtvaartmaatschappij van Singapore en staat wereldwijd bekend om haar uitstekende service en innovatieve benadering van luchtvaart. Met een moderne vloot van voornamelijk wide-body vliegtuigen bedient de maatschappij een wereldwijd netwerk van bestemmingen. Het iconische 'Singapore Girl'-imago symboliseert de toewijding aan klanttevredenheid en Aziatische gastvrijheid.",
                    LogoUrl = "/images/airlines/singapore.png"
                }
            };

            context.Airlines.AddRange(airlines);
            await context.SaveChangesAsync();
        }

        public static async Task SeedMealOptions(SkyRouteDbContext context)
        {
            if (context.MealOptions.Any()) return;

            var meals = new List<MealOption>
            {
                new() { Name = "Standaard maaltijd", Description = "Gegrilde kip met aardappelen en groenten.",IsLocalMeal=false },
                new() { Name = "Vegetarische maaltijd", Description = "Groenterisotto.",IsLocalMeal=false },
                new() { Name = "Veganistische maaltijd", Description = "Linzencurry.",IsLocalMeal=false },
                new() { Name = "Halal maaltijd", Description = "Lam met rijst." , IsLocalMeal = false},
                new() { Name = "Kosher maaltijd", Description = "Voorgerecht, hoofdgerecht en dessert volgens joodse regels." , IsLocalMeal = false},
                new() { Name = "Glutenvrije maaltijd", Description = "Kipfilet met groenten.", IsLocalMeal = false },

                new() { Name = "New York special", Description = "Classic pastrami sandwich met zuurkool.", IsLocalMeal=true },
                new() { Name = "Londen special", Description = "Fish and chips met doperwtenpuree.", IsLocalMeal=true },
                new() { Name = "Tokio special", Description = "Sushi assortiment met misosoep." , IsLocalMeal=true},
                new() { Name = "Dubai special", Description = "Kibbeh met hummus en flatbread." , IsLocalMeal=true},
                new() { Name = "Sydney special", Description = "Barramundi met citroenboter en salade.", IsLocalMeal=true },
                new() { Name = "Kaapstad special", Description = "Bobotie met gele rijst en chutney." , IsLocalMeal=true},
                new() { Name = "Singapore special", Description = "Hainanese chicken rice met bouillon." , IsLocalMeal=true}
            };

            await context.MealOptions.AddRangeAsync(meals);
            await context.SaveChangesAsync();
        }

        //public static async Task SeedFlights(SkyRouteDbContext context)
        //{
        //    if (context.Flights.Any()) return;

        //    var startDate = DateTime.Today.AddDays(3);
        //    var endDate = DateTime.Today.AddMonths(7);

        //    var airlines = context.Airlines.ToList();
        //    var routes = context.FlightRoutes
        //        .Include(r => r.FromCity)
        //        .Include(r => r.ToCity)
        //        .Include(r => r.Stopovers)
        //        .ThenInclude(s => s.StopoverCity)
        //        .ToList();

        //    var allMeals = context.MealOptions.ToList();

        //    foreach (var route in routes)
        //    {
        //        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        //        {
        //            var shuffledAirlines = airlines.OrderBy(_ => _random.Next()).ToList();

        //            for (int i = 0; i < 3; i++)
        //            {
        //                var airline = shuffledAirlines[i];
        //                var departureTime = TimeSpan.FromHours(6 + i * 4);
        //                var departureDateTime = date.Date + departureTime;
        //                var currentTime = departureDateTime;

        //                var stopsOrdered = route.Stopovers.OrderBy(s => s.Order).ToList();
        //                var cities = new List<string> { route.FromCity.Name };
        //                cities.AddRange(stopsOrdered.Select(s => s.StopoverCity.Name));
        //                cities.Add(route.ToCity.Name);

        //                // Seizoensgebonden prijsverhoging
        //                decimal priceMultiplier = 1m;
        //                if ((route.FromCity.Name == "Londen" || route.ToCity.Name == "Londen" ||
        //                     route.FromCity.Name == "New York" || route.ToCity.Name == "New York") && date.Month == 11)
        //                {
        //                    priceMultiplier = 1.3m;
        //                }
        //                else if ((route.FromCity.Name == "Tokio" || route.ToCity.Name == "Tokio" ||
        //                          route.FromCity.Name == "Singapore" || route.ToCity.Name == "Singapore" ||
        //                          route.FromCity.Name == "Dubai" || route.ToCity.Name == "Dubai") &&
        //                         (date.Month == 7 || date.Month == 8))
        //                {
        //                    priceMultiplier = 1.3m;
        //                }

        //                for (int j = 0; j < cities.Count - 1; j++)
        //                {
        //                    var from = cities[j];
        //                    var to = cities[j + 1];
        //                    var flightDuration = TimeSpan.FromHours(_random.Next(6, 10));
        //                    var arrivalTime = currentTime + flightDuration;

        //                    var flight = new Flight
        //                    {
        //                        FromCity = from,
        //                        ToCity = to,
        //                        Airline = airline.Name,
        //                        FlightDate = date,
        //                        DepartureTime = currentTime.TimeOfDay,
        //                        ArrivalTime = arrivalTime,
        //                        PriceEconomy = GetRandomPrice(200, 800) * priceMultiplier,
        //                        PriceBusiness = GetRandomPrice(800, 1600) * priceMultiplier,
        //                        Seats = new List<Seat>(),
        //                        MealOptions = new List<FlightMealOption>()
        //                    };

        //                    var seats = GenerateSeats(flight);
        //                    foreach (var seat in seats)
        //                    {
        //                        seat.Flight = flight;
        //                        flight.Seats.Add(seat);
        //                    } 

        //                    context.Flights.Add(flight);
        //                    await context.SaveChangesAsync();



        //                    foreach (var meal in allMeals)
        //                    {
        //                        if (!meal.IsLocalMeal)
        //                        {
        //                            context.FlightMealOptions.Add(new FlightMealOption
        //                            {
        //                                Flight = flight,
        //                                MealOption = meal,
        //                            });
        //                        }
        //                    }

        //                    var localMeal = allMeals.FirstOrDefault(x =>
        //                        x.Name.ToLower().Contains(to.ToLower()) && x.IsLocalMeal);
        //                    if (localMeal != null)
        //                    {
        //                        context.FlightMealOptions.Add(new FlightMealOption
        //                        {
        //                            Flight = flight,
        //                            MealOption = localMeal,
        //                        });
        //                    }

        //                    await context.SaveChangesAsync();

        //                    // Overstaptijd van 2 uur (behalve laatste segment)
        //                    if (j < cities.Count - 2)
        //                    {
        //                        currentTime = arrivalTime + TimeSpan.FromHours(2);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}


        public static async Task SeedFlights(SkyRouteDbContext context)
        {
            if (context.Flights.Any()) return;

            var startDate = DateTime.Today.AddDays(3);
            var endDate = DateTime.Today.AddMonths(3);

            var airlines = await context.Airlines.ToListAsync();
            var routes = await context.FlightRoutes
                .Include(r => r.FromCity)
                .Include(r => r.ToCity)
                .Include(r => r.Stopovers)
                .ThenInclude(s => s.StopoverCity)
                .ToListAsync();

            var allMeals = await context.MealOptions.ToListAsync();


            foreach (var route in routes)
            {
                var cities = BuildCityPath(route);

                for (var date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    var shuffledAirlines = airlines.OrderBy(_ => _random.Next()).ToList();

                    for (int i = 0; i < airlines.Count; i++)
                    {
                        var airline = shuffledAirlines[i];
                        var departureTime = TimeSpan.FromHours(6 + i * 4);
                        var currentTime = date.Date + departureTime;

                        decimal priceMultiplier = GetSeasonalPriceMultiplier(route, date);
                        Guid segmentId = Guid.NewGuid();
                        var flightsToAdd = new List<Flight>();

                        for (int j = 0; j < cities.Count - 1; j++)
                        {
                            var from = cities[j];
                            var to = cities[j + 1];
                            var flightDuration = TimeSpan.FromHours(_random.Next(6, 10));
                            var arrivalDateTime = currentTime + flightDuration;

                            var flight = new Flight
                            {
                                FlightNumber = $"{from.Name[..2].ToUpper()}{to.Name[..2].ToUpper()}{(j * 2):D3}",
                                FromCityId = from.Id,
                                ToCityId = to.Id,
                                AirlineId = airline.Id,
                                FlightRouteId = route.Id,
                                SegmentId = segmentId,
                                FlightDate = currentTime.Date,
                                DepartureTime = currentTime.TimeOfDay,
                                ArrivalDate = arrivalDateTime.Date,
                                ArrivalTime = arrivalDateTime.TimeOfDay,
                                PriceEconomy = GetRandomPrice(600, 800) * priceMultiplier,
                                PriceBusiness = GetRandomPrice(800, 1000) * priceMultiplier,
                                Seats = [],
                                MealOptions = []
                            };

                            var seats = GenerateSeats(flight);
                            flight.Seats = seats;

                            var meals = GetFlightMealOptions(allMeals, to.Name, flight);
                            flight.MealOptions = meals;

                            flightsToAdd.Add(flight);
                            //context.Flights.Add(flight);
                            //await context.SaveChangesAsync();

                            if (j < cities.Count - 2)
                            {
                                currentTime = arrivalDateTime + TimeSpan.FromHours(2);// transfer time
                            }
                            else
                            {
                                currentTime = arrivalDateTime;
                            }
                        }
                        await context.Flights.AddRangeAsync(flightsToAdd);
                        await context.SaveChangesAsync();
                    }
                }
            }
        }

        private static List<City> BuildCityPath(FlightRoute route)
        {
            var cities = new List<City> { route.FromCity };
            cities.AddRange(route.Stopovers.OrderBy(s => s.Order).Select(s => s.StopoverCity));
            cities.Add(route.ToCity);
            return cities;
        }

        private static decimal GetSeasonalPriceMultiplier(FlightRoute route, DateTime date)
        {
            var highSeasonCities = new[] { "Londen", "New York", "Tokio", "Singapore", "Dubai" };

            if ((highSeasonCities.Contains(route.FromCity.Name) || highSeasonCities.Contains(route.ToCity.Name)) &&
                ((route.FromCity.Name is "Londen" or "New York") && date.Month == 11 ||
                 (route.FromCity.Name is "Tokio" or "Singapore" or "Dubai") && (date.Month == 7 || date.Month == 8)))
            {
                return 1.3m;
            }

            return 1m;
        }


        private static List<FlightMealOption> GetFlightMealOptions(List<MealOption> allMeals, string toCity, Flight flight)
        {
            var meals = allMeals
                .Where(m => !m.IsLocalMeal)
                .Select(m => new FlightMealOption { MealOption = m, Flight = flight })
                .ToList();

            var local = allMeals
                .FirstOrDefault(x => x.IsLocalMeal && x.Name.Contains(toCity, StringComparison.OrdinalIgnoreCase));

            if (local != null)
            {
                meals.Add(new FlightMealOption { MealOption = local, Flight = flight });
            }

            return meals;
        }

        private static List<Seat> GenerateSeats(Flight flight)
        {


            var totalRows = 48;

            // 3-3-3 opstelling: A B C - D E F - G H J
            var seatLetters = new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J' };
            var businessRows = (int)(totalRows * 0.3);

            var seats = new List<Seat>();

            for (int row = 1; row <= totalRows; row++)
            {
                bool isBusiness = row <= businessRows;

                foreach (var letter in seatLetters)
                {
                    var seatNumber = $"{row}{letter}";

                    seats.Add(new Seat
                    {
                        SeatNumber = seatNumber,
                        IsBusiness = isBusiness,
                        IsAvailable = true,
                        Flight = flight,
                    });
                }
            }

            return seats;
        }

        private static decimal GetRandomPrice(int min, int max)
        {
            return Math.Round((decimal)(_random.NextDouble() * (max - min) + min), 2);
        }
    }

}
