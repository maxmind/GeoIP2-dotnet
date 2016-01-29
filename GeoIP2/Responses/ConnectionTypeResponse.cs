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
        /// Construct ConnectionTypeResponse model
        /// </summary>
        public ConnectionTypeResponse()
        {
        }

        /// <summary>
        /// Construct ConnectionTypeResponse model
        /// </summary>
        [Constructor]
        public ConnectionTypeResponse(
            [Parameter("connection_type")] string connectionType,
            [Inject("ip_address")] string ipAddress
            )
        {
            ConnectionType = connectionType;
            IPAddress = ipAddress;
        }

        /// <summary>
        ///     The connection type of the IP address.
        /// </summary>
        [JsonProperty("connection_type")]
        public string ConnectionType { get; internal set; }

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