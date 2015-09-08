#region

using Newtonsoft.Json;

#endregion

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     This class represents the GeoIP2 Anonymous IP response.
    /// </summary>
    public class AnonymousIPResponse : AbstractResponse
    {
        /// <summary>
        ///     Returns true if the IP address belongs to any sort of anonymous network.
        /// </summary>
        [JsonProperty("is_anonymous")]
        public bool IsAnonymous { get; internal set; }

        /// <summary>
        ///     Returns true if the IP address belongs to an anonymous VPN system.
        /// </summary>
        [JsonProperty("is_anonymous_vpn")]
        public bool IsAnonymousVpn { get; internal set; }

        /// <summary>
        ///     Returns true if the IP address belongs to a hosting provider.
        /// </summary>
        [JsonProperty("is_hosting_provider")]
        public bool IsHostingProvider { get; internal set; }

        /// <summary>
        ///     Returns true if the IP address belongs to a public proxy.
        /// </summary>
        [JsonProperty("is_public_proxy")]
        public bool IsPublicProxy { get; internal set; }

        /// <summary>
        ///     Returns true if IP is a Tor exit node.
        /// </summary>
        [JsonProperty("is_tor_exit_node")]
        public bool IsTorExitNode { get; internal set; }

        /// <summary>
        ///     The IP address that the data in the model is for. If you
        ///     performed a "me" lookup against the web service, this will be the
        ///     externally routable IP address for the system the code is running
        ///     on. If the system is behind a NAT, this may differ from the IP
        ///     address locally assigned to it.
        /// </summary>
        [JsonProperty("ip_address")]
        public string IPAddress { get; internal set; }
    }
}