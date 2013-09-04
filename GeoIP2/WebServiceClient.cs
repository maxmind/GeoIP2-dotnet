using System.Runtime.InteropServices.ComTypes;
using MaxMind.GeoIP2.Model;
using RestSharp;
using RestSharp.Deserializers;

namespace MaxMind.GeoIP2
{
    public class WebServiceClient
    {
        private readonly int _userId;
        private readonly string _licenseKey;
        private const string BASE_URL = "https://geoip.maxmind.com/geoip/v2.0";

        public WebServiceClient(int userId, string licenseKey)
        {
            _userId = userId;
            _licenseKey = licenseKey;
        }

        public Omni Omni(string ipAddress)
        {
            var req = new RestRequest("omni/{ip}");
            req.AddUrlSegment("ip", ipAddress);
            return Execute<Omni>(req);
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