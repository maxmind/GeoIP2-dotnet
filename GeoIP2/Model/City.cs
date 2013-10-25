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
        /// <summary>
        /// A value from 0-100 indicating MaxMind's confidence that the city
        /// is correct. This attribute is only available from the Omni end
        /// point.        
        /// </summary>
        [JsonProperty("confidence")]
        public int? Confidence { get; internal set; }
    }
}