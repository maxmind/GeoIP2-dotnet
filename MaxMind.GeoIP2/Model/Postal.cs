using MaxMind.Db;
using System.Text.Json.Serialization;

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Contains data for the postal record associated with an IP address.
    /// </summary>
    public record Postal
    {
        /// <summary>
        ///     The postal code of the location. Postal codes are not available
        ///     for all countries. In some countries, this will only contain part
        ///     of the postal code.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("code")]
        [MapKey("code")]
        public string? Code { get; init; }

        /// <summary>
        ///     A value from 0-100 indicating MaxMind's confidence that the
        ///     postal code is correct. This value is only set when using the
        ///     Insights web service or the Enterprise database.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("confidence")]
        [MapKey("confidence")]
        public int? Confidence { get; init; }
    }
}
