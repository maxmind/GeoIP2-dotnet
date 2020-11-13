using System.Text.Json.Serialization;

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Contains data about an error that occurred while calling the web service
    /// </summary>
    internal class WebServiceError
    {
        /// <summary>
        ///     Gets or sets the error.
        /// </summary>
        /// <value>
        ///     The error message returned by the service.
        /// </value>
        [JsonPropertyName("error")]
        public string? Error { get; set; }

        /// <summary>
        ///     Gets or sets the code.
        /// </summary>
        /// <value>
        ///     The error code returned by the service.
        /// </value>
        [JsonPropertyName("code")]
        public string? Code { get; set; }
    }
}