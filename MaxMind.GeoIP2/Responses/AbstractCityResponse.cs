using MaxMind.Db;
using MaxMind.GeoIP2.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     Abstract class that city-level response.
    /// </summary>
    public abstract record AbstractCityResponse : AbstractCountryResponse
    {
        /// <summary>
        ///     Gets the city for the requested IP address.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("city")]
        [MapKey("city", true)]
        public City City { get; init; } = new();

        /// <summary>
        ///     Gets the location for the requested IP address.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("location")]
        [MapKey("location", true)]
        public Location Location { get; init; } = new();

        /// <summary>
        ///     Gets the postal object for the requested IP address.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("postal")]
        [MapKey("postal", true)]
        public Postal Postal { get; init; } = new();

        /// <summary>
        ///     An <see cref="System.Collections.Generic.List{T}" /> of <see cref="Subdivision" /> objects representing
        ///     the country subdivisions for the requested IP address. The number
        ///     and type of subdivisions varies by country, but a subdivision is
        ///     typically a state, province, county, etc. Subdivisions are
        ///     ordered from most general (largest) to most specific (smallest).
        ///     If the response did not contain any subdivisions, this method
        ///     returns an empty array.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("subdivisions")]
        [MapKey("subdivisions")]
        public IReadOnlyList<Subdivision> Subdivisions { get; init; } = [];

        /// <summary>
        ///     An object representing the most specific subdivision returned. If
        ///     the response did not contain any subdivisions, this method
        ///     returns an empty <see cref="Subdivision" /> object.
        /// </summary>
        [JsonIgnore]
        public Subdivision MostSpecificSubdivision => Subdivisions.Count == 0 ? new() : Subdivisions[Subdivisions.Count - 1];

        /// <inheritdoc/>
        internal override AbstractResponse WithLocales(IReadOnlyList<string> locales)
        {
            var baseResult = (AbstractCityResponse)base.WithLocales(locales);
            return baseResult with
            {
                City = City with { Locales = locales },
                Subdivisions = [.. Subdivisions.Select(s => s with { Locales = locales })],
            };
        }
    }
}
