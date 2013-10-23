using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    /// Abstract class for records with name maps.
    /// </summary>
    public abstract class NamedEntity
    {
        [JsonProperty("names")]
        private Dictionary<string, string> _names;

        public NamedEntity()
        {
            Names = new Dictionary<string, string>();
            Locales = new List<string>();
        }

        /// <summary>
        /// A <see cref="System.Collections.Generic.Dictionary{T,U}"/> from locale codes to the name in that locale.
        /// This attribute is returned by all end points.
        /// </summary>
        public Dictionary<string, string> Names
        {
            get { return new Dictionary<string, string>(_names); }
            internal set { _names = value; }
        }

        /// <summary>
        /// The GeoName ID for the city. This attribute is returned by all endpoints
        /// </summary>
        [JsonProperty("geoname_id")]
        public int GeonameID { get; internal set; }

        /// <summary>
        /// Gets or sets the locales specified by the user.
        /// </summary>
        internal List<string> Locales { get; set; }

        /// <summary>
        /// The name of the city based on the locales list passed to the
        /// <see cref="WebServiceClient"/> constructor. This
        /// attribute is returned by all endpoints.
        /// </summary>
        public string Name
        {
            get
            {
                var locale = Locales.FirstOrDefault(l => Names.ContainsKey(l));
                return locale == null ? null : Names[locale];
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name ?? string.Empty;
        }
    }
}
