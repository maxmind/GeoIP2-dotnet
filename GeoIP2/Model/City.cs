using System.Collections.Generic;
using Newtonsoft.Json;

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    /// City-level data associated with an IP address.
    ///
    /// This record is returned by all the end points except the Country end point.
    /// </summary>
    public class City : NamedEntity
    {
        public City()
        {
        }
        public City(int? confidence = null, Dictionary<string, string> names = null, int? geoNameId = null, List<string> locales = null) : base(names, geoNameId, locales)
        {
            Confidence = confidence;
        }

        /// <summary>
        /// A value from 0-100 indicating MaxMind's confidence that the city
        /// is correct. This attribute is only available from the Omni end
        /// point.        
        /// </summary>
        [JsonProperty("confidence")]
        public int? Confidence { get; internal set; }
    }
}