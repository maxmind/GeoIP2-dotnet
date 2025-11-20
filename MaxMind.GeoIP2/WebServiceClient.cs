#region

using MaxMind.GeoIP2.Exceptions;
using MaxMind.GeoIP2.Http;
using MaxMind.GeoIP2.Model;
using MaxMind.GeoIP2.Responses;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

#endregion

namespace MaxMind.GeoIP2
{
    /// <summary>
    ///     <para>
    ///         This class provides a client API for all the GeoIP2 web services. The
    ///         services are Country, City Plus, and Insights. Each service returns a
    ///         different set of data about an IP address, with Country returning the
    ///         least data and Insights the most.
    ///     </para>
    ///     <para>
    ///         Each service is represented by a different model class which contains
    ///         data about the IP address.
    ///     </para>
    ///     <para>
    ///         If the service does not return a particular piece of data for an IP
    ///         address, the associated property is not populated.
    ///     </para>
    ///     <para>
    ///         The service may not return any information for an entire record, in
    ///         which case all of the properties for that model class will be empty.
    ///     </para>
    ///     <para>
    ///         Usage
    ///     </para>
    ///     <para>
    ///         The basic API for this class is the same for all of the services.
    ///         First you create a <c>WebServiceClient</c> with your MaxMind
    ///         <c>accountId</c> and <c>licenseKey</c>, then you call the method
    ///         corresponding to a specific service, passing it the IP address you want
    ///         to look up.
    ///     </para>
    ///     <para>
    ///         If the request succeeds, the method call will return a model class for
    ///         the service you called. This model in turn contains multiple record
    ///         classes, each of which represents part of the data returned.
    ///     </para>
    ///     <para>
    ///         If the request fails, the client class throws an exception.
    ///     </para>
    ///     <para>
    ///         Exceptions
    ///     </para>
    ///     <para>
    ///         For details on the possible errors returned by the web service itself,
    ///         see <a href="https://dev.maxmind.com/geoip/docs/web-services?lang=en">the
    ///         GeoIP2 web service documentation</a>.
    ///     </para>
    /// </summary>
    public class WebServiceClient : IGeoIP2WebServicesClient, IDisposable
    {
        private static readonly string Version =
            typeof(WebServiceClient).GetTypeInfo().Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? "unknown";

        private readonly string _host;
        private readonly IReadOnlyList<string> _locales;
        private readonly Client _client;
#if NETSTANDARD2_0 || NETSTANDARD2_1
        private readonly ISyncClient _syncClient;
#endif
        private bool _disposed;
        private readonly bool _disableHttps;
        private readonly JsonSerializerOptions _jsonOptions;

        private static ProductInfoHeaderValue UserAgent => new("GeoIP2-dotnet", Version);

        /// <summary>
        ///     Initializes a new instance of the <see cref="WebServiceClient" /> class.
        /// </summary>
        /// <param name="httpClient">Injected HttpClient.</param>
        /// <param name="options">Injected Options.</param>
        [CLSCompliant(false)]
        public WebServiceClient(
            HttpClient httpClient,
            IOptions<WebServiceClientOptions> options
        ) : this(
            options.Value.AccountId,
            options.Value.LicenseKey,
            options.Value.Locales,
            options.Value.Host,
            options.Value.Timeout,
            options.Value.DisableHttps,
            httpClient)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WebServiceClient" /> class.
        /// </summary>
        /// <param name="accountId">Your MaxMind account ID.</param>
        /// <param name="licenseKey">Your MaxMind license key.</param>
        /// <param name="locales">List of locale codes to use in name
        ///     property from most preferred to least preferred.</param>
        /// <param name="host">The host to use when accessing the service. Set this to
        ///     "geolite.info" to use the GeoLite2 web service instead of GeoIP2.
        ///     Set this to "sandbox.maxmind.com" to use the Sandbox GeoIP2 web service
        ///     instead of the production GeoIP2 web service. The sandbox allows you to
        ///     experiment with the API without affecting your production data.</param>
        /// <param name="timeout">Timeout in milliseconds for connection to
        ///     web service. The default is 3000.</param>
        /// <param name="disableHttps">Use HTTP instead of HTTPS. Note that MaxMind
        ///     servers require HTTPS.</param>
        /// <param name="httpMessageHandler">The <c>HttpMessageHandler</c> to
        ///     use when creating the <c>HttpClient</c>. The handler will be
        ///     disposed.</param>
        public WebServiceClient(
            int accountId,
            string licenseKey,
            IEnumerable<string>? locales = null,
            string host = "geoip.maxmind.com",
            int timeout = 3000,
            bool disableHttps = false,
            HttpMessageHandler? httpMessageHandler = null
        ) : this(
            accountId,
            licenseKey,
            locales,
            host,
            timeout,
            disableHttps,
            new HttpClient(httpMessageHandler ?? new HttpClientHandler(), true))
        {
        }

