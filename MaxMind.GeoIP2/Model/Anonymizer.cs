using System;
using System.Text.Json.Serialization;

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Contains anonymizer-related data associated with an IP address.
    ///     This data is available from the GeoIP2 Insights web service.
    /// </summary>
    public record Anonymizer
    {
        /// <summary>
        ///     A score ranging from 1 to 99 that represents our percent confidence
        ///     that the network is currently part of an actively used VPN service.
        ///     This is available from the GeoIP2 Insights web service.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("confidence")]
        public int? Confidence { get; init; }

        /// <summary>
        ///     This is true if the IP address belongs to any sort of anonymous
        ///     network. This is available from the GeoIP2 Insights web service.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("is_anonymous")]
        public bool IsAnonymous { get; init; }

        /// <summary>
        ///     This is true if the IP address is registered to an anonymous
        ///     VPN provider. This is available from the GeoIP2 Insights web
        ///     service.
        /// </summary>
        /// <remarks>
        ///     If a VPN provider does not register subnets under names
        ///     associated with them, we will likely only flag their IP ranges
        ///     using the IsHostingProvider property.
        /// </remarks>
        [JsonInclude]
        [JsonPropertyName("is_anonymous_vpn")]
        public bool IsAnonymousVpn { get; init; }

        /// <summary>
        ///     This is true if the IP address belongs to a hosting or VPN
        ///     provider (see description of IsAnonymousVpn property).
        ///     This is available from the GeoIP2 Insights web service.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("is_hosting_provider")]
        public bool IsHostingProvider { get; init; }

        /// <summary>
        ///     This is true if the IP address belongs to a public proxy.
        ///     This is available from the GeoIP2 Insights web service.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("is_public_proxy")]
        public bool IsPublicProxy { get; init; }

        /// <summary>
        ///     This is true if the IP address is on a suspected anonymizing
        ///     network and belongs to a residential ISP. This is available
        ///     from the GeoIP2 Insights web service.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("is_residential_proxy")]
        public bool IsResidentialProxy { get; init; }

        /// <summary>
        ///     This is true if the IP address belongs to a Tor exit node.
        ///     This is available from the GeoIP2 Insights web service.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("is_tor_exit_node")]
        public bool IsTorExitNode { get; init; }

#if NET6_0_OR_GREATER
        /// <summary>
        ///     The last day that the network was sighted in our analysis of
        ///     anonymized networks. This is available from the GeoIP2 Insights
        ///     web service.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("network_last_seen")]
        public DateOnly? NetworkLastSeen { get; init; }
#endif

        /// <summary>
        ///     The name of the VPN provider (e.g., NordVPN, SurfShark)
        ///     associated with the network. This is available from the
        ///     GeoIP2 Insights web service.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("provider_name")]
        public string? ProviderName { get; init; }
    }
}
