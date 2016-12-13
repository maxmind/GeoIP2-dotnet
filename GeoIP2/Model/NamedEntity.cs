﻿#region

using MaxMind.Db;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Abstract class for records with name maps.
    /// </summary>
    public abstract class NamedEntity
    {
        [JsonProperty("names")]
        private IDictionary<string, string> _names;

        /// <summary>
        ///     Constructor
        /// </summary>
        [Constructor]
        protected NamedEntity(int? geoNameId = null, IDictionary<string, string> names = null,
            IEnumerable<string> locales = null)
        {
            Names = names != null ? new Dictionary<string, string>(names) : new Dictionary<string, string>();
            GeoNameId = geoNameId;
            Locales = locales != null ? new List<string>(locales) : new List<string> { "en" };
        }

        /// <summary>
        ///     A <see cref="System.Collections.Generic.Dictionary{T,U}" />
        ///     from locale codes to the name in that locale. Don't use any of
        ///     these names as a database or dictionary key. Use the
        ///     <see
        ///         cred="GeoNameId" />
        ///     or relevant code instead.
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, string> Names
        {
            get { return new Dictionary<string, string>(_names); }
            internal set { _names = value; }
        }

        /// <summary>
        ///     The GeoName ID for the city.
        /// </summary>
        [JsonProperty("geoname_id")]
        public int? GeoNameId { get; internal set; }

        /// <summary>
        ///     Gets or sets the locales specified by the user.
        /// </summary>
        [JsonIgnore]
        protected internal IEnumerable<string> Locales { get; set; }

        /// <summary>
        ///     The name of the city based on the locales list passed to the
        ///     <see cref="WebServiceClient" /> constructor. Don't use any of
        ///     these names as a database or dictionary key. Use the
        ///     <see
        ///         cred="GeoNameId" />
        ///     or relevant code instead.
        /// </summary>
        [JsonIgnore]
        public string Name
        {
            get
            {
                var names = _names;
                var locale = Locales.FirstOrDefault(l => names.ContainsKey(l));
                return locale == null ? null : names[locale];
            }
        }

        /// <summary>
        ///     Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name ?? string.Empty;
        }
    }
}