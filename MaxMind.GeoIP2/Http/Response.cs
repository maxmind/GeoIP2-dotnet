#region

using System;
using System.Net;

#endregion

namespace MaxMind.GeoIP2.Http
{
    internal class Response
    {
        internal HttpStatusCode StatusCode { get; }
        internal Uri RequestUri { get; }
        internal byte[] Content { get; }
        internal string? ContentType { get; }

        public Response(Uri requestUri, HttpStatusCode statusCode, string? contentType, byte[] content)
        {
            RequestUri = requestUri;
            StatusCode = statusCode;
            ContentType = contentType;
#pragma warning disable IDE0003 // Mono gets confused if 'this' is missing
            this.Content = content;
#pragma warning restore IDE0003
        }
    }
}