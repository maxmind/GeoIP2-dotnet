#region

using System;
#endregion

namespace MaxMind.GeoIP2.Exceptions
{
    /// <summary>
    ///     This class represents a non-specific error returned by MaxMind's GeoIP2 web
    ///     service. This occurs when the web service is up and responding to requests,
    ///     but the request sent was invalid in some way.
    /// </summary>
    public class InvalidRequestException : GeoIP2Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="InvalidRequestException" /> class.
        /// </summary>
        /// <param name="message">A message explaining the cause of the error.</param>
        /// <param name="code">The error code returned by the web service.</param>
        /// <param name="uri">The URL queried.</param>
        public InvalidRequestException(string message, string code, Uri uri)
            : base(message)
        {
            Code = code;
#pragma warning disable IDE0003 // Mono gets confused if 'this' is missing
            this.Uri = uri;
#pragma warning restore IDE0003
        }

        /// <summary>
        ///     The error code returned by the web service.
        /// </summary>
        public string Code { get; }

        /// <summary>
        ///     The URI queried by the web service.
        /// </summary>
        public Uri Uri { get; }
    }
}
