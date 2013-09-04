using System.Collections.Generic;
using MaxMind.GeoIP2.Model;

namespace MaxMind.GeoIP2.Responses
{
    public class OmniResponse
    {
        public City City { get; set; }
        public Continent Continent { get; set; }
        public Country Country { get; set; }
        public Location Location { get; set; }
        public Model.MaxMind MaxMind { get; set; }
        public Postal Postal { get; set; }
        public Country RegisteredCountry { get; set; }
        public RepresentedCountry RepresentedCountry { get; set; }
        public List<Subdivision> Subdivisions { get; set; }
        public Traits Traits { get; set; }
    }
}