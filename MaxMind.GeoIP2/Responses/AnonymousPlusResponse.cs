using MaxMind.Db;
using System;
using System.Text.Json.Serialization;

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     This class represents the GeoIP Anonymous Plus response.
    /// </summary>
    public record AnonymousPlusResponse : AnonymousIPResponse
    {
        /// <summary>
        ///     A score ranging from 1 to 99 that is our percent confidence
        ///     that the network is currently part of an actively used VPN
        ///     service.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("anonymizer_confidence")]
        [MapKey("anonymizer_confidence")]
        public int? AnonymizerConfidence { get; init; }

#if NET6_0_OR_GREATER
        private DateOnly? _networkLastSeen;

        /// <summary>
        ///     The last day that the network was sighted in our analysis of
        ///     anonymized networks.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("network_last_seen")]
        public DateOnly? NetworkLastSeen
        {
            get => _networkLastSeen;
            init => _networkLastSeen = value;
        }

        /// <summary>
        ///     Internal property for MMDB deserialization where the value
        ///     is stored as a string.
        /// </summary>
        [JsonIgnore]
        [MapKey("network_last_seen")]
        internal string? NetworkLastSeenString
        {
            get => _networkLastSeen?.ToString("o");
            init => _networkLastSeen = value == null ? null : DateOnly.Parse(value);
        }
#endif

        /// <summary>
        ///     The name of the VPN provider (e.g., NordVPN, SurfShark, etc.)
        ///     associated with the network.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("provider_name")]
        [MapKey("provider_name")]
        public string? ProviderName { get; init; }
    }
}
