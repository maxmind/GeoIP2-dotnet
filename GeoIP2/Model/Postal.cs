namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    /// Contains data for the postal record associated with an IP address.
    /// This record is returned by all the end points except the Country end point.
    /// </summary>
    public class Postal
    {
        /// <summary>
        /// The postal code of the location. Postal codes are not available
        /// for all countries. In some countries, this will only contain part
        /// of the postal code. This attribute is returned by all end points
        /// except the Country end point.
        /// </summary>
        public string Code { get; internal set; }

        /// <summary>
        /// A value from 0-100 indicating MaxMind's confidence that the
        /// postal code is correct. This attribute is only available from the
        /// Omni end point.
        /// </summary>
        public int Confidence { get; internal set; }
    }
}