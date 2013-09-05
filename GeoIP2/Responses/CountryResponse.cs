using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MaxMind.GeoIP2.Model;

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    /// This class provides a model for the data returned by the GeoIP2 Country end
    /// point.
    /// 
    /// The only difference between the City, City/ISP/Org, and Omni response classes is
    /// which fields in each record may be populated.
    /// 
    /// See <a href="http://dev.maxmind.com/geoip/geoip2/web-services">GeoIP2 Web
    ///      Services</a>
    /// </summary>
    public class CountryResponse
    {
        /// <summary>
        /// Gets the continent for the requested IP address.
        /// </summary>
        public Continent Continent { get; internal set; }

        /// <summary>
        /// Gets the country for the requested IP address. This
        /// object represents the country where MaxMind believes
        /// the end user is located
        /// </summary>
        public Country Country { get; internal set; }

        /// <summary>
        /// Gets the MaxMind record containing data related to your account
        /// </summary>
        public Model.MaxMind MaxMind { get; internal set; }

        /// <summary>
        /// Registered country record for the requested IP address. This
        /// record represents the country where the ISP has registered a
        /// given IP block and may differ from the user's country.
        /// </summary>
        public Country RegisteredCountry { get; internal set; }

        /// <summary>
        /// Represented country record for the requested IP address. The
        /// represented country is used for things like military bases or
        /// embassies. It is only present when the represented country
        /// differs from the country.
        /// </summary>
        public RepresentedCountry RepresentedCountry { get; internal set; }

        /// <summary>
        /// Gets the traits for the requested IP address.
        /// </summary>
        public Traits Traits { get; internal set; }
    }
}
