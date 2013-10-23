using Newtonsoft.Json;

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    /// Contains data for the postal record associated with an IP address.
    /// This record is returned by all the end points except the Country end point.
    /// </summary>
    public class Postal
    {
        /// <summary>
        /// The postal code of the location. Postal codes are not available
        /// for all countries. In some countries, this will only contain part
        /// of the postal code. This attribute is returned by all end points
        /// except the Country end point.
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; internal set; }

        /// <summary>
        /// A value from 0-100 indicating MaxMind's confidence that the
        /// postal code is correct. This attribute is only available from the
        /// Omni end point.
        /// </summary>
        [JsonProperty("confidence")]
        public int? Confidence { get; internal set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Code ?? string.Empty;
        }
    }
}