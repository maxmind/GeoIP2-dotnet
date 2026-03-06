using System;
using MaxMind.Db;
using System.Text.Json.Serialization;

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Contains data for the location record associated with an IP address.
    /// </summary>
    public record Location
    {
        /// <summary>
        ///     The approximate accuracy radius in kilometers around the
        ///     latitude and longitude for the IP address. This is the radius
        ///     where we have a 67% confidence that the device using the IP
        ///     address resides within the circle centered at the latitude and
        ///     longitude with the provided radius.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("accuracy_radius")]
        [MapKey("accuracy_radius")]
        public int? AccuracyRadius { get; init; }

        /// <summary>
        ///     The average income in US dollars associated with the IP address.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("average_income")]
        [MapKey("average_income")]
        public int? AverageIncome { get; init; }

        /// <summary>
        ///     Determines whether both the <see cref="Latitude">Latitude</see>
        ///     and <see cref="Longitude">Longitude</see> have values.
        /// </summary>
        [JsonIgnore]
        public bool HasCoordinates => Latitude.HasValue && Longitude.HasValue;

        /// <summary>
        ///     The approximate latitude of the location associated with the
        ///     IP address. This value is not precise and should not be used
        ///     to identify a particular address or household.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("latitude")]
        [MapKey("latitude")]
        public double? Latitude { get; init; }

        /// <summary>
        ///     The approximate longitude of the location associated with the
        ///     IP address. This value is not precise and should not be used
        ///     to identify a particular address or household.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("longitude")]
        [MapKey("longitude")]
        public double? Longitude { get; init; }

        /// <summary>
        ///     The metro code is a no-longer-maintained code for targeting
        ///     advertisements in Google.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("metro_code")]
        [MapKey("metro_code")]
        [Obsolete("Code values are no longer maintained.")]
        public int? MetroCode { get; init; }

        /// <summary>
        ///     The estimated number of people per square kilometer.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("population_density")]
        [MapKey("population_density")]
        public int? PopulationDensity { get; init; }

        /// <summary>
        ///     The time zone associated with location, as specified by the
        ///     <a
        ///         href="https://www.iana.org/time-zones">
        ///         IANA Time Zone
        ///         Database
        ///     </a>
        ///     , e.g., "America/New_York".
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("time_zone")]
        [MapKey("time_zone")]
        public string? TimeZone { get; init; }
    }
}
