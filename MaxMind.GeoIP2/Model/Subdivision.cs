#region

using MaxMind.Db;
using Newtonsoft.Json;
using System.Collections.Generic;

#endregion

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Contains data for the subdivisions associated with an IP address.
    ///     Do not use any of the subdivision names as a database or dictionary
    ///     key. Use the <see cred="GeoNameId" /> or <see cred="IsoCode" />
    ///     instead.
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
        [Constructor]
        public Subdivision(
            int? confidence = null,
            [Parameter("geoname_id")] long? geoNameId = null,
            [Parameter("iso_code")] string? isoCode = null,
            IDictionary<string, string>? names = null,
            IEnumerable<string>? locales = null)
            : base(geoNameId, names, locales)
        {
            Confidence = confidence;
            IsoCode = isoCode;
        }

        /// <summary>
        ///     This is a value from 0-100 indicating MaxMind's confidence that
        ///     the subdivision is correct. This value is only set when using the
        ///     Insights web service or the Enterprise database.
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
        public string? IsoCode { get; set; }
    }
}