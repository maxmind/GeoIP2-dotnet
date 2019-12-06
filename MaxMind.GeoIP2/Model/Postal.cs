#region

using MaxMind.Db;
using Newtonsoft.Json;

#endregion

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Contains data for the postal record associated with an IP address.
    /// </summary>
    public class Postal
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public Postal()
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        [Constructor]
        public Postal(string? code = null, int? confidence = null)
        {
            Code = code;
            Confidence = confidence;
        }

        /// <summary>
        ///     The postal code of the location. Postal codes are not available
        ///     for all countries. In some countries, this will only contain part
        ///     of the postal code.
        /// </summary>
        [JsonProperty("code")]
        public string? Code { get; internal set; }

        /// <summary>
        ///     A value from 0-100 indicating MaxMind's confidence that the
        ///     postal code is correct. This value is only set when using the
        ///     Insights web service or the Enterprise database.
        /// </summary>
        [JsonProperty("confidence")]
        public int? Confidence { get; internal set; }

        /// <summary>
        ///     Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"Code: {Code}, Confidence: {Confidence}";
        }
    }
}