using MaxMind.GeoIP2.Model;

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     This class provides a model for the data returned by the GeoIP2 Precision: Country and GeoIP2 Country.
    ///     The only difference between the City and Insights response classes is
    ///     which fields in each record may be populated.
    ///     See
    ///     <a href="http://dev.maxmind.com/geoip/geoip2/web-services">
    ///         GeoIP2 Web
    ///         Services
    ///     </a>
    /// </summary>
    public class CountryResponse : AbstractCountryResponse
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public CountryResponse()
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public CountryResponse(Continent continent = null, Country country = null, Model.MaxMind maxMind = null,
            Country registeredCountry = null, RepresentedCountry representedCountry = null, Traits traits = null)
            : base(continent, country, maxMind, registeredCountry, representedCountry, traits)
        {
        }
    }
}