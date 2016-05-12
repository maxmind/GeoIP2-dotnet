﻿#region

using MaxMind.GeoIP2.Responses;
using System.Net;
using System.Threading.Tasks;

#endregion

namespace MaxMind.GeoIP2
{
    /// <summary>
    ///     Interface for web-service client
    /// </summary>
#if !NETSTANDARD1_4
    public interface IGeoIP2WebServicesClient : IGeoIP2Provider
#else
    public interface IGeoIP2WebServicesClient
#endif
    {
#if !NETSTANDARD1_4
        /// <summary>
        ///     Returns an <see cref="CountryResponse" /> for the requesting IP address.
        /// </summary>
        /// <returns>An <see cref="CountryResponse" /></returns>
        CountryResponse Country();

        /// <summary>
        ///     Returns an <see cref="CityResponse" /> for the requesting IP address.
        /// </summary>
        /// <returns>An <see cref="CityResponse" /></returns>
        CityResponse City();

        /// <summary>
        ///     Returns an <see cref="InsightsResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="InsightsResponse" /></returns>
        InsightsResponse Insights(string ipAddress);

        /// <summary>
        ///     Returns an <see cref="InsightsResponse" /> for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>An <see cref="InsightsResponse" /></returns>
        InsightsResponse Insights(IPAddress ipAddress);

        /// <summary>
        ///     Returns an <see cref="InsightsResponse" /> for the requesting IP address.
        /// </summary>
        /// <returns>An <see cref="InsightsResponse" /></returns>
        InsightsResponse Insights();
#endif

        /// <summary>
        ///     Asynchronously query the GeoIP2 Precision: Country web service for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>Task that produces an object modeling the Country response</returns>
        Task<CountryResponse> CountryAsync(string ipAddress);

        /// <summary>
        ///     Asynchronously query the GeoIP2 Precision: Country web service for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>Task that produces an object modeling the Country response</returns>
        Task<CountryResponse> CountryAsync(IPAddress ipAddress);

        /// <summary>
        ///     Asynchronously query the GeoIP2 Precision: Country web service for the requesting IP address.
        /// </summary>
        /// <returns>Task that produces an object modeling the Country response</returns>
        Task<CountryResponse> CountryAsync();

        /// <summary>
        ///     Asynchronously query the GeoIP2 Precision: City web service for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>Task that produces an object modeling the City response</returns>
        Task<CityResponse> CityAsync(string ipAddress);

        /// <summary>
        ///     Asynchronously query the GeoIP2 Precision: City web service for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>Task that produces an object modeling the City response</returns>
        Task<CityResponse> CityAsync(IPAddress ipAddress);

        /// <summary>
        ///     Asynchronously query the GeoIP2 Precision: City web service for the requesting IP address.
        /// </summary>
        /// <returns>Task that produces an object modeling the City response</returns>
        Task<CityResponse> CityAsync();

        /// <summary>
        ///     Asynchronously query the GeoIP2 Precision: Insights web service for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>Task that produces an object modeling the Insights response</returns>
        Task<InsightsResponse> InsightsAsync(string ipAddress);

        /// <summary>
        ///     Asynchronously query the GeoIP2 Precision: Insights web service for the specified IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address.</param>
        /// <returns>Task that produces an object modeling the Insights response</returns>
        Task<InsightsResponse> InsightsAsync(IPAddress ipAddress);

        /// <summary>
        ///     Asynchronously query the GeoIP2 Precision: Insights web service for the requesting IP address.
        /// </summary>
        /// <returns>Task that produces an object modeling the Insights response</returns>
        Task<InsightsResponse> InsightsAsync();
    }
}