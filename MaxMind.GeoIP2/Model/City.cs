using MaxMind.Db;
using System.Text.Json.Serialization;

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     City-level data associated with an IP address.
    /// </summary>
    /// <remarks>
    ///     Do not use any of the city names as a database or dictionary
    ///     key. Use the <see cred="GeoNameId" /> instead.
    /// </remarks>
    public record City : NamedEntity
    {
        /// <summary>
        ///     A value from 0-100 indicating MaxMind's confidence that the city
        ///     is correct. This value is only set when using the Insights
        ///     web service or the Enterprise database.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("confidence")]
        [MapKey("confidence")]
        public int? Confidence { get; init; }
    }
}
