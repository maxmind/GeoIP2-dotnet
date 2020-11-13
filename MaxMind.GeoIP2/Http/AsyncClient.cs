#region

using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

#endregion

namespace MaxMind.GeoIP2.Http
{
    internal class AsyncClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private bool _disposed;

        public AsyncClient(
            string auth,
            int timeout,
            ProductInfoHeaderValue userAgent,
            HttpClient httpClient
            )
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.UserAgent.Add(userAgent);
            httpClient.Timeout = TimeSpan.FromMilliseconds(timeout);

            _httpClient = httpClient;
        }

        public async Task<Response> Get(Uri uri)
        {
            var response = await _httpClient.GetAsync(uri).ConfigureAwait(false);

            // Reading to a byte array isn't ideal, but changing this would require
            // more refactoring and probably introducing completely separate code
            // paths for async vs sync. Hopefully we can get rid of the sync code at
            // some point instead.
            var content = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            var contentType = response.Content.Headers.GetValues("Content-Type")?.FirstOrDefault();

            return new Response(uri, response.StatusCode, contentType, content);
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
                _httpClient.Dispose();
            }

            _disposed = true;
        }
    }
}
