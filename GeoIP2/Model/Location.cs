namespace MaxMind.GeoIP2.Model
{
    public class Location
    {
        public int AccuracyRadius { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int MetroCode { get; set; }
        public string TimeZone { get; set; }
    }
}