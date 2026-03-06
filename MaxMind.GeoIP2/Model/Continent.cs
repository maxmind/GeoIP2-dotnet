using MaxMind.Db;
using System.Text.Json.Serialization;

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Contains data for the continent record associated with an IP address.
    ///     Do not use any of the continent names as a database or dictionary
    ///     key. Use the <see cred="GeoNameId" /> or <see cred="Code" />
    ///     instead.
    /// </summary>
    public record Continent : NamedEntity
    {
        /// <summary>
        ///     A two character continent code like "NA" (North America) or "OC"
        ///     (Oceania).
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("code")]
        [MapKey("code")]
        public string? Code { get; init; }
    }
}
