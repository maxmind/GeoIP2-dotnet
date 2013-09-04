namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    /// Contains data for the traits record associated with an IP address.
    /// This record is returned by all the end points.
    /// </summary>
    public class Traits
    {
        /// <summary>
        /// The <a
        /// href="http://en.wikipedia.org/wiki/Autonomous_system_(Internet)"
        /// >autonomous system number</a> associated with the IP address.
        /// This attribute is only available from the City/ISP/Org and Omni
        /// end points.
        /// </summary>
        public int AutonomousSystemNumber { get; internal set; }

        /// <summary>
        /// The organization associated with the registered <a
        /// href="http://en.wikipedia.org/wiki/Autonomous_system_(Internet)"
        /// >autonomous system number</a> for the IP address. This attribute
        /// is only available from the City/ISP/Org and Omni end points.
        /// </summary>
        public string AutonomousSystemOrganization { get; internal set; }

        /// <summary>
        /// The second level domain associated with the IP address. This will
        /// be something like "example.com" or "example.co.uk", not
        /// "foo.example.com". This attribute is only available from the
        /// City/ISP/Org and Omni end points.
        /// </summary>
        public string Domain { get; internal set; }

        /// <summary>
        /// The IP address that the data in the model is for. If you
        /// performed a "me" lookup against the web service, this will be the
        /// externally routable IP address for the system the code is running
        /// on. If the system is behind a NAT, this may differ from the IP
        /// address locally assigned to it. This attribute is returned by all
        /// end points.
        /// </summary>
        public string IpAddress { get; internal set; }

        /// <summary>
        /// This is true if the IP is an anonymous proxy. This attribute is
        /// returned by all end points.
        /// <a href="http://dev.maxmind.com/faq/geoip#anonproxy">MaxMind's GeoIP
        /// FAQ</a>
        /// </summary>
        public bool IsAnonymousProxy { get; internal set; }

        /// <summary>
        /// This is true if the IP belong to a satellite internet provider.
        /// This attribute is returned by all end points.
        /// </summary>
        public bool IsSatelliteProvider { get; internal set; }

        /// <summary>
        /// The name of the ISP associated with the IP address. This
        /// attribute is only available from the City/ISP/Org and Omni end
        /// points.
        /// </summary>
        public string Isp { get; internal set; }

        /// <summary>
        /// The name of the organization associated with the IP address. This
        /// attribute is only available from the City/ISP/Org and Omni end
        /// points.
        /// </summary>
        public string Organization { get; internal set; }

        /// <summary>
        /// The user type associated with the IP address. This can be one of
        /// the following values:
        /// business
        /// cafe
        /// cellular
        /// college
        /// content_delivery_network
        /// dialup
        /// government
        /// hosting
        /// library
        /// military
        /// residential
        /// router
        /// school
        /// search_engine_spider
        /// traveler
        /// This attribute is only available from the Omni end point.
        /// </summary>
        public string UserType { get; internal set; }
    }
}