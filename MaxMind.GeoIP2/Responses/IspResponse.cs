#region

using MaxMind.Db;
using System;
using System.Text.Json.Serialization;

#endregion

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     This class represents the GeoIP2 ISP response.
    /// </summary>
    public class IspResponse : AbstractResponse
    {
        /// <summary>
        ///     Construct an IspResponse model.
        /// </summary>
        public IspResponse() : this(null, null, null, null, null, null, null)
        {
        }

        /// <summary>
        ///     Construct an IspResponse model.
        /// </summary>
        [Constructor]
        public IspResponse(
            [Parameter("autonomous_system_number")] long? autonomousSystemNumber,
            [Parameter("autonomous_system_organization")] string? autonomousSystemOrganization,
            string? isp,
            [Parameter("mobile_country_code")] string? mobileCountryCode,
            [Parameter("mobile_network_code")] string? mobileNetworkCode,
            string? organization,
            [Inject("ip_address")] string? ipAddress,
            [Network] Network? network = null
        )
        {
            AutonomousSystemNumber = autonomousSystemNumber;
            AutonomousSystemOrganization = autonomousSystemOrganization;
            Isp = isp;
            MobileCountryCode = mobileCountryCode;
            MobileNetworkCode = mobileNetworkCode;
            Organization = organization;
            IPAddress = ipAddress;
            Network = network;
        }

        /// <summary>
        ///     The
        ///     <a
        ///         href="https://en.wikipedia.org/wiki/Autonomous_system_(Internet)">
        ///         autonomous system number
        ///     </a>
        ///     associated with the IP address.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("autonomous_system_number")]
        public long? AutonomousSystemNumber { get; internal set; }

        /// <summary>
        ///     The organization associated with the registered
        ///     <a
        ///         href="https://en.wikipedia.org/wiki/Autonomous_system_(Internet)">
        ///         autonomous system number
        ///     </a>
        ///     for the IP address.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("autonomous_system_organization")]
        public string? AutonomousSystemOrganization { get; internal set; }

        /// <summary>
        ///     The name of the ISP associated with the IP address.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("isp")]
        public string? Isp { get; internal set; }

        /// <summary>
        ///     The <a href="https://en.wikipedia.org/wiki/Mobile_country_code">
        ///     mobile country code (MCC)</a> associated with the IP address and ISP.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("mobile_country_code")]
        public string? MobileCountryCode { get; internal set; }

        /// <summary>
        ///     The <a href="https://en.wikipedia.org/wiki/Mobile_country_code">
        ///     mobile network code (MNC)</a> associated with the IP address and ISP.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("mobile_network_code")]
        public string? MobileNetworkCode { get; internal set; }

        /// <summary>
        ///     The name of the organization associated with the IP address.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("organization")]
        public string? Organization { get; internal set; }

        /// <summary>
        ///     The IP address that the data in the model is for. If you
        ///     performed a "me" lookup against the web service, this will be the
        ///     externally routable IP address for the system the code is running
        ///     on. If the system is behind a NAT, this may differ from the IP
        ///     address locally assigned to it.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("ip_address")]
        public string? IPAddress { get; internal set; }

        /// <summary>
        ///     The network associated with the record. In particular, this is
        ///     the largest network where all of the fields besides
        ///     <c>IPAddress</c> have the same value.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("network")]
        public Network? Network { get; internal set; }
    }
}
