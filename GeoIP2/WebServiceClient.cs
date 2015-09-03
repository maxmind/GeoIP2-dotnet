#region

using MaxMind.GeoIP2.Exceptions;
using MaxMind.GeoIP2.Model;
using MaxMind.GeoIP2.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

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
        private static readonly Version Version = Assembly.GetExecutingAssembly().GetName().Version;
        private readonly string _host;
        private readonly string _licenseKey;
        private readonly List<string> _locales;
        private readonly int _timeout;
        private readonly int _userId;

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
        }

        /// <summary>
        ///     Returns an <see cref="InsightsResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="InsightsResponse" /></returns>
        public InsightsResponse Insights(IPAddress ipAddress)
        {
            return Insights(ipAddress.ToString());
        }

        /// <summary>
        ///     Returns an <see cref="InsightsResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="InsightsResponse" /></returns>
        public InsightsResponse Insights(string ipAddress)
        {
            return Execute<InsightsResponse>("insights", ipAddress);
        }

        /// <summary>
        ///     Returns an <see cref="CountryResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="CountryResponse" /></returns>
        public CountryResponse Country(IPAddress ipAddress)
        {
            return Country(ipAddress.ToString());
        }

        /// <summary>
        ///     Returns an <see cref="CountryResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="CountryResponse" /></returns>
        public CountryResponse Country(string ipAddress)
        {
            return Execute<CountryResponse>("country", ipAddress);
        }

        /// <summary>
        ///     Returns an <see cref="CityResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="CityResponse" /></returns>
        public CityResponse City(IPAddress ipAddress)
        {
            return City(ipAddress.ToString());
        }

        /// <summary>
        ///     Returns an <see cref="CityResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="CityResponse" /></returns>
        public CityResponse City(string ipAddress)
        {
            return Execute<CityResponse>("city", ipAddress);
        }

        private WebRequest CreateRequest(string type, string ipAddress)
        {
            var request = (HttpWebRequest)WebRequest.Create($"https://{_host}/geoip/v2.1/{type}/{ipAddress}");
            var authInfo = Convert.ToBase64String(Encoding.Default.GetBytes($"{_userId}:{_licenseKey}"));
            request.Headers["Authorization"] = $"Basic {authInfo}";
            request.Timeout = _timeout;

            // XXX - Fix user agent to be valid
            request.UserAgent = $"GeoIP2 .NET Client {Version}";

            return request;
        }

        private T Execute<T>(string type, string ipAddress)
            where T : AbstractCountryResponse, new()
        {
            IPAddress ip;
            if (ipAddress != null && !IPAddress.TryParse(ipAddress, out ip))
                throw new GeoIP2Exception($"The specified IP address was incorrectly formatted: {ipAddress}");

            var request = CreateRequest(type, ipAddress ?? "me");

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                return CreateModel<T>(response);
            }
            catch (WebException e)
            {
                if (e.Status != WebExceptionStatus.ProtocolError)
                {
                    throw new HttpException(
                        $"Error received while making request: {e.Message}",
                        0, request.RequestUri, e);
                }
                throw CreateStatusException((HttpWebResponse)e.Response);
            }
        }

        private T CreateModel<T>(HttpWebResponse response)
            where T : AbstractCountryResponse, new()
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpException(
                    $"Received an unexpected {response.StatusCode} response for {response.ResponseUri}",
                    response.StatusCode, response.ResponseUri);
            }

            if (response.ContentType == null || !response.ContentType.Contains("json"))
                throw new GeoIP2Exception(
                    $"Received a 200 response for {response.ContentType} but it does not appear to be JSON:\n");

            var stream = response.GetResponseStream();
            if (response.ContentLength <= 0 || stream == null)
            {
                stream?.Dispose();
                throw new HttpException(
                    $"Received a 200 response for {response.ResponseUri} but there was no message body.",
                    response.StatusCode, response.ResponseUri);
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

        private Exception CreateStatusException(HttpWebResponse response)
        {
            var status = (int)response.StatusCode;
            if (status >= 400 && status < 500)
            {
                return Create4xxException(response);
            }
            else if (status >= 500 && status < 600)
            {
                return new HttpException(
                    $"Received a server ({(int)response.StatusCode}) error for {response.ResponseUri}",
                    response.StatusCode, response.ResponseUri);
            }

            var errorMessage =
                $"Received an unexpected response for {response.ResponseUri} (status code: {(int)response.StatusCode})";
            return new HttpException(errorMessage, response.StatusCode, response.ResponseUri);
        }

        private Exception Create4xxException(HttpWebResponse response)
        {
            string content = null;
            using (var stream = response.GetResponseStream())
            {
                if (stream != null)
                {
                    var reader = new StreamReader(stream, Encoding.UTF8);
                    content = reader.ReadToEnd();
                }
            }
            if (string.IsNullOrEmpty(content))
            {
                return new HttpException(
                    $"Received a {response.StatusCode} error for {response.ResponseUri} with no body",
                    response.StatusCode, response.ResponseUri);
            }

            try
            {
                var webServiceError = JsonConvert.DeserializeObject<WebServiceError>(content);

                return CreateExceptionFromJson(webServiceError, response, content);
            }
            catch (JsonSerializationException e)
            {
                return new HttpException(
                    $"Received a {response.StatusCode} error for {response.ResponseUri} but it did not include the expected JSON body: {content}",
                    response.StatusCode, response.ResponseUri, e);
            }
        }

        private static Exception CreateExceptionFromJson(WebServiceError webServiceError, HttpWebResponse response, string content)
        {
            if (webServiceError.Code == null || webServiceError.Error == null)
                return new HttpException(
                    $"Response contains JSON but does not specify code or error keys: {content}",
                    response.StatusCode,
                    response.ResponseUri);
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
                    return new InvalidRequestException(webServiceError.Error, webServiceError.Code, response.ResponseUri);
            }
        }
    }
}