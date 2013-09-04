namespace MaxMind.GeoIP2.Model
{
    public class Subdivision : NamedEntity
    {
        public int Confidence { get; set; }
        public string IsoCode { get; set; }
    }
}