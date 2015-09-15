#region

using System;
using System.IO;
using System.Net;

#endregion

namespace MaxMind.GeoIP2.Http
{
    internal class Response : IDisposable
    {
        private bool _disposed;
        internal HttpStatusCode StatusCode { get; }
        internal Uri RequestUri { get; }
        internal Stream Stream { get; }
        internal string ContentType { get; }

        public Response(Uri requestUri, HttpStatusCode statusCode, string contentType, Stream stream)
        {
            RequestUri = requestUri;
            StatusCode = statusCode;
            ContentType = contentType;
            this.Stream = stream;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                Stream.Dispose();
            }

            _disposed = true;
        }
    }
}