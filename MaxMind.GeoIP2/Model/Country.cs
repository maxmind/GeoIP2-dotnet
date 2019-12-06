#region

using MaxMind.Db;
using Newtonsoft.Json;
using System.Collections.Generic;

#endregion

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Contains data for the country record associated with an IP address.
    ///     Do not use any of the country names as a database or dictionary
    ///     key. Use the <see cred="GeoNameId" /> or <see cred="IsoCode" />
    ///     instead.
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
        [Constructor]
        public Country(
            int? confidence = null,
            [Parameter("geoname_id")] long? geoNameId = null,
            [Parameter("is_in_european_union")] bool isInEuropeanUnion = false,
            [Parameter("iso_code")] string? isoCode = null,
            IDictionary<string, string>? names = null,
            IEnumerable<string>? locales = null)
            : base(geoNameId, names, locales)
        {
            Confidence = confidence;
            IsoCode = isoCode;
            IsInEuropeanUnion = isInEuropeanUnion;
        }

        /// <summary>
        ///     A value from 0-100 indicating MaxMind's confidence that the country
        ///     is correct. This value is only set when using the Insights
        ///     web service or the Enterprise database.
        /// </summary>
        [JsonProperty("confidence")]
        public int? Confidence { get; internal set; }

        /// <summary>
        ///     This is true if the country is a member state of the
        ///     European Union. This is available from  all location
        ///     services and databases.
        /// </summary>
        [JsonProperty("is_in_european_union")]
        public bool IsInEuropeanUnion { get; internal set; }

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
        public string? IsoCode { get; internal set; }
    }
}