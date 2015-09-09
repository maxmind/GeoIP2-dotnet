﻿#region

using System;
using Newtonsoft.Json;

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
        public Traits(
            int? autonomousSystemNumber = null,
            string autonomousSystemOrganization = null,
            string domain = null,
            string ipAddress = null,
            bool isAnonymousProxy = false,
            bool isSatelliteProvider = false,
            string isp = null,
            string organization = null,
            string userType = null)
        {
            AutonomousSystemNumber = autonomousSystemNumber;
            AutonomousSystemOrganization = autonomousSystemOrganization;
            Domain = domain;
            IPAddress = ipAddress;
#pragma warning disable 0618
            IsAnonymousProxy = isAnonymousProxy;
            IsSatelliteProvider = isSatelliteProvider;
#pragma warning restore 0618
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
        ///     This attribute is only available from the City and Insights web
        ///     service end points.
        /// </summary>
        [JsonProperty("autonomous_system_number")]
        public int? AutonomousSystemNumber { get; internal set; }

        /// <summary>
        ///     The organization associated with the registered
        ///     <a
        ///         href="http://en.wikipedia.org/wiki/Autonomous_system_(Internet)">
        ///         autonomous system number
        ///     </a>
        ///     for the IP address. This attribute
        ///     is only available from the City and Insights web service end points.
        /// </summary>
        [JsonProperty("autonomous_system_organization")]
        public string AutonomousSystemOrganization { get; internal set; }

        /// <summary>
        ///     The second level domain associated with the IP address. This will
        ///     be something like "example.com" or "example.co.uk", not
        ///     "foo.example.com". This attribute is only available from the
        ///     City and Insights web service end points.
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
        ///     This is true if the IP belong to a satellite internet provider.
        /// </summary>
        [JsonProperty("is_satellite_provider")]
        [Obsolete("Due to increased mobile usage, we have insufficient data to maintain this field.")]
        public bool IsSatelliteProvider { get; internal set; }

        /// <summary>
        ///     The name of the ISP associated with the IP address. This
        ///     attribute is only available from the City and Insights web
        ///     service end points.
        /// </summary>
        [JsonProperty("isp")]
        public string Isp { get; internal set; }

        /// <summary>
        ///     The name of the organization associated with the IP address. This
        ///     attribute is only available from the City and Insights web
        ///     service end points.
        /// </summary>
        [JsonProperty("organization")]
        public string Organization { get; internal set; }

        /// <summary>
        ///     The user type associated with the IP address. This can be one of
        ///     the following values:
        ///     business
        ///     cafe
        ///     cellular
        ///     college
        ///     content_delivery_network
        ///     dialup
        ///     government
        ///     hosting
        ///     library
        ///     military
        ///     residential
        ///     router
        ///     school
        ///     search_engine_spider
        ///     traveler
        ///     This attribute is only available from the Insights end point.
        /// </summary>
        [JsonProperty("user_type")]
        public string UserType { get; internal set; }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Traits ["
                   + (AutonomousSystemNumber != null ? "AutonomousSystemNumber=" + AutonomousSystemNumber + ", " : "")
                   +
                   (AutonomousSystemOrganization != null
                       ? "AutonomousSystemOrganization=" + AutonomousSystemOrganization + ", "
                       : "")
                   + (Domain != null ? "Domain=" + Domain + ", " : "")
                   + (IPAddress != null ? "IpAddress=" + IPAddress + ", " : "")
#pragma warning disable 0618
                   + "IsAnonymousProxy=" + IsAnonymousProxy
                   + ", IsSatelliteProvider=" + IsSatelliteProvider + ", "
#pragma warning restore 0618
                   + (Isp != null ? "Isp=" + Isp + ", " : "")
                   + (Organization != null ? "Organization=" + Organization + ", " : "")
                   + (UserType != null ? "UserType=" + UserType : "")
                   + "]";
        }
    }
}