using System.Collections.Generic;
using Newtonsoft.Json;

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    /// City-level data associated with an IP address.
    /// </summary>
    public class City : NamedEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public City()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public City(int? confidence = null, int? geoNameId = null, Dictionary<string, string> names = null, List<string> locales = null)
            : base(geoNameId, names, locales)
        {
            Confidence = confidence;
        }

        /// <summary>
        /// A value from 0-100 indicating MaxMind's confidence that the city
        /// is correct.      
        /// </summary>
        [JsonProperty("confidence")]
        public int? Confidence { get; internal set; }
    }
}