#region

using MaxMind.Db;
using Newtonsoft.Json;
using System;

#endregion

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Contains data for the traits record associated with an IP address.
    /// </summary>
    public class Traits
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public Traits()
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        [Constructor]
        public Traits(
            [Parameter("autonomous_system_number")] long? autonomousSystemNumber = null,
            [Parameter("autonomous_system_organization")] string? autonomousSystemOrganization = null,
            [Parameter("connection_type")] string? connectionType = null,
            string? domain = null,
            [Inject("ip_address")] string? ipAddress = null,
            [Parameter("is_anonymous")] bool isAnonymous = false,
            [Parameter("is_anonymous_proxy")] bool isAnonymousProxy = false,
            [Parameter("is_anonymous_vpn")] bool isAnonymousVpn = false,
            [Parameter("is_hosting_provider")] bool isHostingProvider = false,
            [Parameter("is_legitimate_proxy")] bool isLegitimateProxy = false,
            [Parameter("is_public_proxy")] bool isPublicProxy = false,
            [Parameter("is_residential_proxy")] bool isResidentialProxy = false,
            [Parameter("is_satellite_provider")] bool isSatelliteProvider = false,
            [Parameter("is_tor_exit_node")] bool isTorExitNode = false,
            string? isp = null,
            string? organization = null,
            [Parameter("user_type")] string? userType = null,
            [Network] Network? network = null,
            [Parameter("static_ip_score")] double? staticIPScore = null,
            [Parameter("user_count")] int? userCount = null
        )
        {
            AutonomousSystemNumber = autonomousSystemNumber;
            AutonomousSystemOrganization = autonomousSystemOrganization;
            ConnectionType = connectionType;
            Domain = domain;
            IPAddress = ipAddress;
            IsAnonymous = isAnonymous;
#pragma warning disable 618
            IsAnonymousProxy = isAnonymousProxy;
#pragma warning restore 618
            IsAnonymousVpn = isAnonymousVpn;
            IsHostingProvider = isHostingProvider;
            IsLegitimateProxy = isLegitimateProxy;
            IsPublicProxy = isPublicProxy;
            IsResidentialProxy = isResidentialProxy;
#pragma warning disable 618
            IsSatelliteProvider = isSatelliteProvider;
#pragma warning restore 618
            IsTorExitNode = isTorExitNode;
            Isp = isp;
            Network = network;
            Organization = organization;
            StaticIPScore = staticIPScore;
            UserCount = userCount;
            UserType = userType;
        }

        /// <summary>
        ///     Constructor for binary compatibility.
        /// </summary>
        [Obsolete("For binary compatibility only")]
        public Traits(
            long? autonomousSystemNumber,
            string? autonomousSystemOrganization,
            string? connectionType,
            string? domain,
            string? ipAddress,
            bool isAnonymous,
            bool isAnonymousProxy,
            bool isAnonymousVpn,
            bool isHostingProvider,
            bool isLegitimateProxy,
            bool isPublicProxy,
            bool isSatelliteProvider,
            bool isTorExitNode,
            string? isp,
            string? organization,
            string? userType,
            Network? network,
            double? staticIPScore,
            int? userCount
        ) : this(autonomousSystemNumber, autonomousSystemOrganization, connectionType, domain,
                 ipAddress, isAnonymous, isAnonymousProxy, isAnonymousVpn, isHostingProvider,
                 isLegitimateProxy, isPublicProxy, false, isSatelliteProvider, isTorExitNode,
                 isp, organization, userType, network, staticIPScore, userCount)
        {
        }

        /// <summary>
        ///     The
        ///     <a
        ///         href="http://en.wikipedia.org/wiki/Autonomous_system_(Internet)">
        ///         autonomous system number
        ///     </a>
        ///     associated with the IP address.
        ///     This value is only set when using the City or Insights web
        ///     service or the Enterprise database.
        /// </summary>
        [JsonProperty("autonomous_system_number")]
        public long? AutonomousSystemNumber { get; internal set; }

        /// <summary>
        ///     The organization associated with the registered
        ///     <a
        ///         href="http://en.wikipedia.org/wiki/Autonomous_system_(Internet)">
        ///         autonomous system number
        ///     </a>
        ///     for the IP address. This value is only set when using the City or
        ///     Insights web service or the Enterprise database.
        /// </summary>
        [JsonProperty("autonomous_system_organization")]
        public string? AutonomousSystemOrganization { get; internal set; }

        /// <summary>
        ///     The connection type of the IP address. This value is only set when
        ///     using the Enterprise database.
        /// </summary>
        [JsonProperty("connection_type")]
        public string? ConnectionType { get; internal set; }

        /// <summary>
        ///     The second level domain associated with the IP address. This will
        ///     be something like "example.com" or "example.co.uk", not
        ///     "foo.example.com". This value is only set when using the City or
        ///     Insights web service or the Enterprise database.
        /// </summary>
        [JsonProperty("domain")]
        public string? Domain { get; internal set; }

        /// <summary>
        ///     The IP address that the data in the model is for. If you
        ///     performed a "me" lookup against the web service, this will be the
        ///     externally routable IP address for the system the code is running
        ///     on. If the system is behind a NAT, this may differ from the IP
        ///     address locally assigned to it.
        /// </summary>
        [JsonProperty("ip_address")]
        public string? IPAddress { get; internal set; }

        /// <summary>
        ///     This is true if the IP address belongs to any sort of anonymous
        ///     network. This value is only available from GeoIP2 Precision
        ///     Insights.
        /// </summary>
        [JsonProperty("is_anonymous")]
        public bool IsAnonymous { get; internal set; }

        /// <summary>
        ///     This is true if the IP is an anonymous proxy. See
        ///     <a href="https://dev.maxmind.com/faq/geoip#anonproxy">
        ///         MaxMind's GeoIP
        ///         FAQ
        ///     </a>
        /// </summary>
        [JsonProperty("is_anonymous_proxy")]
        [Obsolete("Use our GeoIP2 Anonymous IP database instead.")]
        public bool IsAnonymousProxy { get; internal set; }

        /// <summary>
        ///     This is true if the IP address is registered to an anonymous
        ///     VPN provider.
        ///     This value is only available from GeoIP2 Precision Insights.
        /// </summary>
        /// <remarks>
        ///     If a VPN provider does not register subnets under names
        ///     associated with them, we will likely only flag their IP ranges
        ///     using the IsHostingProvider property.
        /// </remarks>
        [JsonProperty("is_anonymous_vpn")]
        public bool IsAnonymousVpn { get; internal set; }

        /// <summary>
        ///     This is true if the IP address belongs to a hosting or VPN
        ///     provider (see description of IsAnonymousVpn property).
        ///     This value is only available from GeoIP2 Precision Insights.
        /// </summary>
        [JsonProperty("is_hosting_provider")]
        public bool IsHostingProvider { get; internal set; }

        /// <summary>
        ///     True if MaxMind believes this IP address to be a legitimate
        ///     proxy, such as an internal VPN used by a corporation. This is
        ///     only available in the GeoIP2 Enterprise database.
        /// </summary>
        [JsonProperty("is_legitimate_proxy")]
        public bool IsLegitimateProxy { get; internal set; }

        /// <summary>
        ///     This is true if the IP address belongs to a public proxy.
        ///     This value is only available from GeoIP2 Precision Insights.
        /// </summary>
        [JsonProperty("is_public_proxy")]
        public bool IsPublicProxy { get; internal set; }

        /// <summary>
        ///     This is true if the IP address is on a suspected anonymizing
        ///     network and belongs to a residential ISP. This value is
        ///     only available from GeoIP2 Precision Insights.
        /// </summary>
        [JsonProperty("is_residential_proxy")]
        public bool IsResidentialProxy { get; internal set; }

        /// <summary>
        ///     This is true if the IP belong to a satellite Internet provider.
        /// </summary>
        [JsonProperty("is_satellite_provider")]
        [Obsolete("Due to increased mobile usage, we have insufficient data to maintain this field.")]
        public bool IsSatelliteProvider { get; internal set; }

        /// <summary>
        ///     This is true if the IP address belongs to a Tor exit node.
        ///     This value is only available from GeoIP2 Precision Insights.
        /// </summary>
        [JsonProperty("is_tor_exit_node")]
        public bool IsTorExitNode { get; internal set; }

        /// <summary>
        ///     The name of the ISP associated with the IP address. This value
        ///     is only set when using the City or Insights web service or the
        ///     Enterprise database.
        /// </summary>
        [JsonProperty("isp")]
        public string? Isp { get; internal set; }

        /// <summary>
        ///     The network associated with the record. In particular, this is
        ///     the largest network where all of the fields besides
        ///     <c>IPAddress</c> have the same value.
        /// </summary>
        [JsonProperty("network")]
        public Network? Network { get; internal set; }

        /// <summary>
        ///     The name of the organization associated with the IP address. This
        ///     value is only set when using the City or Insights web service or the
        ///     Enterprise database.
        /// </summary>
        [JsonProperty("organization")]
        public string? Organization { get; internal set; }

        /// <summary>
        ///     An indicator of how static or dynamic an IP address is. The value
        ///     ranges from 0 to 99.99 with higher values meaning a greater static
        ///     association. For example, many IP addresses with a <c>UserType</c>
        ///     of <c>cellular</c> have a lifetime under one. Static Cable/DSL IPs
        ///     typically have a lifetime above thirty.
        /// </summary>
        /// <remark>
        ///     This indicator can be useful for deciding whether an IP address
        ///     represents the same user over time.
        /// </remark>
        [JsonProperty("static_ip_score")]
        public double? StaticIPScore { get; internal set; }

        /// <summary>
        ///     The estimated number of users sharing the IP/network during the past
        ///     24 hours. For IPv4, the count is for the individual IP. For IPv6, the
        ///     count is for the /64 network. This value is only available from
        ///     GeoIP2 Precision Insights.
        /// </summary>
        [JsonProperty("user_count")]
        public int? UserCount { get; internal set; }

        /// <summary>
        ///     The user type associated with the IP address. This can be one of
        ///     the following values:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>business</description>
        ///         </item>
        ///         <item>
        ///             <description>cafe</description>
        ///         </item>
        ///         <item>
        ///             <description>cellular</description>
        ///         </item>
        ///         <item>
        ///             <description>college</description>
        ///         </item>
        ///         <item>
        ///             <description>content_delivery_network</description>
        ///         </item>
        ///         <item>
        ///             <description>dialup</description>
        ///         </item>
        ///         <item>
        ///             <description>government</description>
        ///         </item>
        ///         <item>
        ///             <description>hosting</description>
        ///         </item>
        ///         <item>
        ///             <description>library</description>
        ///         </item>
        ///         <item>
        ///             <description>military</description>
        ///         </item>
        ///         <item>
        ///             <description>residential</description>
        ///         </item>
        ///         <item>
        ///             <description>router</description>
        ///         </item>
        ///         <item>
        ///             <description>school</description>
        ///         </item>
        ///         <item>
        ///             <description>search_engine_spider</description>
        ///         </item>
        ///         <item>
        ///             <description>traveler</description>
        ///         </item>
        ///     </list>
        ///     This value is only set when using the City or Insights web service
        ///     or the Enterprise database.
        /// </summary>
        [JsonProperty("user_type")]
        public string? UserType { get; internal set; }

        /// <summary>
        ///     Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{nameof(AutonomousSystemNumber)}: {AutonomousSystemNumber}, " +
                $"{nameof(AutonomousSystemOrganization)}: {AutonomousSystemOrganization}, " +
                $"{nameof(ConnectionType)}: {ConnectionType}, " +
                $"{nameof(Domain)}: {Domain}, " +
                $"{nameof(IPAddress)}: {IPAddress}, " +
                $"{nameof(IsAnonymous)}: {IsAnonymous}, " +
#pragma warning disable 618
                $"{nameof(IsAnonymousProxy)}: {IsAnonymousProxy}, " +
#pragma warning restore 618
                $"{nameof(IsAnonymousVpn)}: {IsAnonymousVpn}, " +
                $"{nameof(IsHostingProvider)}: {IsHostingProvider}, " +
                $"{nameof(IsLegitimateProxy)}: {IsLegitimateProxy}, " +
                $"{nameof(IsPublicProxy)}: {IsPublicProxy}, " +
#pragma warning disable 618
                $"{nameof(IsSatelliteProvider)}: {IsSatelliteProvider}, " +
#pragma warning restore 618
                $"{nameof(Isp)}: {Isp}, " +
                $"{nameof(Organization)}: {Organization}, " +
                $"{nameof(UserType)}: {UserType}";
        }
    }
}
