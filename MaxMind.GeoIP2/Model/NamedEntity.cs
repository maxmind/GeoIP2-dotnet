#region

using MaxMind.Db;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;

#endregion

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Abstract class for records with name maps.
    /// </summary>
    public abstract class NamedEntity
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        [Constructor]
        protected NamedEntity(long? geoNameId = null, IReadOnlyDictionary<string, string>? names = null,
            IReadOnlyList<string>? locales = null)
        {
            Names = names ?? new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
            GeoNameId = geoNameId;
            Locales = locales ?? new List<string> { "en" }.AsReadOnly();
        }

        /// <summary>
        ///     A <see cref="System.Collections.Generic.Dictionary{T,U}" />
        ///     from locale codes to the name in that locale. Don't use any of
        ///     these names as a database or dictionary key. Use the
        ///     <see
        ///         cred="GeoNameId" />
        ///     or relevant code instead.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("names")]
        public IReadOnlyDictionary<string, string> Names { get; internal set; }

        /// <summary>
        ///     The GeoName ID for the city.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("geoname_id")]
        public long? GeoNameId { get; internal set; }

        /// <summary>
        ///     Gets or sets the locales specified by the user.
        /// </summary>
        [JsonIgnore]
        protected internal IReadOnlyList<string> Locales { get; set; }

        /// <summary>
        ///     The name of the city based on the locales list passed to the
        ///     <see cref="WebServiceClient" /> constructor. Don't use any of
        ///     these names as a database or dictionary key. Use the
        ///     <see
        ///         cred="GeoNameId" />
        ///     or relevant code instead.
        /// </summary>
        [JsonIgnore]
        public string? Name
        {
            get
            {
                var locale = Locales.FirstOrDefault(l => Names.ContainsKey(l));
                return locale == null ? null : Names[locale];
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
