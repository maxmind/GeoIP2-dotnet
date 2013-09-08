using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace MaxMind.GeoIP2.Exceptions
{
    /// <summary>
    /// This class represents an HTTP transport error. This is not an error returned
    /// by the web service itself.
    /// </summary>
    public class GeoIP2HttpException : ApplicationException
    {
        public HttpStatusCode HttpStatus { get; private set; }
        public string Url { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoIP2HttpException"/> class.
        /// </summary>
        /// <param name="message">A message describing the reason why the exception was thrown.</param>
        /// <param name="httpStatus">The HTTP status of the response that caused the exception.</param>
        /// <param name="url">The URL queried.</param>
        public GeoIP2HttpException(string message, HttpStatusCode httpStatus, string url) : base(message)
        {
            HttpStatus = httpStatus;
            Url = url;
        }
    }
}
