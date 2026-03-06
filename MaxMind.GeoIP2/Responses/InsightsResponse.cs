using MaxMind.GeoIP2.Model;
using System.Text.Json.Serialization;

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     This class provides a model for the data returned by the GeoIP2
    ///     Insights web service.
    /// </summary>
    public record InsightsResponse : AbstractCityResponse
    {
        /// <summary>
        ///     Gets anonymizer-related data for the requested IP address.
        ///     This is available from the GeoIP2 Insights web service.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("anonymizer")]
        public Anonymizer Anonymizer { get; init; } = new();
    }
}
