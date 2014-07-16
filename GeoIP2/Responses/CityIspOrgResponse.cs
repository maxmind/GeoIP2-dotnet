using System.Collections.Generic;
using MaxMind.GeoIP2.Model;
using System;

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    /// This class provides a model for the data returned by the GeoIP2 Precision: City
    /// end point.
    /// 
    /// The only difference between the City and Insights response classes is
    /// which fields in each record may be populated.
    /// 
    /// <a href="http://dev.maxmind.com/geoip/geoip2/web-services">GeoIP2 Web
    ///      Services</a>
    /// </summary>
    [Obsolete("CityIspOrgResponse is deprecated. Please use CityResponse instead.")]
    public class CityIspOrgResponse : AbstractCityResponse
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CityIspOrgResponse() { }

        /// <summary>
        /// Constructor
        /// </summary>
        public CityIspOrgResponse(
            City city = null,
            Continent continent = null,
            Country country = null, Location location = null,
            Model.MaxMind maxMind = null,
            Postal postal = null,
            Country registeredCountry = null,
            RepresentedCountry representedCountry = null,
            List<Subdivision> subdivisions = null,
            Traits traits = null)
            : base(
                city, continent, country, location, maxMind, postal, registeredCountry, representedCountry, subdivisions, traits)
        {
        }
    }
}