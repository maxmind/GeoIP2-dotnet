using System.Collections.Generic;
using MaxMind.GeoIP2.Model;

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     This class provides a model for the data returned by GeoIP2 Precision: City and GeoIP2 City.
    ///     The only difference between the City and Insights response classes is
    ///     which fields in each record may be populated.
    ///     <a href="http://dev.maxmind.com/geoip/geoip2/web-services">
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
        public CityResponse(
            City city = null,
            Continent continent = null,
            Country country = null,
            Location location = null,
            Model.MaxMind maxMind = null,
            Postal postal = null,
            Country registeredCountry = null,
            RepresentedCountry representedCountry = null,
            List<Subdivision> subdivisions = null,
            Traits traits = null)
            : base(
                city, continent, country, location, maxMind, postal, registeredCountry, representedCountry, subdivisions,
                traits)
        {
        }
    }
}