﻿#region

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
            [Parameter("autonomous_system_organization")] string autonomousSystemOrganization = null,
            [Parameter("connection_type")] string connectionType = null,
            string domain = null,
            [Inject("ip_address")] string ipAddress = null,
            [Parameter("is_anonymous_proxy")] bool isAnonymousProxy = false,
            [Parameter("is_legitimate_proxy")] bool isLegitimateProxy = false,
            [Parameter("is_satellite_provider")] bool isSatelliteProvider = false,
            string isp = null,
            string organization = null,
            [Parameter("user_type")] string userType = null)
        {
            // XXX - if we ever do a breaking release, this property should
            // be changes to long.
            AutonomousSystemNumber = (int?)autonomousSystemNumber;
            AutonomousSystemOrganization = autonomousSystemOrganization;
            ConnectionType = connectionType;
            Domain = domain;
            IPAddress = ipAddress;
#pragma warning disable 618
            IsAnonymousProxy = isAnonymousProxy;
            IsLegitimateProxy = isLegitimateProxy;
            IsSatelliteProvider = isSatelliteProvider;
#pragma warning restore 618
            Isp = isp;
            Organization = organization;
            UserType = userType;
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
        public int? AutonomousSystemNumber { get; internal set; }

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
        public string AutonomousSystemOrganization { get; internal set; }

        /// <summary>
        ///     The connection type of the IP address. This value is only set when
        ///     using the Enterprise database.
        /// </summary>
        [JsonProperty("connection_type")]
        public string ConnectionType { get; internal set; }

        /// <summary>
        ///     The second level domain associated with the IP address. This will
        ///     be something like "example.com" or "example.co.uk", not
        ///     "foo.example.com". This value is only set when using the City or
        ///     Insights web service or the Enterprise database.
        /// </summary>
        [JsonProperty("domain")]
        public string Domain { get; internal set; }

        /// <summary>
        ///     The IP address that the data in the model is for. If you
        ///     performed a "me" lookup against the web service, this will be the
        ///     externally routable IP address for the system the code is running
        ///     on. If the system is behind a NAT, this may differ from the IP
        ///     address locally assigned to it.
        /// </summary>
        [JsonProperty("ip_address")]
        public string IPAddress { get; internal set; }

        /// <summary>
        ///     This is true if the IP is an anonymous proxy. See
        ///     <a href="http://dev.maxmind.com/faq/geoip#anonproxy">
        ///         MaxMind's GeoIP
        ///         FAQ
        ///     </a>
        /// </summary>
        [JsonProperty("is_anonymous_proxy")]
        [Obsolete("Use our GeoIP2 Anonymous IP database instead.")]
        public bool IsAnonymousProxy { get; internal set; }

        /// <summary>
        ///     True if MaxMind believes this IP address to be a legitimate
        ///     proxy, such as an internal VPN used by a corporation.This is only
        ///     available in the GeoIP2 Enterprise database.
        /// </summary>
        [JsonProperty("is_legitimate_proxy")]
        public bool IsLegitimateProxy { get; internal set; }

        /// <summary>
        ///     This is true if the IP belong to a satellite Internet provider.
        /// </summary>
        [JsonProperty("is_satellite_provider")]
        [Obsolete("Due to increased mobile usage, we have insufficient data to maintain this field.")]
        public bool IsSatelliteProvider { get; internal set; }

        /// <summary>
        ///     The name of the ISP associated with the IP address. This value
        ///     is only set when using the City or Insights web service or the
        ///     Enterprise database.
        /// </summary>
        [JsonProperty("isp")]
        public string Isp { get; internal set; }

        /// <summary>
        ///     The name of the organization associated with the IP address. This
        ///     value is only set when using the City or Insights web service or the
        ///     Enterprise database.
        /// </summary>
        [JsonProperty("organization")]
        public string Organization { get; internal set; }

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
        public string UserType { get; internal set; }

        /// <summary>
        ///     Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return
                $"AutonomousSystemNumber: {AutonomousSystemNumber}, " +
                $"AutonomousSystemOrganization: {AutonomousSystemOrganization}, " +
                $"ConnectionType: {ConnectionType}, Domain: {Domain}, IPAddress: {IPAddress}," +
#pragma warning disable 0618
                    $" IsAnonymousProxy: {IsAnonymousProxy}, IsLegitimateProxy: {IsLegitimateProxy}, " +
                $"IsSatelliteProvider: {IsSatelliteProvider}, " +
#pragma warning restore 0618
                    $"Isp: {Isp}, Organization: {Organization}, UserType: {UserType}";
        }
    }
}