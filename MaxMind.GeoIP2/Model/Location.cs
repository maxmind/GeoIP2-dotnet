#region

using MaxMind.Db;
using System;
using System.Text.Json.Serialization;

#endregion

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Contains data for the location record associated with an IP address.
    /// </summary>
    public class Location
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public Location()
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        [Constructor]
        public Location(
            [Parameter("accuracy_radius")] int? accuracyRadius = null,
            double? latitude = null,
            double? longitude = null,
            [Parameter("metro_code")] int? metroCode = null,
            [Parameter("time_zone")] string? timeZone = null)
        {
            AccuracyRadius = accuracyRadius;
            Latitude = latitude;
            Longitude = longitude;
#pragma warning disable 618
            MetroCode = metroCode;
#pragma warning restore 618
            TimeZone = timeZone;
        }

        /// <summary>
        ///     The approximate accuracy radius in kilometers around the
        ///     latitude and longitude for the IP address. This is the radius
        ///     where we have a 67% confidence that the device using the IP
        ///     address resides within the circle centered at the latitude and
        ///     longitude with the provided radius.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("accuracy_radius")]
        public int? AccuracyRadius { get; internal set; }

        /// <summary>
        ///     The average income in US dollars associated with the IP address.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("average_income")]
        public int? AverageIncome { get; internal set; }

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
        public double? Latitude { get; internal set; }

        /// <summary>
        ///     The approximate longitude of the location associated with the
        ///     IP address. This value is not precise and should not be used
        ///     to identify a particular address or household.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("longitude")]
        public double? Longitude { get; internal set; }

        /// <summary>
        ///     The metro code is a no-longer-maintained code for targeting
        ///     advertisements in Google.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("metro_code")]
        [Obsolete("Code values are no longer maintained.")]
        public int? MetroCode { get; internal set; }

        /// <summary>
        ///     The estimated number of people per square kilometer.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("population_density")]
        public int? PopulationDensity { get; internal set; }

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
        public string? TimeZone { get; internal set; }

        /// <summary>
        ///     Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Location [ "
                   + (AccuracyRadius.HasValue ? "AccuracyRadius=" + AccuracyRadius + ", " : string.Empty)
                   + (Latitude.HasValue ? "Latitude=" + Latitude + ", " : string.Empty)
                   + (Longitude.HasValue ? "Longitude=" + Longitude + ", " : string.Empty)
#pragma warning disable 618
                   + (MetroCode.HasValue ? "MetroCode=" + MetroCode + ", " : string.Empty)
#pragma warning restore 618
                   + (TimeZone != null ? "TimeZone=" + TimeZone : "") + "]";
        }
    }
}
