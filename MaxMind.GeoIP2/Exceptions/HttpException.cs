#region

using System;
using System.IO;
using System.Net;
#endregion

namespace MaxMind.GeoIP2.Exceptions
{
    /// <summary>
    ///     This class represents an HTTP transport error. This is not an error returned
    ///     by the web service itself. As such, it is a IOException instead of a
    ///     GeoIP2Exception.
    /// </summary>
    public class HttpException : IOException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="HttpException" /> class.
        /// </summary>
        /// <param name="message">A message describing the reason why the exception was thrown.</param>
        /// <param name="httpStatus">The HTTP status of the response that caused the exception.</param>
        /// <param name="uri">The URL queried.</param>
        public HttpException(string message, HttpStatusCode httpStatus, Uri uri)
            : base(message)
        {
            HttpStatus = httpStatus;
#pragma warning disable IDE0003 // Mono gets confused if 'this' is missing
            this.Uri = uri;
#pragma warning restore IDE0003
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HttpException" /> class.
        /// </summary>
        /// <param name="message">A message describing the reason why the exception was thrown.</param>
        /// <param name="httpStatus">The HTTP status of the response that caused the exception.</param>
        /// <param name="uri">The URL queried.</param>
        /// <param name="innerException">The underlying exception that caused this one.</param>
        public HttpException(string message, HttpStatusCode httpStatus, Uri uri, Exception innerException)
            : base(message, innerException)
        {
            HttpStatus = httpStatus;
#pragma warning disable IDE0003 // Mono gets confused without 'this'
            this.Uri = uri;
#pragma warning restore IDE0003
        }

        /// <summary>
        ///     The HTTP status code returned by the web service.
        /// </summary>
        public HttpStatusCode HttpStatus { get; }

        /// <summary>
        ///     The URI queried by the web service.
        /// </summary>
        public Uri Uri { get; }
    }
}
