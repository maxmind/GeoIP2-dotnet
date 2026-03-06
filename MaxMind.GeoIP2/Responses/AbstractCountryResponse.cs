using MaxMind.Db;
using MaxMind.GeoIP2.Model;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     Abstract class for country-level response.
    /// </summary>
    public abstract record AbstractCountryResponse : AbstractResponse
    {
        /// <summary>
        ///     Gets the continent for the requested IP address.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("continent")]
        [MapKey("continent", true)]
        public Continent Continent { get; init; } = new();

        /// <summary>
        ///     Gets the country for the requested IP address. This
        ///     object represents the country where MaxMind believes
        ///     the end user is located
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("country")]
        [MapKey("country", true)]
        public Country Country { get; init; } = new();

        /// <summary>
        ///     Gets the MaxMind record containing data related to your account
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("maxmind")]
        public Model.MaxMind MaxMind { get; init; } = new();

        /// <summary>
        ///     Registered country record for the requested IP address. This
        ///     record represents the country where the ISP has registered a
        ///     given IP block and may differ from the user's country.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("registered_country")]
        [MapKey("registered_country", true)]
        public Country RegisteredCountry { get; init; } = new();

        /// <summary>
        ///     Represented country record for the requested IP address. The
        ///     represented country is used for things like military bases or
        ///     embassies. It is only present when the represented country
        ///     differs from the country.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("represented_country")]
        [MapKey("represented_country", true)]
        public RepresentedCountry RepresentedCountry { get; init; } = new();

        /// <summary>
        ///     Gets the traits for the requested IP address.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("traits")]
        [MapKey("traits", true)]
        public Traits Traits { get; init; } = new();

        /// <inheritdoc/>
        internal override AbstractResponse WithLocales(IReadOnlyList<string> locales)
        {
            return this with
            {
                Continent = Continent with { Locales = locales },
                Country = Country with { Locales = locales },
                RegisteredCountry = RegisteredCountry with { Locales = locales },
                RepresentedCountry = RepresentedCountry with { Locales = locales },
            };
        }
    }
}
