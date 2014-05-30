using System.Collections.Generic;
using MaxMind.GeoIP2.Model;

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    /// This class provides a model for the data returned by the GeoIP2 City/ISP/Org
    /// end point.
    /// 
    /// The only difference between the City, City/ISP/Org, and Omni response classes is
    /// which fields in each record may be populated.
    /// 
    /// <a href="http://dev.maxmind.com/geoip/geoip2/web-services">GeoIP2 Web
    ///      Services</a>
    /// </summary>
    public class CityIspOrgResponse : AbstractCityResponse
    {
		public CityIspOrgResponse() { }

	    public CityIspOrgResponse(City city = null, Location location = null, Postal postal = null,
		    List<Subdivision> subdivisions = null,
		    Continent continent = null, Country country = null, Model.MaxMind maxMind = null, Country registeredCountry = null,
		    RepresentedCountry representedCountry = null, Traits traits = null)
		    : base(
			    city, location, postal, subdivisions, continent, country, maxMind, registeredCountry, representedCountry, traits)
	    {
	    }
    }
}