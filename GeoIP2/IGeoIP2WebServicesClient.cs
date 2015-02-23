using MaxMind.GeoIP2.Responses;

namespace MaxMind.GeoIP2
{
    public interface IGeoIP2WebServicesClient : IGeoIP2Provider
    {
        /// <summary>
        /// Returns an <see cref="InsightsResponse"/> for the specified ip address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>An <see cref="InsightsResponse"/></returns>
        InsightsResponse Insights(string ipAddress);
    }
}