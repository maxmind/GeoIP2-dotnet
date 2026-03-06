using MaxMind.Db;
using System.Text.Json.Serialization;

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     This class represents the GeoIP2 Connection-Type response.
    /// </summary>
    public record ConnectionTypeResponse : AbstractResponse
    {
        /// <summary>
        ///     The connection type may take the following values: "Dialup",
        ///     "Cable/DSL", "Corporate", "Cellular", and "Satellite". Additional
        ///     values may be added in the future.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("connection_type")]
        [MapKey("connection_type")]
        public string? ConnectionType { get; init; }

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
