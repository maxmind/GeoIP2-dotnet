using MaxMind.Db;
using System.Text.Json.Serialization;

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     This record represents the GeoIP2 ISP response.
    /// </summary>
    public record IspResponse : AbstractResponse
    {
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
        [MapKey("autonomous_system_number")]
        public long? AutonomousSystemNumber { get; init; }

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
        [MapKey("autonomous_system_organization")]
        public string? AutonomousSystemOrganization { get; init; }

        /// <summary>
        ///     The name of the ISP associated with the IP address.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("isp")]
        [MapKey("isp")]
        public string? Isp { get; init; }

        /// <summary>
        ///     The <a href="https://en.wikipedia.org/wiki/Mobile_country_code">
        ///     mobile country code (MCC)</a> associated with the IP address and ISP.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("mobile_country_code")]
        [MapKey("mobile_country_code")]
        public string? MobileCountryCode { get; init; }

        /// <summary>
        ///     The <a href="https://en.wikipedia.org/wiki/Mobile_country_code">
        ///     mobile network code (MNC)</a> associated with the IP address and ISP.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("mobile_network_code")]
        [MapKey("mobile_network_code")]
        public string? MobileNetworkCode { get; init; }

        /// <summary>
        ///     The name of the organization associated with the IP address.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("organization")]
        [MapKey("organization")]
        public string? Organization { get; init; }

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
