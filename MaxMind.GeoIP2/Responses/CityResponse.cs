#region

using MaxMind.Db;
using MaxMind.GeoIP2.Model;
using System.Collections.Generic;

#endregion

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     This class provides a model for the data returned by GeoIP2 Precision: City and GeoIP2 City.
    ///     The only difference between the City and Insights response classes is
    ///     which fields in each record may be populated.
    ///     <a href="https://dev.maxmind.com/geoip/geoip2/web-services">
    ///         GeoIP2 Web
    ///         Services
    ///     </a>
    /// </summary>
    public class CityResponse : AbstractCityResponse
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public CityResponse()
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        [Constructor]
        public CityResponse(
            City? city = null,
            Continent? continent = null,
            Country? country = null,
            Location? location = null,
            [Parameter("maxmind")] Model.MaxMind? maxMind = null,
            Postal? postal = null,
            [Parameter("registered_country")] Country? registeredCountry = null,
            [Parameter("represented_country")] RepresentedCountry? representedCountry = null,
            IEnumerable<Subdivision>? subdivisions = null,
            [Parameter("traits", true)] Traits? traits = null)
            : base(
                city, continent, country, location, maxMind, postal, registeredCountry, representedCountry, subdivisions,
                traits)
        {
        }
    }
}