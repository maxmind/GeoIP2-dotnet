#region

using System;
using System.Net;

#endregion

namespace MaxMind.GeoIP2.Http
{
    internal class Response(Uri requestUri, HttpStatusCode statusCode, string? contentType, byte[] content)
    {
        internal HttpStatusCode StatusCode { get; } = statusCode;
        internal Uri RequestUri { get; } = requestUri;
        internal byte[] Content { get; } = content;
        internal string? ContentType { get; } = contentType;

        internal static object Create()
        {
            throw new NotImplementedException();
        }
    }
}