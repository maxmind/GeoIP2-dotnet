#region

using MaxMind.GeoIP2.Exceptions;
using MaxMind.GeoIP2.Http;
using MaxMind.GeoIP2.Model;
using MaxMind.GeoIP2.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using Microsoft.Extensions.Options;
#endif

#endregion

namespace MaxMind.GeoIP2
{
    /// <summary>
    ///     <para>
    ///         This class provides a client API for all the GeoIP2 Precision web service
    ///         end points. The end points are Country, City, and Insights. Each end point
    ///         returns a different set of data about an IP address, with Country returning
    ///         the least data and Insights the most.
    ///     </para>
    ///     <para>
    ///         Each web service end point is represented by a different model class
    ///         which contains data about the IP address.
    ///     </para>
    ///     <para>
    ///         If the web service does not return a particular piece of data for an IP
    ///         address, the associated property is not populated.
    ///     </para>
    ///     <para>
    ///         The web service may not return any information for an entire record, in which
    ///         case all of the properties for that model class will be empty.
    ///     </para>
    ///     <para>
    ///         Usage
    ///     </para>
    ///     <para>
    ///         The basic API for this class is the same for all of the web service end
    ///         points. First you create a web service object with your MaxMind
    ///         <c>accountId</c> and <c>licenseKey</c>, then you call the method corresponding
    ///         to a specific end point, passing it the IP address you want to look up.
    ///     </para>
    ///     <para>
    ///         If the request succeeds, the method call will return a model class for the
    ///         end point you called. This model in turn contains multiple record classes,
    ///         each of which represents part of the data returned by the web service.
    ///     </para>
    ///     <para>
    ///         If the request fails, the client class throws an exception.
    ///     </para>
    ///     <para>
    ///         Exceptions
    ///     </para>
    ///     <para>
    ///         For details on the possible errors returned by the web service itself, see
    ///         <a
    ///             href="https://dev.maxmind.com/geoip/geoip2/web-services">
    ///             the GeoIP2 web
    ///             service documentation
    ///         </a>
    ///         .
    ///     </para>
    /// </summary>
    public class WebServiceClient : IGeoIP2WebServicesClient, IDisposable
    {
        private static readonly string Version =
            ((AssemblyInformationalVersionAttribute)
                typeof(WebServiceClient).GetTypeInfo().Assembly.GetCustomAttribute(
                    typeof(AssemblyInformationalVersionAttribute))).InformationalVersion;

        private readonly string _host;
        private readonly IEnumerable<string> _locales;
        private readonly AsyncClient _asyncClient;
#if !NETSTANDARD1_4
        private readonly ISyncClient _syncClient;
#endif
        private bool _disposed;

        private static ProductInfoHeaderValue UserAgent => new ProductInfoHeaderValue("GeoIP2-dotnet", Version);

#if NETSTANDARD2_0 || NETSTANDARD2_1
        /// <summary>
        ///     Initializes a new instance of the <see cref="WebServiceClient" /> class.
        /// </summary>
        /// <param name="httpClient">Injected HttpClient.</param>
        /// <param name="options">Injected Options.</param>
        public WebServiceClient(
            HttpClient httpClient, 
            IOptionsMonitor<WebServiceClientOptions> options
        ) : this(
            options.CurrentValue.AccountId, 
            options.CurrentValue.LicenseKey, 
            options.CurrentValue.Locales, 
            options.CurrentValue.Host, 
            options.CurrentValue.Timeout,
            null,
            false,
            null,
            httpClient)
        {
        }
#endif

        /// <summary>
        ///     Initializes a new instance of the <see cref="WebServiceClient" /> class.
        /// </summary>
        /// <param name="accountId">Your MaxMind account ID.</param>
        /// <param name="licenseKey">Your MaxMind license key.</param>
        /// <param name="locales">List of locale codes to use in name property from most preferred to least preferred.</param>
        /// <param name="host">The host to use when accessing the service</param>
        /// <param name="timeout">Timeout in milliseconds for connection to web service. The default is 3000.</param>
        /// <param name="httpMessageHandler">The <c>HttpMessageHandler</c> to use when creating the <c>HttpClient</c>.</param>
        public WebServiceClient(
            int accountId,
            string licenseKey,
            IEnumerable<string>? locales = null,
            string host = "geoip.maxmind.com",
            int timeout = 3000,
            HttpMessageHandler? httpMessageHandler = null
        ) : this(accountId, licenseKey, locales, host, timeout, null, false)
        {
        }

        /// <summary>
        ///     Constructor for binary compatibility.
        /// </summary>
        public WebServiceClient(
            int accountId,
            string licenseKey,
            IEnumerable<string>? locales,
            string host,
            int timeout
        ) : this(accountId, licenseKey, locales, host, timeout, null)
        {
        }

