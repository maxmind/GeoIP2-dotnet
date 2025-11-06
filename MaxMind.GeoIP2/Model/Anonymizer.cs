#region

using System;
using System.Text.Json.Serialization;

#endregion

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Contains anonymizer-related data associated with an IP address.
    ///     This data is available from the GeoIP2 Insights web service.
    /// </summary>
    public class Anonymizer
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public Anonymizer()
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public Anonymizer(
            int? confidence = null,
            bool isAnonymous = false,
            bool isAnonymousVpn = false,
            bool isHostingProvider = false,
            bool isPublicProxy = false,
            bool isResidentialProxy = false,
            bool isTorExitNode = false,
#if NET6_0_OR_GREATER
            DateOnly? networkLastSeen = null,
#endif
            string? providerName = null
        )
        {
            Confidence = confidence;
            IsAnonymous = isAnonymous;
            IsAnonymousVpn = isAnonymousVpn;
            IsHostingProvider = isHostingProvider;
            IsPublicProxy = isPublicProxy;
            IsResidentialProxy = isResidentialProxy;
            IsTorExitNode = isTorExitNode;
#if NET6_0_OR_GREATER
            NetworkLastSeen = networkLastSeen;
#endif
            ProviderName = providerName;
        }

        /// <summary>
        ///     A score ranging from 1 to 99 that represents our percent confidence
        ///     that the network is currently part of an actively used VPN service.
        ///     This is available from the GeoIP2 Insights web service.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("confidence")]
        public int? Confidence { get; internal set; }

        /// <summary>
        ///     This is true if the IP address belongs to any sort of anonymous
        ///     network. This is available from the GeoIP2 Insights web service.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("is_anonymous")]
        public bool IsAnonymous { get; internal set; }

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
        public bool IsAnonymousVpn { get; internal set; }

        /// <summary>
        ///     This is true if the IP address belongs to a hosting or VPN
        ///     provider (see description of IsAnonymousVpn property).
        ///     This is available from the GeoIP2 Insights web service.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("is_hosting_provider")]
        public bool IsHostingProvider { get; internal set; }

        /// <summary>
        ///     This is true if the IP address belongs to a public proxy.
        ///     This is available from the GeoIP2 Insights web service.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("is_public_proxy")]
        public bool IsPublicProxy { get; internal set; }

        /// <summary>
        ///     This is true if the IP address is on a suspected anonymizing
        ///     network and belongs to a residential ISP. This is available
        ///     from the GeoIP2 Insights web service.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("is_residential_proxy")]
        public bool IsResidentialProxy { get; internal set; }

        /// <summary>
        ///     This is true if the IP address belongs to a Tor exit node.
        ///     This is available from the GeoIP2 Insights web service.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("is_tor_exit_node")]
        public bool IsTorExitNode { get; internal set; }

#if NET6_0_OR_GREATER
        /// <summary>
        ///     The last day that the network was sighted in our analysis of
        ///     anonymized networks. This is available from the GeoIP2 Insights
        ///     web service.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("network_last_seen")]
        public DateOnly? NetworkLastSeen { get; internal set; }
#endif

        /// <summary>
        ///     The name of the VPN provider (e.g., NordVPN, SurfShark)
        ///     associated with the network. This is available from the
        ///     GeoIP2 Insights web service.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("provider_name")]
        public string? ProviderName { get; internal set; }

        /// <summary>
        ///     Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{nameof(Confidence)}: {Confidence}, " +
                $"{nameof(IsAnonymous)}: {IsAnonymous}, " +
                $"{nameof(IsAnonymousVpn)}: {IsAnonymousVpn}, " +
                $"{nameof(IsHostingProvider)}: {IsHostingProvider}, " +
                $"{nameof(IsPublicProxy)}: {IsPublicProxy}, " +
                $"{nameof(IsResidentialProxy)}: {IsResidentialProxy}, " +
                $"{nameof(IsTorExitNode)}: {IsTorExitNode}, " +
#if NET6_0_OR_GREATER
                $"{nameof(NetworkLastSeen)}: {NetworkLastSeen}, " +
#endif
                $"{nameof(ProviderName)}: {ProviderName}";
        }
    }
}
