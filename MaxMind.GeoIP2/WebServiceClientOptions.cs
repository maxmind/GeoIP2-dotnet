using System.Collections.Generic;

namespace MaxMind.GeoIP2
{
    /// <summary>
    /// Options class for WebServiceClient.
    /// </summary>
    public class WebServiceClientOptions
    {
        /// <summary>
        /// Your MaxMind account ID.
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// Your MaxMind license key.
        /// </summary>
        public string LicenseKey { get; set; } = string.Empty;

        /// <summary>
        /// List of locale codes to use in name property from most preferred to least preferred.
        /// </summary>
        public IEnumerable<string>? Locales { get; set; }

        /// <summary>
        /// Timeout in milliseconds for connection to web service. The default is 3000.
        /// </summary>
        public int Timeout { get; set; } = 3000;

        /// <summary>
        /// The host to use when accessing the service.
        /// </summary>
        public string Host { get; set; } = "geoip.maxmind.com";
    }
}
