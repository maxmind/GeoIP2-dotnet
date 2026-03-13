using MaxMind.Db;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Abstract record for entities with name maps.
    /// </summary>
    public abstract record NamedEntity
    {
        /// <summary>
        ///     A <see cref="System.Collections.Generic.Dictionary{T,U}" />
        ///     from locale codes to the name in that locale. Don't use any of
        ///     these names as a database or dictionary key. Use the
        ///     <see
        ///         cref="GeoNameId" />
        ///     or relevant code instead.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("names")]
        [MapKey("names")]
        public IReadOnlyDictionary<string, string> Names { get; init; }
            = new Dictionary<string, string>();

        /// <summary>
        ///     The GeoName ID for the entity.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("geoname_id")]
        [MapKey("geoname_id")]
        public long? GeoNameId { get; init; }

        /// <summary>
        ///     The locales specified by the user.
        /// </summary>
        [JsonIgnore]
        [Inject("locales")]
        public IReadOnlyList<string> Locales { get; init; } = ["en"];

        /// <summary>
        ///     The name based on the locales list passed to the
        ///     <see cref="WebServiceClient" /> or <c>DatabaseReader</c>. Don't use any of
        ///     these names as a database or dictionary key. Use the
        ///     <see
        ///         cref="GeoNameId" />
        ///     or relevant code instead.
        /// </summary>
        [JsonIgnore]
        public string? Name =>
            Locales.FirstOrDefault(l => Names.ContainsKey(l)) is { } locale
                ? Names[locale] : null;
    }
}
