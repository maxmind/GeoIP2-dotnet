using MaxMind.Db;
using System;
using System.Text.Json.Serialization;

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Contains data for the traits record associated with an IP address.
    /// </summary>
    public record Traits
    {
        /// <summary>
        ///     The
        ///     <a
        ///         href="https://en.wikipedia.org/wiki/Autonomous_system_(Internet)">
        ///         autonomous system number
        ///     </a>
        ///     associated with the IP address.
        ///     This value is only available from the City Plus and Insights web
        ///     services and the Enterprise database.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("autonomous_system_number")]
        [MapKey("autonomous_system_number")]
        public long? AutonomousSystemNumber { get; init; }

        /// <summary>
        ///     The organization associated with the registered
        ///     <a
        ///         href="https://en.wikipedia.org/wiki/Autonomous_system_(Internet)">
        ///         autonomous system number
        ///     </a>
        ///     for the IP address. This value is only available from the City
        ///     Plus and Insights web services and the Enterprise database.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("autonomous_system_organization")]
        [MapKey("autonomous_system_organization")]
        public string? AutonomousSystemOrganization { get; init; }

        /// <summary>
        ///     The connection type may take the following values: "Dialup",
        ///     "Cable/DSL", "Corporate", "Cellular", and "Satellite".
        ///     Additional values may be added in the future. This value is
        ///     only available from the City Plus and Insights web services
        ///     and the Enterprise database.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("connection_type")]
        [MapKey("connection_type")]
        public string? ConnectionType { get; init; }

        /// <summary>
        ///     The second level domain associated with the IP address. This will
        ///     be something like "example.com" or "example.co.uk", not
        ///     "foo.example.com". This value is only available from the City
        ///     Plus and Insights web services and the Enterprise database.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("domain")]
        [MapKey("domain")]
        public string? Domain { get; init; }

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
        ///     A risk score associated with the IP address, ranging from 0.01 to 99.
        ///     A higher score indicates a higher risk. Please note that the IP risk
        ///     score provided in GeoIP products and services is more static than the
        ///     IP risk score provided in minFraud and is not responsive to traffic on
        ///     your network. If you need realtime IP risk scoring based on behavioral
        ///     signals on your own network, please use minFraud. This is available
        ///     from the GeoIP2 Insights web service.
        ///     <para>
        ///     We do not provide an IP risk snapshot for low-risk networks. If this
        ///     field is not populated, we either do not have signals for the network
        ///     or the signals we have show that the network is low-risk. If you would
        ///     like to get signals for low-risk networks, please use the minFraud web
        ///     services.
        ///     </para>
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("ip_risk_snapshot")]
        [MapKey("ip_risk_snapshot")]
        public double? IpRiskSnapshot { get; init; }

        /// <summary>
        ///     This is true if the IP address belongs to any sort of anonymous
        ///     network. This value is only available from the GeoIP2 Insights
        ///     web service.
        /// </summary>
        [Obsolete("Please use the Anonymizer object on the response instead.")]
        [JsonInclude]
        [JsonPropertyName("is_anonymous")]
        [MapKey("is_anonymous")]
        public bool IsAnonymous { get; init; }

        /// <summary>
        ///     This is true if the IP is an anonymous proxy.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("is_anonymous_proxy")]
        [Obsolete("Use our GeoIP2 Anonymous IP database instead.")]
        [MapKey("is_anonymous_proxy")]
        public bool IsAnonymousProxy { get; init; }

        /// <summary>
        ///     This is true if the IP address belongs to an <a
        ///     href="https://en.wikipedia.org/wiki/Anycast">anycast network</a>.
        ///     This is not available from GeoLite databases or web services.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("is_anycast")]
        [MapKey("is_anycast")]
        public bool IsAnycast { get; init; }

        /// <summary>
        ///     This is true if the IP address is registered to an anonymous
        ///     VPN provider.
        ///     This value is only available from the GeoIP2 Insights web
        ///     service.
        /// </summary>
        /// <remarks>
        ///     If a VPN provider does not register subnets under names
        ///     associated with them, we will likely only flag their IP ranges
        ///     using the IsHostingProvider property.
        /// </remarks>
        [Obsolete("Please use the Anonymizer object on the response instead.")]
        [JsonInclude]
        [JsonPropertyName("is_anonymous_vpn")]
        [MapKey("is_anonymous_vpn")]
        public bool IsAnonymousVpn { get; init; }

        /// <summary>
        ///     This is true if the IP address belongs to a hosting or VPN
        ///     provider (see description of IsAnonymousVpn property).
        ///     This value is only available from the GeoIP2 Insights web
        ///     service.
        /// </summary>
        [Obsolete("Please use the Anonymizer object on the response instead.")]
        [JsonInclude]
        [JsonPropertyName("is_hosting_provider")]
        [MapKey("is_hosting_provider")]
        public bool IsHostingProvider { get; init; }

        /// <summary>
        ///     True if MaxMind believes this IP address to be a legitimate
        ///     proxy, such as an internal VPN used by a corporation. This is
        ///     only available in the GeoIP2 Enterprise database.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("is_legitimate_proxy")]
        [MapKey("is_legitimate_proxy")]
        public bool IsLegitimateProxy { get; init; }

        /// <summary>
        ///     This is true if the IP address belongs to a public proxy.
        ///     This value is only available from the GeoIP2 Insights web
        ///     service.
        /// </summary>
        [Obsolete("Please use the Anonymizer object on the response instead.")]
        [JsonInclude]
        [JsonPropertyName("is_public_proxy")]
        [MapKey("is_public_proxy")]
        public bool IsPublicProxy { get; init; }

        /// <summary>
        ///     This is true if the IP address is on a suspected anonymizing
        ///     network and belongs to a residential ISP. This value is
        ///     only available from the GeoIP2 Insights web service.
        /// </summary>
        [Obsolete("Please use the Anonymizer object on the response instead.")]
        [JsonInclude]
        [JsonPropertyName("is_residential_proxy")]
        [MapKey("is_residential_proxy")]
        public bool IsResidentialProxy { get; init; }

        /// <summary>
        ///     This is true if the IP belong to a satellite Internet provider.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("is_satellite_provider")]
        [Obsolete("Due to increased mobile usage, we have insufficient data to maintain this field.")]
        [MapKey("is_satellite_provider")]
        public bool IsSatelliteProvider { get; init; }

        /// <summary>
        ///     This is true if the IP address belongs to a Tor exit node.
        ///     This value is only available from the GeoIP2 Insights web
        ///     service.
        /// </summary>
        [Obsolete("Please use the Anonymizer object on the response instead.")]
        [JsonInclude]
        [JsonPropertyName("is_tor_exit_node")]
        [MapKey("is_tor_exit_node")]
        public bool IsTorExitNode { get; init; }

        /// <summary>
        ///     The name of the ISP associated with the IP address. This value
        ///     is available from the City Plus and Insights web services and the
        ///     Enterprise database.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("isp")]
        [MapKey("isp")]
        public string? Isp { get; init; }

        /// <summary>
        ///     The <a href="https://en.wikipedia.org/wiki/Mobile_country_code">
        ///     mobile country code (MCC)</a> associated with the IP address and ISP.
        ///     This value is available from the City Plus and Insights web services
        ///     and the GeoIP2 Enterprise database.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("mobile_country_code")]
        [MapKey("mobile_country_code")]
        public string? MobileCountryCode { get; init; }

        /// <summary>
        ///     The <a href="https://en.wikipedia.org/wiki/Mobile_country_code">
        ///     mobile network code (MNC)</a> associated with the IP address and ISP.
        ///     This value is available from the City Plus and Insights web services
        ///     and the GeoIP2 Enterprise database.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("mobile_network_code")]
        [MapKey("mobile_network_code")]
        public string? MobileNetworkCode { get; init; }

        /// <summary>
        ///     The network associated with the record. In particular, this is
        ///     the largest network where all of the fields besides
        ///     <c>IPAddress</c> have the same value.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("network")]
        [Network]
        public Network? Network { get; init; }

        /// <summary>
        ///     The name of the organization associated with the IP address. This
        ///     value is only available from the City Plus and Insights web services
        ///     and the Enterprise database.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("organization")]
        [MapKey("organization")]
        public string? Organization { get; init; }

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
        [JsonInclude]
        [JsonPropertyName("static_ip_score")]
        [MapKey("static_ip_score")]
        public double? StaticIPScore { get; init; }

        /// <summary>
        ///     The estimated number of users sharing the IP/network during the past
        ///     24 hours. For IPv4, the count is for the individual IP. For IPv6, the
        ///     count is for the /64 network. This value is only available from
        ///     the GeoIP2 Insights web service.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("user_count")]
        [MapKey("user_count")]
        public int? UserCount { get; init; }

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
        ///             <description>consumer_privacy_network</description>
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
        ///     This value is only available from the City Plus and Insights web
        ///     services and the Enterprise database.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("user_type")]
        [MapKey("user_type")]
        public string? UserType { get; init; }
    }
}
