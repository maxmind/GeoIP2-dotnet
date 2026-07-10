using System;
using System.Text.Json.Serialization;

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Contains data for one type of anonymizer detection, currently
    ///     residential proxies. Additional feeds may be added in the
    ///     future. This record is returned by the GeoIP Insights web
    ///     service.
    /// </summary>
    public record AnonymizerFeed
    {
        /// <summary>
        ///     A score ranging from 1 to 99 that represents our percent
        ///     confidence that the network is currently part of this
        ///     anonymizer feed. This is available from the GeoIP Insights
        ///     web service.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("confidence")]
        public int? Confidence { get; init; }

#if NET6_0_OR_GREATER
        /// <summary>
        ///     The last day that the network was sighted in our analysis of
        ///     this anonymizer feed. This is available from the GeoIP
        ///     Insights web service.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("network_last_seen")]
        public DateOnly? NetworkLastSeen { get; init; }
#endif

        /// <summary>
        ///     The name of the provider associated with the network in this
        ///     anonymizer feed. This is available from the GeoIP Insights
        ///     web service.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("provider_name")]
        public string? ProviderName { get; init; }
    }
}
