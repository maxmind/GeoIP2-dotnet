using System.Text.Json.Serialization;

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Contains data related to your MaxMind account.
    /// </summary>
    public record MaxMind
    {
        /// <summary>
        ///     The number of remaining queries in your account for the web
        ///     service end point. This will be null when using a local
        ///     database.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("queries_remaining")]
        public int? QueriesRemaining { get; init; }
    }
}
