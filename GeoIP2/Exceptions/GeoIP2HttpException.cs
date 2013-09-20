using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace MaxMind.GeoIP2.Exceptions
{
    /// <summary>
    /// This class represents an HTTP transport error. This is not an error returned
    /// by the web service itself. As such, it is a IOException instead of a
    /// GeoIP2Exception.
    /// </summary>
    public class GeoIP2HttpException : IOException
    {
        public HttpStatusCode HttpStatus { get; private set; }
        public Uri Uri { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoIP2HttpException"/> class.
        /// </summary>
        /// <param name="message">A message describing the reason why the exception was thrown.</param>
        /// <param name="httpStatus">The HTTP status of the response that caused the exception.</param>
        /// <param name="url">The URL queried.</param>
        public GeoIP2HttpException(string message, HttpStatusCode httpStatus, Uri uri) : base(message)
        {
            HttpStatus = httpStatus;
            Uri = uri;
        }
    }
}
