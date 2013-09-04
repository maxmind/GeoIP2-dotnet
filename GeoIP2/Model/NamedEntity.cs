using System.Collections.Generic;
using System.Linq;

namespace MaxMind.GeoIP2.Model
{
    public class NamedEntity
    {
        public Dictionary<string, string> Names { get; set; }
        public int GeonameId { get; set; }
        internal List<string> Languages = new List<string>();

        public string Name
        {
            get
            {
                var lang = Languages.FirstOrDefault(l => Names.ContainsKey(l));
                return lang == null ? null : Names[lang];
            }
        }
    }
}