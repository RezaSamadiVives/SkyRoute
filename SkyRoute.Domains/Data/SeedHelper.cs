using SkyRoute.Domains.Entities;

namespace SkyRoute.Domains.Data
{
    public static class SeedHelper
    {
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
    }

}
