using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MaxMind.GeoIP2.Model;

namespace MaxMind.GeoIP2.Responses
{
    public class CountryResponse
    {
        public Continent Continent { get; internal set; }
        public Country Country { get; internal set; }
        public Model.MaxMind MaxMind { get; internal set; }
        public Country RegisteredCountry { get; internal set; }
        public RepresentedCountry RepresentedCountry { get; internal set; }
        public Traits Traits { get; internal set; }
    }
}