        internal WebServiceClient(
            int accountId,
            string licenseKey,
            IEnumerable<string>? locales,
            string host,
            int timeout,
            HttpMessageHandler? httpMessageHandler,
            // This is a hack so that we can keep this internal while adding
            // httpMessageHandler to the public constructor. We can remove
            // this when we drop .NET 4.5 support and get rid of ISyncClient.
            bool fakeParam = false
#if !NETSTANDARD1_4
            , ISyncClient? syncWebRequest = null
#endif
            , HttpClient? httpClient = null
            )
        {
            var auth = EncodedAuth(accountId, licenseKey);
            _host = host;
            _locales = locales == null ? new List<string> { "en" } : new List<string>(locales);
#if !NETSTANDARD1_4
            _syncClient = syncWebRequest ?? new SyncClient(auth, timeout, UserAgent);
#endif            
            _asyncClient = new AsyncClient(auth, timeout, UserAgent, httpMessageHandler, httpClient);
        }

        /// <summary>
        ///     Asynchronously query the GeoIP2 Precision: Country web service for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>Task that produces an object modeling the Country response</returns>
        public async Task<CountryResponse> CountryAsync(string ipAddress)
        {
            return await CountryAsync(ParseIP(ipAddress)).ConfigureAwait(false);
        }

        /// <summary>
        ///     Asynchronously query the GeoIP2 Precision: Country web service for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>Task that produces an object modeling the Country response</returns>
        public async Task<CountryResponse> CountryAsync(IPAddress ipAddress)
        {
            return await ExecuteAsync<CountryResponse>("country", ipAddress).ConfigureAwait(false);
        }

        /// <summary>
        ///     Asynchronously query the GeoIP2 Precision: Country web service for the requesting IP address.
        /// </summary>
        /// <returns>Task that produces an object modeling the Country response</returns>
        public async Task<CountryResponse> CountryAsync()
        {
            return await ExecuteAsync<CountryResponse>("country", null).ConfigureAwait(false);
        }

        /// <summary>
        ///     Asynchronously query the GeoIP2 Precision: City web service for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>Task that produces an object modeling the City response</returns>
        public async Task<CityResponse> CityAsync(string ipAddress)
        {
            return await CityAsync(ParseIP(ipAddress)).ConfigureAwait(false);
        }

        /// <summary>
        ///     Asynchronously query the GeoIP2 Precision: City web service for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>Task that produces an object modeling the City response</returns>
        public async Task<CityResponse> CityAsync(IPAddress ipAddress)
        {
            return await ExecuteAsync<CityResponse>("city", ipAddress).ConfigureAwait(false);
        }

        /// <summary>
        ///     Asynchronously query the GeoIP2 Precision: City web service for the requesting IP address.
        /// </summary>
        /// <returns>Task that produces an object modeling the City response</returns>
        public async Task<CityResponse> CityAsync()
        {
            return await ExecuteAsync<CityResponse>("city", null).ConfigureAwait(false);
        }

        /// <summary>
        ///     Asynchronously query the GeoIP2 Precision: Insights web service for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>Task that produces an object modeling the Insights response</returns>
        public async Task<InsightsResponse> InsightsAsync(string ipAddress)
        {
            return await InsightsAsync(ParseIP(ipAddress)).ConfigureAwait(false);
        }

        /// <summary>
        ///     Asynchronously query the GeoIP2 Precision: Insights web service for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>Task that produces an object modeling the Insights response</returns>
        public async Task<InsightsResponse> InsightsAsync(IPAddress ipAddress)
        {
            return await ExecuteAsync<InsightsResponse>("insights", ipAddress).ConfigureAwait(false);
        }

        /// <summary>
        ///     Asynchronously query the GeoIP2 Precision: Insights web service for the requesting IP address.
        /// </summary>
        /// <returns>Task that produces an object modeling the Insights response</returns>
        public async Task<InsightsResponse> InsightsAsync()
        {
            return await ExecuteAsync<InsightsResponse>("insights", null).ConfigureAwait(false);
        }

#if !NETSTANDARD1_4

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
        ///     Returns an <see cref="InsightsResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="InsightsResponse" /></returns>
        public InsightsResponse Insights(IPAddress ipAddress)
        {
            return Execute<InsightsResponse>("insights", ipAddress);
        }

        /// <summary>
        ///     Returns an <see cref="InsightsResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="InsightsResponse" /></returns>
        public InsightsResponse Insights(string ipAddress)
        {
            return Insights(ParseIP(ipAddress));
        }

        /// <summary>
        ///     Returns an <see cref="InsightsResponse" /> for the requesting IP address.
        /// </summary>
        /// <returns>An <see cref="InsightsResponse" /></returns>
        public InsightsResponse Insights()
        {
            return Execute<InsightsResponse>("insights", null);
        }

#endif

        private static IPAddress ParseIP(string ipAddress)
        {
            IPAddress? ip = null;

            // The "ipAddress != null" is here for backwards compatibility with
            // pre-nullable-reference-types code that might possible rely on the
            // undocumented feature of passing a null IP string.
            if (ipAddress != null && !IPAddress.TryParse(ipAddress, out ip))
            {
                throw new GeoIP2Exception($"The specified IP address was incorrectly formatted: {ipAddress}");
            }
            return ip!;
        }

#if !NETSTANDARD1_4

