using MaxMind.GeoIP2.Model;
using System.Text.Json.Serialization;

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     This record provides a model for the data returned by the GeoIP
    ///     Insights web service.
    /// </summary>
    public record InsightsResponse : AbstractCityResponse
    {
        private Anonymizer _anonymizer = new();

        /// <summary>
        ///     Gets anonymizer-related data for the requested IP address.
        ///     This is available from the GeoIP Insights web service.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("anonymizer")]
        public Anonymizer Anonymizer
        {
            get => _anonymizer;
            init => _anonymizer = value ?? new();
        }
    }
}
