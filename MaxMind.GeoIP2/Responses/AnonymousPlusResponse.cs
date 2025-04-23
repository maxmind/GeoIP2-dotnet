#region

using MaxMind.Db;
using System;
using System.Text.Json.Serialization;

#endregion

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     This class represents the GeoIP Anonymous Plus response.
    /// </summary>
    public class AnonymousPlusResponse : AnonymousIPResponse
    {
        /// <summary>
        /// Construct AnonymousPlusResponse model
        /// </summary>
        public AnonymousPlusResponse()
        {
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Construct AnonymousPlusResponse model
        /// </summary>
        /// <param name="anonymizerConfidence"></param>
        /// <param name="isAnonymous"></param>
        /// <param name="isAnonymousVpn"></param>
        /// <param name="isHostingProvider"></param>
        /// <param name="isPublicProxy"></param>
        /// <param name="isResidentialProxy"></param>
        /// <param name="isTorExitNode"></param>
        /// <param name="ipAddress"></param>
        /// <param name="network"></param>
        /// <param name="networkLastSeen"></param>
        /// <param name="providerName"></param>
        [Constructor]
        public AnonymousPlusResponse(
            [Parameter("anonymizer_confidence")] int? anonymizerConfidence,
            [Parameter("is_anonymous")] bool isAnonymous,
            [Parameter("is_anonymous_vpn")] bool isAnonymousVpn,
            [Parameter("is_hosting_provider")] bool isHostingProvider,
            [Parameter("is_public_proxy")] bool isPublicProxy,
            [Parameter("is_residential_proxy")] bool isResidentialProxy,
            [Parameter("is_tor_exit_node")] bool isTorExitNode,
            [Inject("ip_address")] string? ipAddress,
            [Network] Network? network = null,
            [Parameter("network_last_seen")] string? networkLastSeen = null,
            [Parameter("provider_name")] string? providerName = null
        ) : this(anonymizerConfidence, isAnonymous, isAnonymousVpn, isHostingProvider, isPublicProxy,
                isResidentialProxy, isTorExitNode, ipAddress, network,
                networkLastSeen == null ? null : DateOnly.Parse(networkLastSeen),
                providerName
            )
        { }

        /// <summary>
        /// Construct AnonymousPlusResponse model
        /// </summary>
        /// <param name="anonymizerConfidence"></param>
        /// <param name="isAnonymous"></param>
        /// <param name="isAnonymousVpn"></param>
        /// <param name="isHostingProvider"></param>
        /// <param name="isPublicProxy"></param>
        /// <param name="isResidentialProxy"></param>
        /// <param name="isTorExitNode"></param>
        /// <param name="ipAddress"></param>
        /// <param name="network"></param>
        /// <param name="networkLastSeen"></param>
        /// <param name="providerName"></param>
        public AnonymousPlusResponse(
            int? anonymizerConfidence,
            bool isAnonymous,
            bool isAnonymousVpn,
            bool isHostingProvider,
            bool isPublicProxy,
            bool isResidentialProxy,
            bool isTorExitNode,
            string? ipAddress,
            Network? network = null,
            DateOnly? networkLastSeen = null,
            string? providerName = null
        ) : base(isAnonymous, isAnonymousVpn, isHostingProvider, isPublicProxy,
                isResidentialProxy, isTorExitNode, ipAddress, network)
        {
            AnonymizerConfidence = anonymizerConfidence;
            NetworkLastSeen = networkLastSeen;
            ProviderName = providerName;
        }
#else
        /// <summary>
        /// Construct AnonymousPlusResponse model
        /// </summary>
        /// <param name="anonymizerConfidence"></param>
        /// <param name="isAnonymous"></param>
        /// <param name="isAnonymousVpn"></param>
        /// <param name="isHostingProvider"></param>
        /// <param name="isPublicProxy"></param>
        /// <param name="isResidentialProxy"></param>
        /// <param name="isTorExitNode"></param>
        /// <param name="ipAddress"></param>
        /// <param name="network"></param>
        /// <param name="providerName"></param>
        [Constructor]
        public AnonymousPlusResponse(
            [Parameter("anonymizer_confidence")] int anonymizerConfidence,
            [Parameter("is_anonymous")] bool isAnonymous,
            [Parameter("is_anonymous_vpn")] bool isAnonymousVpn,
            [Parameter("is_hosting_provider")] bool isHostingProvider,
            [Parameter("is_public_proxy")] bool isPublicProxy,
            [Parameter("is_residential_proxy")] bool isResidentialProxy,
            [Parameter("is_tor_exit_node")] bool isTorExitNode,
            [Inject("ip_address")] string? ipAddress,
            [Network] Network? network = null,
            [Parameter("provider_name")] string? providerName = null
        ) : base(isAnonymous, isAnonymousVpn, isHostingProvider, isPublicProxy,
                isResidentialProxy, isTorExitNode, ipAddress, network)
        {
            AnonymizerConfidence = anonymizerConfidence;
            ProviderName = providerName;
        }
#endif

        /// <summary>
        ///     A score ranging from 1 to 99 that is our percent confidence
        ///     that the network is currently part of an actively used VPN
        ///     service.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("anonymizer_confidence")]
        public int? AnonymizerConfidence { get; internal set; }

#if NET6_0_OR_GREATER
        /// <summary>
        ///     The last day that the network was sighted in our analysis of
        ///     anonymized networks.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("network_last_seen")]
        public DateOnly? NetworkLastSeen { get; internal set; }
#endif

        /// <summary>
        ///     The name of the VPN provider (e.g., NordVPN, SurfShark, etc.)
        ///     associated with the network.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("provider_name")]
        public string? ProviderName { get; internal set; }
    }
}