        internal WebServiceClient(
             int accountId,
             string licenseKey,
             IEnumerable<string>? locales,
             string host,
             int timeout,
             bool disableHttps,
             HttpClient httpClient
         )
        {
            var auth = EncodedAuth(accountId, licenseKey);
            _host = host;
            _locales = locales == null ? ["en"] : [.. locales];
            _client = new Client(auth, timeout, UserAgent, httpClient);
            _disableHttps = disableHttps;
            _jsonOptions = new JsonSerializerOptions();
            _jsonOptions.Converters.Add(new NetworkConverter());
#if NETSTANDARD2_0 || NETSTANDARD2_1
            _syncClient = new SyncClient(auth, timeout, UserAgent);
#endif
        }

        /// <summary>
        ///     Asynchronously query the Country web service for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>Task that produces an object modeling the Country response</returns>
        public async Task<CountryResponse> CountryAsync(string ipAddress)
        {
            return await CountryAsync(ParseIP(ipAddress)).ConfigureAwait(false);
        }

        /// <summary>
        ///     Asynchronously query the Country web service for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>Task that produces an object modeling the Country response</returns>
        public async Task<CountryResponse> CountryAsync(IPAddress ipAddress)
        {
            return await ExecuteAsync<CountryResponse>("country", ipAddress).ConfigureAwait(false);
        }

        /// <summary>
        ///     Asynchronously query the Country web service for the requesting IP address.
        /// </summary>
        /// <returns>Task that produces an object modeling the Country response</returns>
        public async Task<CountryResponse> CountryAsync()
        {
            return await ExecuteAsync<CountryResponse>("country", null).ConfigureAwait(false);
        }

        /// <summary>
        ///     Asynchronously query the City Plus web service for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>Task that produces an object modeling the City Plus response</returns>
        public async Task<CityResponse> CityAsync(string ipAddress)
        {
            return await CityAsync(ParseIP(ipAddress)).ConfigureAwait(false);
        }

        /// <summary>
        ///     Asynchronously query the City Plus web service for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>Task that produces an object modeling the City Plus response</returns>
        public async Task<CityResponse> CityAsync(IPAddress ipAddress)
        {
            return await ExecuteAsync<CityResponse>("city", ipAddress).ConfigureAwait(false);
        }

        /// <summary>
        ///     Asynchronously query the City Plus web service for the requesting IP address.
        /// </summary>
        /// <returns>Task that produces an object modeling the City Plus response</returns>
        public async Task<CityResponse> CityAsync()
        {
            return await ExecuteAsync<CityResponse>("city", null).ConfigureAwait(false);
        }

        /// <summary>
        ///     Asynchronously query the Insights web service for the specified IP
        ///     address. Please note that only the GeoIP2 web services support
        ///     Insights. The GeoLite2 web services do not support it.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>Task that produces an object modeling the Insights response</returns>
        public async Task<InsightsResponse> InsightsAsync(string ipAddress)
        {
            return await InsightsAsync(ParseIP(ipAddress)).ConfigureAwait(false);
        }

        /// <summary>
        ///     Asynchronously query the Insights web service for the specified IP
        ///     address. Please note that only the GeoIP2 web services support
        ///     Insights. The GeoLite2 web services do not support it.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>Task that produces an object modeling the Insights response</returns>
        public async Task<InsightsResponse> InsightsAsync(IPAddress ipAddress)
        {
            return await ExecuteAsync<InsightsResponse>("insights", ipAddress).ConfigureAwait(false);
        }

