#region

using MaxMind.Db;
using Newtonsoft.Json;

#endregion

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     This class represents the GeoIP2 Connection-Type response.
    /// </summary>
    public class ConnectionTypeResponse : AbstractResponse
    {
        /// <summary>
        ///     Construct ConnectionTypeResponse model
        /// </summary>
        public ConnectionTypeResponse()
        {
        }

        /// <summary>
        ///     Construct ConnectionTypeResponse model
        /// </summary>
        [Constructor]
        public ConnectionTypeResponse(
            [Parameter("connection_type")] string? connectionType,
            [Inject("ip_address")] string? ipAddress,
            [Network] Network? network = null
        )
        {
            ConnectionType = connectionType;
            IPAddress = ipAddress;
            Network = network;
        }

        /// <summary>
        ///     The connection type of the IP address.
        /// </summary>
        [JsonProperty("connection_type")]
        public string? ConnectionType { get; internal set; }

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
        ///     The network associated with the record. In particular, this is
        ///     the largest network where all of the fields besides
        ///     <c>IPAddress</c> have the same value.
        /// </summary>
        [JsonProperty("network")]
        public Network? Network { get; internal set; }
    }
}