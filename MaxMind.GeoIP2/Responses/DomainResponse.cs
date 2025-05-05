#region

using MaxMind.Db;
using System.Text.Json.Serialization;

#endregion

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     This class represents the GeoIP2 Domain response.
    /// </summary>
    /// <remarks>
    /// Construct a DomainResponse model object.
    /// </remarks>
    /// <param name="domain"></param>
    /// <param name="ipAddress"></param>
    /// <param name="network"></param>
    [method: Constructor]
    public class DomainResponse(
        string? domain,
        [Inject("ip_address")] string? ipAddress,
        [Network] Network? network = null
        ) : AbstractResponse
    {
        /// <summary>
        /// Construct a DomainResponse model object.
        /// </summary>
        public DomainResponse() : this(null, null)
        {
        }

        /// <summary>
        ///     The second level domain associated with the IP address. This will
        ///     be something like "example.com" or "example.co.uk", not
        ///     "foo.example.com".
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("domain")]
        public string? Domain { get; internal set; } = domain;

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
