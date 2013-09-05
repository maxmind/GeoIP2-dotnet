using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MaxMind.GeoIP2.Model;

namespace MaxMind.GeoIP2.Responses
{
    public class CityResponse : CountryResponse
    {
        public City City { get; internal set; }
        public Location Location { get; internal set; }
        public Postal Postal { get; internal set; }
        public List<Subdivision> Subdivisions { get; internal set; }
    }
}
