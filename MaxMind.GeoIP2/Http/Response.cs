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
            Content = content;
        }

        internal static object Create()
        {
            throw new NotImplementedException();
        }
    }
}