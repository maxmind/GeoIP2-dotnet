namespace MaxMind.GeoIP2.Model
{
    public class Traits
    {
        public int AutonomousSystemNumber { get; set; }
        public string AutonomousSystemOrganization { get; set; }
        public string Domain { get; set; }
        public string IpAddress { get; set; }
        public bool IsAnonymousProxy { get; set; }
        public bool IsSatelliteProvider { get; set; }
        public string Isp { get; set; }
        public string Organization { get; set; }
        public string UserType { get; set; }
    }
}