using System.Collections.Generic;

namespace MaxMind.GeoIP2.Model
{
    public class City : NamedEntity
    {
        public int Confidence { get; set; }
    }
}