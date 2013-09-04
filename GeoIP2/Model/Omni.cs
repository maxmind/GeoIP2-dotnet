using System.Collections.Generic;

namespace MaxMind.GeoIP2.Model
{
    public class Omni
    {
        public City City { get; set; }
        public Continent Continent { get; set; }
        public Country Country { get; set; }
        public Location Location { get; set; }
        public MaxMind MaxMind { get; set; }
        public Postal Postal { get; set; }
        public Country RegisteredCountry { get; set; }
        public RepresentedCountry RepresentedCountry { get; set; }
        public List<Subdivision> Subdivisions { get; set; }
        public Traits Traits { get; set; }
    }
}