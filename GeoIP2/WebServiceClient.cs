#region

using MaxMind.GeoIP2.Exceptions;
using MaxMind.GeoIP2.Model;
using MaxMind.GeoIP2.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
    ///         userID and licenseKey, then you call the method corresponding
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
    ///             href="http://dev.maxmind.com/geoip2/geoip/web-services">
    ///             the GeoIP2 web
    ///             service documentation
    ///         </a>
    ///         .
    ///     </para>
    /// </summary>
    public class WebServiceClient : IGeoIP2WebServicesClient
    {
        // XXX - check that this is right given changes in assembly versioning
        private static readonly Version Version = Assembly.GetExecutingAssembly().GetName().Version;

        private readonly string _host;
        private readonly string _licenseKey;
        private readonly List<string> _locales;
        private readonly int _timeout;
        private readonly int _userId;
        private readonly HttpClient _httpClient;

        private ProductInfoHeaderValue UserAgent => new ProductInfoHeaderValue("GeoIP2-dotnet", Version.ToString());

        /// <summary>
        ///     Initializes a new instance of the <see cref="WebServiceClient" /> class.
        /// </summary>
        /// <param name="userID">Your MaxMind user ID.</param>
        /// <param name="licenseKey">Your MaxMind license key.</param>
        /// <param name="baseUrl">The base url to use when accessing the service</param>
        /// <param name="timeout">Timeout in milliseconds for connection to web service. The default is 3000.</param>
        public WebServiceClient(int userID, string licenseKey, string baseUrl = "geoip.maxmind.com", int timeout = 3000)
            : this(userID, licenseKey, new List<string> { "en" }, baseUrl, timeout)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WebServiceClient" /> class.
        /// </summary>
        /// <param name="userID">The user unique identifier.</param>
        /// <param name="licenseKey">The license key.</param>
        /// <param name="locales">List of locale codes to use in name property from most preferred to least preferred.</param>
        /// <param name="host">The base url to use when accessing the service</param>
        /// <param name="timeout">Timeout in milliseconds for connection to web service. The default is 3000.</param>
        public WebServiceClient(int userID, string licenseKey, List<string> locales, string host = "geoip.maxmind.com",
            int timeout = 3000)
        {
            _userId = userID;
            _licenseKey = licenseKey;
            _locales = locales;
            _host = host;
            _timeout = timeout;

            _httpClient = new HttpClient()
            {
                DefaultRequestHeaders =
                {
                    Authorization = new AuthenticationHeaderValue("Basic", EncodedAuth()),
                    Accept = {new MediaTypeWithQualityHeaderValue("application/json")},
                    UserAgent = {UserAgent}
                },
                Timeout = TimeSpan.FromMilliseconds(timeout)
            };
        }

        public async Task<CountryResponse> CountryAsync(string ipAddress)
        {
            return await CountryAsync(ParseIP(ipAddress));
        }

        public async Task<CountryResponse> CountryAsync(IPAddress ipAddress)
        {
            return await ExecuteAsync<CountryResponse>("country", ipAddress);
        }

        public async Task<CityResponse> CityAsync(string ipAddress)
        {
            return await CityAsync(ParseIP(ipAddress));
        }

        public async Task<CityResponse> CityAsync(IPAddress ipAddress)
        {
            return await ExecuteAsync<CityResponse>("city", ipAddress);
        }

        public async Task<InsightsResponse> InsightsAsync(string ipAddress)
        {
            return await InsightsAsync(ParseIP(ipAddress));
        }

        public async Task<InsightsResponse> InsightsAsync(IPAddress ipAddress)
        {
            return await ExecuteAsync<InsightsResponse>("insights", ipAddress);
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

        private IPAddress ParseIP(string ipAddress)
        {
            IPAddress ip = null;
            if (ipAddress != null && !IPAddress.TryParse(ipAddress, out ip))
            {
                throw new GeoIP2Exception($"The specified IP address was incorrectly formatted: {ipAddress}");
            }
            return ip;
        }

        private T Execute<T>(string type, IPAddress ipAddress)
            where T : AbstractCountryResponse, new()
        {
            var uri = BuildUri(type, ipAddress);
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Headers["Authorization"] = $"Basic {EncodedAuth()}";
            request.Timeout = _timeout;

            request.UserAgent = UserAgent.ToString();

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                if (e.Status != WebExceptionStatus.ProtocolError)
                {
                    throw new HttpException(
                        $"Error received while making request: {e.Message}",
                        0, request.RequestUri, e);
                }
                response = (HttpWebResponse)e.Response;
            }
            return HandleResponse<T>(response.StatusCode, response.ContentType, response.GetResponseStream(), uri);
        }

        private async Task<T> ExecuteAsync<T>(string type, IPAddress ipAddress)
                        where T : AbstractCountryResponse, new()
        {
            var uri = BuildUri(type, ipAddress);
            var response = await _httpClient.GetAsync(uri).ConfigureAwait(false);

            using (
                var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false)
                )
            {
                var contentType = response.Content.Headers.GetValues("Content-Type")?.FirstOrDefault();
                return HandleResponse<T>(response.StatusCode, contentType, stream, uri);
            }
        }

        private Uri BuildUri(string type, IPAddress ipAddress)
        {
            var endpoint = ipAddress == null ? "me" : ipAddress.ToString();
            return new UriBuilder("https", _host, -1, $"/geoip/v2.1/{type}/{endpoint}").Uri;
        }

        private string EncodedAuth()
        {
            return Convert.ToBase64String(
                Encoding.ASCII.GetBytes(
                    $"{_userId}:{_licenseKey}"));
        }

        private T HandleResponse<T>(HttpStatusCode status, string contentType, Stream stream, Uri uri)
                        where T : AbstractCountryResponse, new()
        {
            if (status != HttpStatusCode.OK)
            {
                throw CreateStatusException(status, stream, uri);
            }
            return CreateModel<T>(contentType, stream, uri);
        }

        private T CreateModel<T>(string contentType, Stream stream, Uri uri)
            where T : AbstractCountryResponse, new()
        {
            if (contentType == null || !contentType.Contains("json"))
            {
                throw new GeoIP2Exception(
                    $"Received a 200 response for {contentType} but it does not appear to be JSON:\n");
            }

            if (stream == null)
            {
                throw new HttpException(
                    $"Received a 200 response for {uri} but there was no message body.",
                    HttpStatusCode.OK, uri);
            }

            var sr = new StreamReader(stream);
            try
            {
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    sr = null;
                    var serializer = new JsonSerializer();
                    var model = serializer.Deserialize<T>(reader);
                    model.SetLocales(_locales);
                    return model;
                }
            }
            catch (JsonSerializationException ex)
            {
                throw new GeoIP2Exception(
                    "Received a 200 response but not decode it as JSON", ex);
            }
            finally
            {
                sr?.Dispose();
            }
        }

        private Exception CreateStatusException(HttpStatusCode statusCode, Stream stream, Uri uri)
        {
            var status = (int)statusCode;
            if (status >= 400 && status < 500)
            {
                return Create4xxException(statusCode, stream, uri);
            }
            else if (status >= 500 && status < 600)
            {
                return new HttpException(
                    $"Received a server ({status}) error for {uri}",
                    statusCode, uri);
            }

            var errorMessage =
                $"Received an unexpected response for {uri} (status code: {status})";
            return new HttpException(errorMessage, statusCode, uri);
        }

        private Exception Create4xxException(HttpStatusCode statusCode, Stream stream, Uri uri)
        {
            string content = null;

            if (stream != null)
            {
                var reader = new StreamReader(stream, Encoding.UTF8);
                content = reader.ReadToEnd();
            }

            if (string.IsNullOrEmpty(content))
            {
                return new HttpException(
                    $"Received a {statusCode} error for {uri} with no body",
                    statusCode, uri);
            }

            try
            {
                var webServiceError = JsonConvert.DeserializeObject<WebServiceError>(content);

                return CreateExceptionFromJson(statusCode, webServiceError, content, uri);
            }
            catch (JsonSerializationException e)
            {
                return new HttpException(
                    $"Received a {statusCode} error for {uri} but it did not include the expected JSON body: {content}",
                    statusCode, uri, e);
            }
        }

        private static Exception CreateExceptionFromJson(HttpStatusCode statusCode, WebServiceError webServiceError, string content, Uri uri)
        {
            if (webServiceError.Code == null || webServiceError.Error == null)
                return new HttpException(
                    $"Response contains JSON but does not specify code or error keys: {content}",
                    statusCode,
                    uri);
            switch (webServiceError.Code)
            {
                case "IP_ADDRESS_NOT_FOUND":
                case "IP_ADDRESS_RESERVED":
                    return new AddressNotFoundException(webServiceError.Error);

                case "AUTHORIZATION_INVALID":
                case "LICENSE_KEY_REQUIRED":
                case "USER_ID_REQUIRED":
                    return new AuthenticationException(webServiceError.Error);

                case "OUT_OF_QUERIES":
                    return new OutOfQueriesException(webServiceError.Error);

                default:
                    return new InvalidRequestException(webServiceError.Error, webServiceError.Code, uri);
            }
        }
    }
}