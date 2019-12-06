#region

using MaxMind.Db;
using MaxMind.GeoIP2.Model;
using System.Collections.Generic;

#endregion

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     This class provides a model for the data returned by the GeoIP2 Precision:
    ///     Insights end point.
    ///     The only difference between the City and Insights response classes is
    ///     which fields in each record may be populated.
    ///     <a href="https://dev.maxmind.com/geoip/geoip2/web-services">
    ///         GeoIP2 Web
    ///         Services
    ///     </a>
    /// </summary>
    public class EnterpriseResponse : AbstractCityResponse
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        [Constructor]
        public EnterpriseResponse(
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
            : base(
                city, continent, country, location, maxMind, postal, registeredCountry, representedCountry, subdivisions,
                traits)
        {
        }
    }
}