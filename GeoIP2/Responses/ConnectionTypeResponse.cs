using Newtonsoft.Json;

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    /// This class represents the GeoIP2 Connection-Type response.
    /// </summary>
    public class ConnectionTypeResponse : AbstractResponse
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ConnectionTypeResponse() { }

        /// <summary>
        /// The connection type of the IP address.
        /// </summary>
        [JsonProperty("connection_type")]
        public string ConnectionType { get; internal set; }

        /// <summary>
        /// The IP address that the data in the model is for. If you
        /// performed a "me" lookup against the web service, this will be the
        /// externally routable IP address for the system the code is running
        /// on. If the system is behind a NAT, this may differ from the IP
        /// address locally assigned to it. This attribute is returned by all
        /// end points.
        /// </summary>
        [JsonProperty("ip_address")]
        public string IPAddress { get; internal set; }
    }
}
