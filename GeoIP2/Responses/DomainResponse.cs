using Newtonsoft.Json;

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    /// This class represents the GeoIP2 Domain response.
    /// </summary>
    public class DomainResponse : AbstractResponse
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DomainResponse() { }

        /// <summary>
        /// The second level domain associated with the IP address. This will
        /// be something like "example.com" or "example.co.uk", not
        /// "foo.example.com". This attribute is only available from the
        /// City/ISP/Org and Omni end points.
        /// </summary>
        [JsonProperty("domain")]
        public string Domain { get; internal set; }

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
