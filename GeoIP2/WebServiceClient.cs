using System.Runtime.InteropServices.ComTypes;
using MaxMind.GeoIP2.Model;
using MaxMind.GeoIP2.Responses;
using RestSharp;
using RestSharp.Deserializers;

namespace MaxMind.GeoIP2
{
    /// <summary>
    /// <para>
    /// This class provides a client API for all the GeoIP2 web service's end points.
    /// The end points are Country, City, City/ISP/Org, and Omni. Each end point
    /// returns a different set of data about an IP address, with Country returning
    /// the least data and Omni the most.
    /// </para>
    /// 
    /// <para>
    /// Each web service end point is represented by a different model class 
    /// which contains data about the IP address.
    /// </para>
    /// 
    /// <para>
    /// If the web service does not return a particular piece of data for an IP
    /// address, the associated property is not populated.
    /// </para>
    /// 
    /// <para>
    /// The web service may not return any information for an entire record, in which
    /// case all of the properties for that model class will be empty.
    /// </para>
    /// 
    /// <para>
    /// Usage
    /// </para>
    /// 
    /// <para>
    /// The basic API for this class is the same for all of the web service end
    /// points. First you create a web service object with your MaxMind
    /// userId and licenseKey, then you call the method corresponding
    /// to a specific end point, passing it the IP address you want to look up.
    /// </para>
    /// 
    /// <para>
    /// If the request succeeds, the method call will return a model class for the
    /// end point you called. This model in turn contains multiple record classes,
    /// each of which represents part of the data returned by the web service.
    /// </para>
    /// 
    /// <para>
    /// If the request fails, the client class throws an exception.
    /// </para>
    /// 
    /// <para>
    /// Exceptions
    /// </para>
    /// 
    /// <para>
    /// For details on the possible errors returned by the web service itself, see <a
    /// href="http://dev.maxmind.com/geoip2/geoip/web-services">the GeoIP2 web
    /// service documentation</a>.
    /// </para>
    /// 
    /// </summary>
    public class WebServiceClient
    {
        private readonly int _userId;
        private readonly string _licenseKey;
        private const string BASE_URL = "https://geoip.maxmind.com/geoip/v2.0";

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServiceClient"/> class.
        /// </summary>
        /// <param name="userId">Your MaxMind user ID.</param>
        /// <param name="licenseKey">Your MaxMind license key.</param>
        public WebServiceClient(int userId, string licenseKey)
        {
            _userId = userId;
            _licenseKey = licenseKey;
        }

        /// <summary>
        /// Returns an <see cref="OmniResponse"/> model for the specified ip address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>An <see cref="OmniResponse"/> model</returns>
        public OmniResponse Omni(string ipAddress)
        {
            var req = new RestRequest("omni/{ip}");
            req.AddUrlSegment("ip", ipAddress);
            return Execute<OmniResponse>(req);
        }

        private T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient(BASE_URL);
            client.Authenticator = new HttpBasicAuthenticator(_userId.ToString(), _licenseKey);
            client.AddHandler("application/vnd.maxmind.com-omni+json", new JsonDeserializer());;

            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                throw new GeoIP2Exception("An error occurred while executing your request. See the inner exception for details.", response.ErrorException);
            }

            return response.Data;
        }
    }
}