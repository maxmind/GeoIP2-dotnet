namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    /// This class provides a model for the data returned by the GeoIP2 City/ISP/Org
    /// end point.
    /// 
    /// The only difference between the City, City/ISP/Org, and Omni response classes is
    /// which fields in each record may be populated.
    /// 
    /// <a href="http://dev.maxmind.com/geoip/geoip2/web-services">GeoIP2 Web
    ///      Services</a>
    /// </summary>
    public class CityIspOrgResponse : AbstractCityResponse
    {
         
    }
}