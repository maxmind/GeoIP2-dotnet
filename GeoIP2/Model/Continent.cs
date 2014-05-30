using System.Collections.Generic;
using Newtonsoft.Json;

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    /// Contains data for the continent record associated with an IP address.
    ///  
    ///  This record is returned by all the end points.    
    /// </summary>
    public class Continent : NamedEntity
    {
        public Continent() { }
        public Continent(string code = null, Dictionary<string, string> names = null, List<string> locales = null) : base(names, locales)
        {
            Code = code;
        }

        /// <summary>
        /// A two character continent code like "NA" (North America) or "OC"
        /// (Oceania). This attribute is returned by all end points.        
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; internal set; }
    }
}