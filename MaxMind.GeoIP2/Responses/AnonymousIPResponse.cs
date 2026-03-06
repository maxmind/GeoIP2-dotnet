using MaxMind.Db;
using System.Text.Json.Serialization;

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     This class represents the GeoIP2 Anonymous IP response.
    /// </summary>
    public record AnonymousIPResponse : AbstractResponse
    {
        /// <summary>
        ///     Returns true if the IP address belongs to any sort of anonymous network.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("is_anonymous")]
        [MapKey("is_anonymous")]
        public bool IsAnonymous { get; init; }

        /// <summary>
        ///     Returns true if the IP address is registered to an anonymous
        ///     VPN provider.
        /// </summary>
        /// <remarks>
        ///     If a VPN provider does not register subnets under names
        ///     associated with them, we will likely only flag their IP ranges
        ///     using the IsHostingProvider property.
        /// </remarks>
        [JsonInclude]
        [JsonPropertyName("is_anonymous_vpn")]
        [MapKey("is_anonymous_vpn")]
        public bool IsAnonymousVpn { get; init; }

        /// <summary>
        ///     Returns true if the IP address belongs to a hosting or
        ///     VPN provider (see description of IsAnonymousVpn property).
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("is_hosting_provider")]
        [MapKey("is_hosting_provider")]
        public bool IsHostingProvider { get; init; }

        /// <summary>
        ///     Returns true if the IP address belongs to a public proxy.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("is_public_proxy")]
        [MapKey("is_public_proxy")]
        public bool IsPublicProxy { get; init; }

        /// <summary>
        ///     This is true if the IP address is on a suspected anonymizing
        ///     network and belongs to a residential ISP.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("is_residential_proxy")]
        [MapKey("is_residential_proxy")]
        public bool IsResidentialProxy { get; init; }

        /// <summary>
        ///     Returns true if IP is a Tor exit node.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("is_tor_exit_node")]
        [MapKey("is_tor_exit_node")]
        public bool IsTorExitNode { get; init; }

        /// <summary>
        ///     The IP address that the data in the model is for. If you
        ///     performed a "me" lookup against the web service, this will be the
        ///     externally routable IP address for the system the code is running
        ///     on. If the system is behind a NAT, this may differ from the IP
        ///     address locally assigned to it.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("ip_address")]
        [Inject("ip_address")]
        public string? IPAddress { get; init; }

        /// <summary>
        ///     The network associated with the record. In particular, this is
        ///     the largest network where all of the fields besides
        ///     <c>IPAddress</c> have the same value.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("network")]
        [Network]
        public Network? Network { get; init; }
    }
}
