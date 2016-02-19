#region

using MaxMind.Db;
using Newtonsoft.Json;
using System.Collections.Generic;

#endregion

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     City-level data associated with an IP address.
    /// </summary>
    /// <remarks>
    ///     Do not use any of the city names as a database or dictionary
    ///     key. Use the <see cred="GeoNameId" /> instead.
    /// </remarks>
    public class City : NamedEntity
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public City()
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public City(int? confidence = null, int? geoNameId = null, IDictionary<string, string> names = null,
            IEnumerable<string> locales = null)
            : base(geoNameId, names, locales)
        {
            Confidence = confidence;
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        [Constructor]
        public City(int? confidence = null,
            // Unfortunately the existing models incorrectly use an int rather
            // than a long for the geoname_id. This should be corrected if we
            // ever do a major version bump.
            [Parameter("geoname_id")] long? geoNameId = null,
            IDictionary<string, string> names = null,
            IEnumerable<string> locales = null)
            : this(confidence, (int?)geoNameId, names, locales)
        {
        }

        /// <summary>
        ///     A value from 0-100 indicating MaxMind's confidence that the city
        ///     is correct. This value is only set when using the Insights
        ///     web service or the Enterprise database.
        /// </summary>
        [JsonProperty("confidence")]
        public int? Confidence { get; internal set; }
    }
}