using Newtonsoft.Json;

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    /// Contains data for the location record associated with an IP address.
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Location() { }

        /// <summary>
        /// Constructor
        /// </summary>
        public Location(int? accuracyRadius = null, double? latitude = null, double? longitude = null, int? metroCode = null, string timeZone = null)
        {
            AccuracyRadius = accuracyRadius;
            Latitude = latitude;
            Longitude = longitude;
            MetroCode = metroCode;
            TimeZone = timeZone;
        }

        /// <summary>
        /// The radius in kilometers around the specified location where the
        /// IP address is likely to be. This attribute is only available from
        /// the Insights end point.
        /// </summary>
        [JsonProperty("accuracy_radius")]
        public int? AccuracyRadius { get; internal set; }

        /// <summary>
        /// Determines whether both the <see cref="Latitude">latitude</see>
        /// and <see cref="Longitude">longitude</see> have values.
        /// </summary>
        [JsonIgnore]
        public bool HasCoordinates
        {
            get { return Latitude.HasValue && Longitude.HasValue; }
        }

        /// <summary>
        /// The latitude of the location as a floating point number.
        /// </summary>
        [JsonProperty("latitude")]
        public double? Latitude { get; internal set; }

        /// <summary>
        /// The longitude of the location as a floating point number.
        /// </summary>
        [JsonProperty("longitude")]
        public double? Longitude { get; internal set; }

        /// <summary>
        /// The metro code of the location if the location is in the US.
        /// MaxMind returns the same metro codes as the <a href=
        /// "https://developers.google.com/adwords/api/docs/appendix/cities-DMAregions"
        /// >Google AdWords API</a>.
        /// </summary>
        [JsonProperty("metro_code")]
        public int? MetroCode { get; internal set; }

        /// <summary>
        /// The time zone associated with location, as specified by the <a
        /// href="http://www.iana.org/time-zones">IANA Time Zone
        /// Database</a>, e.g., "America/New_York".
        /// </summary>
        [JsonProperty("time_zone")]
        public string TimeZone { get; internal set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Location [ "
                + (AccuracyRadius.HasValue ? "AccuracyRadius=" + AccuracyRadius + ", " : string.Empty)
                + (Latitude.HasValue ? "Latitude=" + Latitude + ", " : string.Empty)
                + (Longitude.HasValue ? "Longitude=" + Longitude + ", " : string.Empty)
                + (MetroCode.HasValue ? "MetroCode=" + MetroCode + ", " : string.Empty)
                + (TimeZone != null ? "TimeZone=" + TimeZone : "") + "]";
        }
    }
}