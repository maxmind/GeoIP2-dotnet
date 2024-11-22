#region

using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
#endregion

namespace MaxMind.GeoIP2.Exceptions
{
    /// <summary>
    ///     This class represents an HTTP transport error. This is not an error returned
    ///     by the web service itself. As such, it is a IOException instead of a
    ///     GeoIP2Exception.
    /// </summary>
    [Serializable]
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
            Uri = uri;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="HttpException" /> class.
        /// </summary>
        /// <param name="message">A message describing the reason why the exception was thrown.</param>
        /// <param name="httpStatus">The HTTP status of the response that caused the exception.</param>
        /// <param name="uri">The URL queried.</param>
        /// <param name="innerException">The underlying exception that caused this one.</param>
        public HttpException(string message, HttpStatusCode httpStatus, Uri uri, Exception? innerException)
            : base(message, innerException)
        {
            HttpStatus = httpStatus;
            Uri = uri;
        }

        /// <summary>
        ///     Constructor for deserialization.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
#if NET8_0_OR_GREATER
        [Obsolete(DiagnosticId = "SYSLIB0051")]
#endif
        protected HttpException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            HttpStatus = (HttpStatusCode)(info.GetValue("MaxMind.GeoIP2.Exceptions.HttpException.HttpStatus", typeof(HttpStatusCode))
                ?? throw new SerializationException("Unexcepted null HttpStatus value"));
            Uri = (Uri)(info.GetValue("MaxMind.GeoIP2.Exceptions.HttpException.Uri", typeof(Uri))
                ?? throw new SerializationException("Unexcepted null Uri value"));
        }

        /// <summary>
        ///     Method to serialize data.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
#if NET8_0_OR_GREATER
        [Obsolete(DiagnosticId = "SYSLIB0051")]
#endif
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("MaxMind.GeoIP2.Exceptions.HttpException.HttpStatus", HttpStatus, typeof(HttpStatusCode));
            info.AddValue("MaxMind.GeoIP2.Exceptions.HttpException.Uri", Uri, typeof(Uri));
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