        /// <summary>
        ///     Asynchronously query the Insights web service for the requesting IP
        ///     address. Please note that only the GeoIP2 web services support
        ///     Insights. The GeoLite2 web services do not support it.
        /// </summary>
        /// <returns>Task that produces an object modeling the Insights response</returns>
        public async Task<InsightsResponse> InsightsAsync()
        {
            return await ExecuteAsync<InsightsResponse>("insights", null).ConfigureAwait(false);
        }

        /// <summary>
        ///     Returns an <see cref="CountryResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="CountryResponse" /></returns>
        public CountryResponse Country(IPAddress ipAddress)
        {
            return Execute<CountryResponse>("country", ipAddress);
        }

        /// <summary>
        ///     Returns an <see cref="CountryResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="CountryResponse" /></returns>
        public CountryResponse Country(string ipAddress)
        {
            return Country(ParseIP(ipAddress));
        }

        /// <summary>
        ///     Returns an <see cref="CountryResponse" /> for the requesting IP address.
        /// </summary>
        /// <returns>An <see cref="CountryResponse" /></returns>
        public CountryResponse Country()
        {
            return Execute<CountryResponse>("country", null);
        }

        /// <summary>
        ///     Returns an <see cref="CityResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="CityResponse" /></returns>
        public CityResponse City(IPAddress ipAddress)
        {
            return Execute<CityResponse>("city", ipAddress);
        }

        /// <summary>
        ///     Returns an <see cref="CityResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="CityResponse" /></returns>
        public CityResponse City(string ipAddress)
        {
            return City(ParseIP(ipAddress));
        }

        /// <summary>
        ///     Returns an <see cref="CityResponse" /> for the requesting IP address.
        /// </summary>
        /// <returns>An <see cref="CityResponse" /></returns>
        public CityResponse City()
        {
            return Execute<CityResponse>("city", null);
        }

        /// <summary>
        ///     Returns an <see cref="InsightsResponse" /> for the specified IP
        ///     address. Please note that only the GeoIP2 web services support
        ///     Insights. The GeoLite2 web services do not support it.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="InsightsResponse" /></returns>
        public InsightsResponse Insights(IPAddress ipAddress)
        {
            return Execute<InsightsResponse>("insights", ipAddress);
        }

        /// <summary>
        ///     Returns an <see cref="InsightsResponse" /> for the specified IP
        ///     address. Please note that only the GeoIP2 web services support
        ///     Insights. The GeoLite2 web services do not support it.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="InsightsResponse" /></returns>
        public InsightsResponse Insights(string ipAddress)
        {
            return Insights(ParseIP(ipAddress));
        }

        /// <summary>
        ///     Returns an <see cref="InsightsResponse" /> for the requesting IP
        ///     address. Please note that only the GeoIP2 web services support
        ///     Insights. The GeoLite2 web services do not support it.
        /// </summary>
        /// <returns>An <see cref="InsightsResponse" /></returns>
        public InsightsResponse Insights()
        {
            return Execute<InsightsResponse>("insights", null);
        }

        private static IPAddress ParseIP(string ipAddress)
        {
            IPAddress? ip = null;

            // The "ipAddress != null" is here for backwards compatibility with
            // pre-nullable-reference-types code that might possibly rely on the
            // undocumented feature of passing a null IP string.
            if (ipAddress != null && !IPAddress.TryParse(ipAddress, out ip))
            {
                throw new GeoIP2Exception($"The specified IP address was incorrectly formatted: {ipAddress}");
            }
            return ip!;
        }

        private T Execute<T>(string type, IPAddress? ipAddress)
            where T : AbstractCountryResponse
        {
            var uri = BuildUri(type, ipAddress);
#if NETSTANDARD2_0 || NETSTANDARD2_1
            var response = _syncClient.Get(uri);
#else
            var response = _client.Get(uri);
#endif
            return HandleResponse<T>(response);
        }

        private async Task<T> ExecuteAsync<T>(string type, IPAddress? ipAddress)
            where T : AbstractCountryResponse
        {
            var uri = BuildUri(type, ipAddress);
            var response = await _client.GetAsync(uri).ConfigureAwait(false);
            return HandleResponse<T>(response);
        }

