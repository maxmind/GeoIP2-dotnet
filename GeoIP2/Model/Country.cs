using Newtonsoft.Json;
using System.Collections.Generic;

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Contains data for the country record associated with an IP address.
    /// </summary>
    public class Country : NamedEntity
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public Country()
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public Country(int? confidence = null, int? geoNameId = null, string isoCode = null,
            Dictionary<string, string> names = null, List<string> locales = null)
            : base(geoNameId, names, locales)
        {
            Confidence = confidence;
            IsoCode = isoCode;
        }

        /// <summary>
        ///     A value from 0-100 indicating MaxMind's confidence that the country
        ///     is correct. This attribute is only available from the Insights web
        ///     service end point.
        /// </summary>
        [JsonProperty("confidence")]
        public int? Confidence { get; internal set; }

        /// <summary>
        ///     The
        ///     <a
        ///         href="http://en.wikipedia.org/wiki/ISO_3166-1">
        ///         two-character ISO
        ///         3166-1 alpha code
        ///     </a>
        ///     for the country.
        /// </summary>
        [JsonProperty("iso_code")]
        public string IsoCode { get; internal set; }
    }
}