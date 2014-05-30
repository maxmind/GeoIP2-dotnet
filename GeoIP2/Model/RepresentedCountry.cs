using System.Collections.Generic;
using Newtonsoft.Json;

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    /// Contains data for the represented country associated with an IP address.
    /// 
    /// This class contains the country-level data associated with an IP address for
    /// the IP's represented country. The represented country is the country
    /// represented by something like a military base or embassy.
    /// 
    /// This record is returned by all the end points.
    /// </summary>
    public class RepresentedCountry : Country
    {
        public RepresentedCountry() { }
        public RepresentedCountry(string type = null, int? confidence = null, string isoCode = null, Dictionary<string, string> names = null, int? geoNameId = null, List<string> locales = null) : base(confidence, isoCode, names, geoNameId, locales)
        {
            Type = type;
        }
        /// <summary>
        /// A string indicating the type of entity that is representing the
        /// country. Currently we only return <c>military</c> but this could
        /// expand to include other types such as <c>embassy</c> in the
        /// future. Returned by all end points.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; internal set; }
    }
}