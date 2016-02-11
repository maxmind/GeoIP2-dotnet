#region

using MaxMind.Db;
using Newtonsoft.Json;
using System.Collections.Generic;

#endregion

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Contains data for the subdivisions associated with an IP address.
    /// </summary>
    /// <remarks>
    //      Do not use any of the subdivions names as a database or dictionary
    //      key. Use the <see cred="GeoNameId" /> or <see cred="IsoCode">
    //      instead.
    /// </remarks>
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
            IDictionary<string, string> names = null, IEnumerable<string> locales = null)
            : base(geoNameId, names, locales)
        {
            Confidence = confidence;
            IsoCode = isoCode;
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        [Constructor]
        public Subdivision(
            int? confidence = null,
            // See note in City
            [Parameter("geoname_id")] long? geoNameId = null,
            [Parameter("iso_code")] string isoCode = null,
            IDictionary<string, string> names = null,
            IEnumerable<string> locales = null)
            : this(confidence, (int?)geoNameId, isoCode, names, locales)
        {
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
