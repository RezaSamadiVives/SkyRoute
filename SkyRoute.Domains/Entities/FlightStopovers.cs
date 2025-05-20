namespace SkyRoute.Domains.Entities
{
    public class FlightStopovers
    {
        public static readonly Dictionary<(string From, string To), string[]> Stopovers = new()
        {
            { ("New York", "Tokio"), new[] { "Londen", "Dubai" } },
            { ("New York", "Sydney"), new[] { "Londen", "Dubai" } },
            { ("New York", "Singapore"), new[] { "Londen", "Dubai" } },
            { ("New York", "Kaapstad"), new[] { "Londen", "Dubai" } },
            { ("Londen", "Tokio"), new[] { "Dubai" } },
            { ("Londen", "Sydney"), new[] { "Dubai" } },
            { ("Londen", "Kaapstad"), new[] { "Dubai" } },
            { ("Tokio", "Sydney"), new[] { "Dubai" } },
            { ("Tokio", "Kaapstad"), new[] { "Dubai" } },
            { ("New York", "Londen"), Array.Empty<string>() },
            { ("Londen", "Singapore"), Array.Empty<string>() },
            { ("Tokio", "Singapore"), Array.Empty<string>() },
            { ("Sydney", "Singapore"), Array.Empty<string>() },
            { ("Sydney", "Kaapstad"), new[] { "Dubai" } },
        };

    }
}
