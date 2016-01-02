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
        ///     is correct.
        /// </summary>
        [JsonProperty("confidence")]
        public int? Confidence { get; internal set; }
    }
}