        private T Execute<T>(string type, IPAddress? ipAddress)
            where T : AbstractCountryResponse, new()
        {
            var uri = BuildUri(type, ipAddress);
            using (var response = _syncClient.Get(uri))
            {
                return HandleResponse<T>(response);
            }
        }

#endif

        private async Task<T> ExecuteAsync<T>(string type, IPAddress? ipAddress)
            where T : AbstractCountryResponse, new()
        {
            var uri = BuildUri(type, ipAddress);
            using (var response = await _asyncClient.Get(uri).ConfigureAwait(false))
            {
                return HandleResponse<T>(response);
            }
        }

        private Uri BuildUri(string type, IPAddress? ipAddress)
        {
            var endpoint = ipAddress?.ToString() ?? "me";
            return new UriBuilder("https", _host, -1, $"/geoip/v2.1/{type}/{endpoint}").Uri;
        }

        private static string EncodedAuth(int accountId, string licenseKey)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes($"{accountId}:{licenseKey}"));
        }

        private T HandleResponse<T>(Response response)
            where T : AbstractCountryResponse, new()
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw CreateStatusException(response);
            }
            return CreateModel<T>(response);
        }

        private T CreateModel<T>(Response response)
            where T : AbstractCountryResponse, new()
        {
            if (response.ContentType == null || !response.ContentType.Contains("json"))
            {
                throw new GeoIP2Exception(
                    $"Received a 200 response for {response.ContentType} but it does not appear to be JSON:\n");
            }

            if (response.Stream == null)
            {
                throw new HttpException(
                    $"Received a 200 response for {response.RequestUri} but there was no message body.",
                    HttpStatusCode.OK, response.RequestUri);
            }

            using var sr = new StreamReader(response.Stream);
            try
            {
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    var settings = new JsonSerializerSettings();
                    settings.Converters.Add(new NetworkConverter());
                    var serializer = JsonSerializer.Create(settings);
                    var model = serializer.Deserialize<T>(reader);
                    if (model == null)
                    {
                        throw new HttpException(
                            $"Received a 200 response for {response.RequestUri} but there was no message body.",
                            HttpStatusCode.OK, response.RequestUri);
                    }
                    model.SetLocales(_locales);
                    return model;
                }
            }
            catch (JsonReaderException ex)
            {
                throw new GeoIP2Exception(
                    "Received a 200 response but not decode it as JSON", ex);
            }
        }

        private static Exception CreateStatusException(Response response)
        {
            var status = (int)response.StatusCode;
            if (status >= 400 && status < 500)
            {
                return Create4xxException(response);
            }
            if (status >= 500 && status < 600)
            {
                return new HttpException(
                    $"Received a server ({status}) error for {response.RequestUri}",
                    response.StatusCode, response.RequestUri);
            }

            var errorMessage =
                $"Received an unexpected response for {response.RequestUri} (status code: {status})";
            return new HttpException(errorMessage, response.StatusCode, response.RequestUri);
        }

        private static Exception Create4xxException(Response response)
        {
            string? content = null;

            if (response.Stream != null)
            {
                var reader = new StreamReader(response.Stream, Encoding.UTF8);
                content = reader.ReadToEnd();
            }

            if (string.IsNullOrEmpty(content))
            {
                return new HttpException(
                    $"Received a {response.StatusCode} error for {response.RequestUri} with no body",
                    response.StatusCode, response.RequestUri);
            }

            try
            {
                var webServiceError = JsonConvert.DeserializeObject<WebServiceError>(content!);

                return CreateExceptionFromJson(response, webServiceError, content!);
            }
            catch (JsonReaderException e)
            {
                return new HttpException(
                    $"Received a {response.StatusCode} error for {response.RequestUri} but it did not include the expected JSON body: {content}",
                    response.StatusCode, response.RequestUri, e);
            }
        }

        private static Exception CreateExceptionFromJson(Response response, WebServiceError webServiceError,
            string content)
        {
            if (webServiceError.Code == null || webServiceError.Error == null)
                return new HttpException(
                    $"Response contains JSON but does not specify code or error keys: {content}",
                    response.StatusCode,
                    response.RequestUri);
            switch (webServiceError.Code)
            {
                case "IP_ADDRESS_NOT_FOUND":
                case "IP_ADDRESS_RESERVED":
                    return new AddressNotFoundException(webServiceError.Error);

                case "ACCOUNT_ID_REQUIRED":
                case "ACCOUNT_ID_UNKNOWN":
                case "AUTHORIZATION_INVALID":
                case "LICENSE_KEY_REQUIRED":
                case "USER_ID_REQUIRED":
                case "USER_ID_UNKNOWN":
                    return new AuthenticationException(webServiceError.Error);

                case "INSUFFICIENT_FUNDS":
                case "OUT_OF_QUERIES":
                    return new OutOfQueriesException(webServiceError.Error);

                case "PERMISSION_REQUIRED":
                    return new PermissionRequiredException(webServiceError.Error);

                default:
                    return new InvalidRequestException(webServiceError.Error, webServiceError.Code, response.RequestUri);
            }
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
                _asyncClient.Dispose();
            }

            _disposed = true;
        }
    }
}
