#region

using MaxMind.GeoIP2.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

#endregion

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     Abstract class that city-level response.
    /// </summary>
    public abstract class AbstractCityResponse : AbstractCountryResponse
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AbstractCityResponse" /> class.
        /// </summary>
        protected AbstractCityResponse()
        {
            City = new();
            Location = new();
            Postal = new();
            Subdivisions = [];
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AbstractCityResponse" /> class.
        /// </summary>
        protected AbstractCityResponse(
            City? city = null,
            Continent? continent = null,
            Country? country = null,
            Location? location = null,
            Model.MaxMind? maxMind = null,
            Postal? postal = null,
            Country? registeredCountry = null,
            RepresentedCountry? representedCountry = null,
            IReadOnlyList<Subdivision>? subdivisions = null,
            Traits? traits = null)
            : base(continent, country, maxMind, registeredCountry, representedCountry, traits)
        {
            City = city ?? new();
            Location = location ?? new();
            Postal = postal ?? new();
            Subdivisions = subdivisions ?? [];
        }

        /// <summary>
        ///     Gets the city for the requested IP address.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("city")]
        public City City { get; internal set; }

        /// <summary>
        ///     Gets the location for the requested IP address.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("location")]
        public Location Location { get; internal set; }

        /// <summary>
        ///     Gets the postal object for the requested IP address.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("postal")]
        public Postal Postal { get; internal set; }

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
        public IReadOnlyList<Subdivision> Subdivisions { get; internal set; }

        /// <summary>
        ///     An object representing the most specific subdivision returned. If
        ///     the response did not contain any subdivisions, this method
        ///     returns an empty <see cref="Subdivision" /> object.
        /// </summary>
        [JsonIgnore]
        public Subdivision MostSpecificSubdivision => Subdivisions.Count == 0 ? new() : Subdivisions[Subdivisions.Count - 1];

        /// <summary>
        ///     Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return GetType().Name + " ["
                  + "City=" + City + ", "
                  + "Location=" + Location + ", "
                  + "Postal=" + Postal + ", "
                  + "Subdivisions={" +
                  string.Join(",", Subdivisions.Select(s => s.ToString()).ToArray()) + "}, "
                  + "Continent=" + Continent + ", "
                  + "Country=" + Country + ", "
                  + "RegisteredCountry=" + RegisteredCountry + ", "
                  + "RepresentedCountry=" + RepresentedCountry + ", "
                  + "Traits=" + Traits
                  + "]";
        }

        /// <summary>
        ///     Sets the locales on all the NamedEntity properties.
        /// </summary>
        /// <param name="locales">The locales specified by the user.</param>
        protected internal override void SetLocales(IReadOnlyList<string> locales)
        {
            locales = [.. locales];
            base.SetLocales(locales);
            City.Locales = locales;

            if (Subdivisions.Count == 0)
            {
                return;
            }

            foreach (var subdivision in Subdivisions)
            {
                subdivision.Locales = locales;
            }
        }
    }
}
