﻿using MaxMind.Db;
using System.Text.Json.Serialization;

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     This class represents the GeoLite2 ASN response.
    /// </summary>
    /// <remarks>
    ///     Construct an AsnResponse model.
    /// </remarks>
    [method: Constructor]
    public class AsnResponse(
        [Parameter("autonomous_system_number")] long? autonomousSystemNumber,
        [Parameter("autonomous_system_organization")] string? autonomousSystemOrganization,
        [Inject("ip_address")] string? ipAddress,
        [Network] Network? network = null
        ) : AbstractResponse
    {
        /// <summary>
        ///     Construct an AsnResponse model.
        /// </summary>
        public AsnResponse() : this(null, null, null)
        {
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
        public long? AutonomousSystemNumber { get; internal set; } = autonomousSystemNumber;

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
        public string? AutonomousSystemOrganization { get; internal set; } = autonomousSystemOrganization;

        /// <summary>
        ///     The IP address that the data in the model is for. If you
        ///     performed a "me" lookup against the web service, this will be the
        ///     externally routable IP address for the system the code is running
        ///     on. If the system is behind a NAT, this may differ from the IP
        ///     address locally assigned to it.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("ip_address")]
        public string? IPAddress { get; internal set; } = ipAddress;

        /// <summary>
        ///     The network associated with the record. In particular, this is
        ///     the largest network where all of the fields besides
        ///     <c>IPAddress</c> have the same value.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("network")]
        public Network? Network { get; internal set; } = network;
    }
}
