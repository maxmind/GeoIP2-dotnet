namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Contains data about an error that occurred while calling the web service
    /// </summary>
    public class WebServiceError
    {
        /// <summary>
        ///     Gets or sets the error.
        /// </summary>
        /// <value>
        ///     The error message returned by the service.
        /// </value>
        public string? Error { get; set; }

        /// <summary>
        ///     Gets or sets the code.
        /// </summary>
        /// <value>
        ///     The error code returned by the service.
        /// </value>
        public string? Code { get; set; }
    }
}