#region

using System.Collections.Generic;
using Newtonsoft.Json;

#endregion

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Contains data for the subdivisions associated with an IP address.
    /// </summary>
    public class Subdivision : NamedEntity
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public Subdivision()
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public Subdivision(int? confidence = null, int? geoNameId = null, string isoCode = null,
            Dictionary<string, string> names = null, IEnumerable<string> locales = null)
            : base(geoNameId, names, locales)
        {
            Confidence = confidence;
            IsoCode = isoCode;
        }

        /// <summary>
        ///     This is a value from 0-100 indicating MaxMind's confidence that
        ///     the subdivision is correct. This attribute is only available from
        ///     the Insights web service end point.
        /// </summary>
        [JsonProperty("confidence")]
        public int? Confidence { get; set; }

        /// <summary>
        ///     This is a string up to three characters long contain the
        ///     subdivision portion of the
        ///     <a
        ///         href="http://en.wikipedia.org/wiki/ISO_3166-2 ISO 3166-2">
        ///         code
        ///     </a>
        ///     .
        /// </summary>
        [JsonProperty("iso_code")]
        public string IsoCode { get; set; }
    }
}