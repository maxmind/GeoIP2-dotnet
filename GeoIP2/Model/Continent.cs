#region

using MaxMind.Db;
using Newtonsoft.Json;
using System.Collections.Generic;

#endregion

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Contains data for the continent record associated with an IP address.
    /// </summary>
    public class Continent : NamedEntity
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public Continent()
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public Continent(string code = null, int? geoNameId = null, IDictionary<string, string> names = null,
            IEnumerable<string> locales = null)
            : base(geoNameId, names, locales)
        {
            Code = code;
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        [Constructor]
        public Continent(
            string code = null,
            // See note in City model
            [Parameter("geoname_id")] long? geoNameId = null,
            IDictionary<string, string> names = null,
            IEnumerable<string> locales = null)
            : this(code, (int?)geoNameId, names, locales)
        {
        }

        /// <summary>
        ///     A two character continent code like "NA" (North America) or "OC"
        ///     (Oceania).
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; internal set; }
    }
}