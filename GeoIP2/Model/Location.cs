namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    /// Contains data for the location record associated with an IP address.
    /// 
    /// This record is returned by all the end points except the Country end point.
    /// </summary>
    public class Location
    {
        /// <summary>
        /// The radius in kilometers around the specified location where the
        /// IP address is likely to be. This attribute is only available from
        /// the Omni end point.
        /// </summary>
        public int? AccuracyRadius { get; internal set; }

        /// <summary>
        /// The latitude of the location as a floating point number. This
        /// attribute is returned by all end points except the Country end
        /// point.
        /// </summary>
        public double? Latitude { get; internal set; }

        /// <summary>
        /// The longitude of the location as a floating point number. This
        /// attribute is returned by all end points except the Country end
        /// point.
        /// </summary>
        public double? Longitude { get; internal set; }

        /// <summary>
        /// The metro code of the location if the location is in the US.
        /// MaxMind returns the same metro codes as the <a href=
        /// "https://developers.google.com/adwords/api/docs/appendix/cities-DMAregions"
        /// >Google AdWords API</a>. This attribute is returned by all end
        /// points except the Country end point.
        /// </summary>
        public int? MetroCode { get; internal set; }

        /// <summary>
        /// The time zone associated with location, as specified by the <a
        /// href="http://www.iana.org/time-zones">IANA Time Zone
        /// Database</a>, e.g., "America/New_York". This attribute is
        /// returned by all end points except the Country end point
        /// </summary>
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