        private Uri BuildUri(string type, IPAddress? ipAddress)
        {
            var endpoint = ipAddress?.ToString() ?? "me";
            var scheme = _disableHttps ? "http" : "https";
            return new Uri($"{scheme}://{_host}/geoip/v2.1/{type}/{endpoint}");
        }

        private static string EncodedAuth(int accountId, string licenseKey)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes($"{accountId}:{licenseKey}"));
        }

        private T HandleResponse<T>(Response response)
            where T : AbstractCountryResponse
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw CreateStatusException(response);
            }
            return CreateModel<T>(response);
        }

        private T CreateModel<T>(Response response)
            where T : AbstractCountryResponse
        {
            if (response.ContentType == null || !response.ContentType.Contains("json"))
            {
                throw new GeoIP2Exception(
                    $"Received a 200 response for {response.ContentType} but it does not appear to be JSON:\n");
            }

            if (response.Content == null || response.Content.Length == 0)
            {
                throw new HttpException(
                    $"Received a 200 response for {response.RequestUri} but there was no message body.",
                    HttpStatusCode.OK, response.RequestUri);
            }
            try
            {
                var model = JsonSerializer.Deserialize<T>(response.Content, _jsonOptions) ??
                    throw new HttpException(
                        $"Received a 200 response for {response.RequestUri} but there was no message body.",
                        HttpStatusCode.OK, response.RequestUri);
                model.SetLocales(_locales);
                return model;
            }
            catch (JsonException ex)
            {
                throw new GeoIP2Exception(
                    "Received a 200 response but not decode it as JSON", ex);
            }
        }

        private static Exception CreateStatusException(Response response)
        {
            var status = (int)response.StatusCode;
            switch (status)
            {
                case >= 400 and < 500:
                    return Create4xxException(response);
                case >= 500 and < 600:
                    return new HttpException(
                        $"Received a server ({status}) error for {response.RequestUri}",
                        response.StatusCode, response.RequestUri);
                default:
                    {
                        var errorMessage =
                            $"Received an unexpected response for {response.RequestUri} (status code: {status})";
                        return new HttpException(errorMessage, response.StatusCode, response.RequestUri);
                    }
            }
        }

        private static Exception Create4xxException(Response response)
        {
            if (response.Content.Length == 0)
            {
                return new HttpException(
                    $"Received a {response.StatusCode} error for {response.RequestUri} with no body",
                    response.StatusCode, response.RequestUri);
            }

            Exception? e = null;
            try
            {
                var webServiceError = JsonSerializer.Deserialize<WebServiceError>(response.Content);
                if (webServiceError != null)
                {
                    return CreateExceptionFromJson(response, webServiceError);
                }
            }
            catch (JsonException je)
            {
                e = je;
            }
            var content = Encoding.UTF8.GetString(response.Content);

            return new HttpException(
                $"Received a {response.StatusCode} error for {response.RequestUri} but it did not include the expected JSON body: {content}",
                response.StatusCode, response.RequestUri, e);
        }

        private static Exception CreateExceptionFromJson(Response response, WebServiceError webServiceError)
        {
            if (webServiceError.Code == null || webServiceError.Error == null)
            {
                var content = Encoding.UTF8.GetString(response.Content);

                return new HttpException(
                    $"Response contains JSON but does not specify code or error keys: {content}",
                    response.StatusCode,
                    response.RequestUri);
            }
            return webServiceError.Code switch
            {
                "IP_ADDRESS_NOT_FOUND" or "IP_ADDRESS_RESERVED" => new AddressNotFoundException(webServiceError.Error),
                "ACCOUNT_ID_REQUIRED" or "ACCOUNT_ID_UNKNOWN" or "AUTHORIZATION_INVALID" or "LICENSE_KEY_REQUIRED"
                    => new AuthenticationException(webServiceError.Error),
                "INSUFFICIENT_FUNDS" or "OUT_OF_QUERIES" => new OutOfQueriesException(webServiceError.Error),
                "PERMISSION_REQUIRED" => new PermissionRequiredException(webServiceError.Error),
                _ => new InvalidRequestException(webServiceError.Error, webServiceError.Code, response.RequestUri),
            };
        }

        /// <summary>
        ///     Release resources back to the operating system.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Release resources back to the operating system.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _client.Dispose();
            }

            _disposed = true;
        }
    }
}
