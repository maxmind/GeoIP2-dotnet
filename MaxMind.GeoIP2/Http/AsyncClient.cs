#region

using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

#endregion

namespace MaxMind.GeoIP2.Http
{
    internal class AsyncClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private bool _disposed;
        private readonly HttpMessageHandler _httpMessageHandler;

        // As far as I can tell, this warning is a false positive. It is for the HttpClient instance.
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public AsyncClient(
            string auth,
            int timeout,
            ProductInfoHeaderValue userAgent,
            HttpMessageHandler? httpMessageHandler = null
            )
        {
            _httpMessageHandler = httpMessageHandler ?? new HttpClientHandler();
            try
            {
                _httpClient = new HttpClient(_httpMessageHandler)
                {
                    DefaultRequestHeaders =
                    {
                        Authorization = new AuthenticationHeaderValue("Basic", auth),
                        Accept = {new MediaTypeWithQualityHeaderValue("application/json")},
                        UserAgent = {userAgent}
                    },
                    Timeout = TimeSpan.FromMilliseconds(timeout)
                };
            }
            catch
            {
                _httpClient?.Dispose();
                _httpMessageHandler.Dispose();
                throw;
            }
        }

        public async Task<Response> Get(Uri uri)
        {
            var response = await _httpClient.GetAsync(uri).ConfigureAwait(false);
            var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            var contentType = response.Content.Headers.GetValues("Content-Type")?.FirstOrDefault();

            return new Response(uri, response.StatusCode, contentType, stream);
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
                _httpMessageHandler.Dispose();
            }

            _disposed = true;
        }
    }
}
