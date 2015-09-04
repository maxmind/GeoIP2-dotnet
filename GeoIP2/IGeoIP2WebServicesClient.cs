using MaxMind.GeoIP2.Responses;
using System.Net;
using System.Threading.Tasks;

namespace MaxMind.GeoIP2
{
    /// <summary>
    ///     Interface for web-service client
    /// </summary>
    public interface IGeoIP2WebServicesClient : IGeoIP2Provider
    {
        /// <summary>
        ///     Returns an <see cref="InsightsResponse" /> for the specified ip address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>An <see cref="InsightsResponse" /></returns>
        InsightsResponse Insights(string ipAddress);

        /// <summary>
        ///     Returns an <see cref="InsightsResponse" /> for the specified ip address.
        /// </summary>
        /// <param name="ip">The ip address.</param>
        /// <returns>An <see cref="InsightsResponse" /></returns>
        InsightsResponse Insights(IPAddress ipAddress);

        Task<CountryResponse> CountryAsync(string ipAddress);

        Task<CountryResponse> CountryAsync(IPAddress ipAddress);

        Task<CityResponse> CityAsync(string ipAddress);

        Task<CityResponse> CityAsync(IPAddress ipAddress);

        Task<InsightsResponse> InsightsAsync(string ipAddress);

        Task<InsightsResponse> InsightsAsync(IPAddress ipAddress);
    }
}