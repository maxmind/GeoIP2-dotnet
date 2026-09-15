using MaxMind.Db;
using MaxMind.GeoIP2.Model;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     Abstract record for country-level responses.
    /// </summary>
    public abstract record AbstractCountryResponse : AbstractResponse
    {
        private Continent _continent = new();
        private Country _country = new();
        private Model.MaxMind _maxMind = new();
        private Country _registeredCountry = new();
        private RepresentedCountry _representedCountry = new();
        private Traits _traits = new();

        /// <summary>
        ///     Gets the continent for the requested IP address.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("continent")]
        [MapKey("continent", true)]
        public Continent Continent
        {
            get => _continent;
            init => _continent = value ?? new();
        }

        /// <summary>
        ///     Gets the country for the requested IP address. This
        ///     object represents the country where MaxMind believes
        ///     the end user is located
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("country")]
        [MapKey("country", true)]
        public Country Country
        {
            get => _country;
            init => _country = value ?? new();
        }

        /// <summary>
        ///     Gets the MaxMind record containing data related to your account
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("maxmind")]
        public Model.MaxMind MaxMind
        {
            get => _maxMind;
            init => _maxMind = value ?? new();
        }

        /// <summary>
        ///     Registered country record for the requested IP address. This
        ///     record represents the country where the ISP has registered a
        ///     given IP block and may differ from the user's country.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("registered_country")]
        [MapKey("registered_country", true)]
        public Country RegisteredCountry
        {
            get => _registeredCountry;
            init => _registeredCountry = value ?? new();
        }

        /// <summary>
        ///     Represented country record for the requested IP address. The
        ///     represented country is used for things like military bases or
        ///     embassies. It is only present when the represented country
        ///     differs from the country.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("represented_country")]
        [MapKey("represented_country", true)]
        public RepresentedCountry RepresentedCountry
        {
            get => _representedCountry;
            init => _representedCountry = value ?? new();
        }

        /// <summary>
        ///     Gets the traits for the requested IP address.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("traits")]
        [MapKey("traits", true)]
        public Traits Traits
        {
            get => _traits;
            init => _traits = value ?? new();
        }

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
