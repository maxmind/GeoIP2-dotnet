#region

using MaxMind.GeoIP2.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     Abstract class that city-level response.
    /// </summary>
    public abstract class AbstractCityResponse : AbstractCountryResponse
    {
        [JsonProperty("subdivisions")]
        private readonly IList<Subdivision> _subdivisions;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AbstractCityResponse" /> class.
        /// </summary>
        protected AbstractCityResponse()
        {
            City = new City();
            Location = new Location();
            Postal = new Postal();
            _subdivisions = new List<Subdivision>();
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
            IEnumerable<Subdivision>? subdivisions = null,
            Traits? traits = null)
            : base(continent, country, maxMind, registeredCountry, representedCountry, traits)
        {
            City = city ?? new City();
            Location = location ?? new Location();
            Postal = postal ?? new Postal();
            _subdivisions = subdivisions != null ? new List<Subdivision>(subdivisions) : new List<Subdivision>();
        }

        /// <summary>
        ///     Gets the city for the requested IP address.
        /// </summary>
        [JsonProperty("city")]
        public City City { get; internal set; }

        /// <summary>
        ///     Gets the location for the requested IP address.
        /// </summary>
        [JsonProperty("location")]
        public Location Location { get; internal set; }

        /// <summary>
        ///     Gets the postal object for the requested IP address.
        /// </summary>
        [JsonProperty("postal")]
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
        [JsonIgnore]
        public List<Subdivision> Subdivisions => new List<Subdivision>(_subdivisions);

        /// <summary>
        ///     An object representing the most specific subdivision returned. If
        ///     the response did not contain any subdivisions, this method
        ///     returns an empty <see cref="Subdivision" /> object.
        /// </summary>
        [JsonIgnore]
        public Subdivision MostSpecificSubdivision
        {
            get
            {
                if (Subdivisions == null || Subdivisions.Count == 0)
                    return new Subdivision();

                return Subdivisions.Last();
            }
        }

        /// <summary>
        ///     Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return GetType().Name + " ["
                   + (City != null ? "City=" + City + ", " : "")
                   + (Location != null ? "Location=" + Location + ", " : "")
                   + (Postal != null ? "Postal=" + Postal + ", " : "")
                   +
                   (Subdivisions != null
                       ? "Subdivisions={" + string.Join(",", Subdivisions.Select(s => s.ToString()).ToArray()) + "}, "
                       : "")
                   + (Continent != null ? "Continent=" + Continent + ", " : "")
                   + (Country != null ? "Country=" + Country + ", " : "")
                   + (RegisteredCountry != null ? "RegisteredCountry=" + RegisteredCountry + ", " : "")
                   + (RepresentedCountry != null ? "RepresentedCountry=" + RepresentedCountry + ", " : "")
                   + (Traits != null ? "Traits=" + Traits : "")
                   + "]";
        }

        /// <summary>
        ///     Sets the locales on all the NamedEntity properties.
        /// </summary>
        /// <param name="locales">The locales specified by the user.</param>
        protected internal override void SetLocales(IEnumerable<string> locales)
        {
            locales = locales.ToList();
            base.SetLocales(locales);

            if (City != null)
                City.Locales = locales;

            if (Subdivisions == null || Subdivisions.Count <= 0) return;
            foreach (var subdivision in Subdivisions)
                subdivision.Locales = locales;
        }
    }
}
