﻿#region

using MaxMind.Db;
using Newtonsoft.Json;

#endregion

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     This class represents the GeoIP2 Domain response.
    /// </summary>
    public class DomainResponse : AbstractResponse
    {
        /// <summary>
        /// Construct a DomainResponse model object.
        /// </summary>
        public DomainResponse() : this(null, null)
        {
        }

        /// <summary>
        /// Construct a DomainResponse model object.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="ipAddress"></param>
        [Constructor]
        public DomainResponse(
            string domain,
            [Inject("ip_address")] string ipAddress)
        {
            Domain = domain;
            IPAddress = ipAddress;
        }

        /// <summary>
        ///     The second level domain associated with the IP address. This will
        ///     be something like "example.com" or "example.co.uk", not
        ///     "foo.example.com".
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
    }
}