using MaxMind.Db;
using System.Text.Json.Serialization;

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Contains data for the represented country associated with an IP address.
    ///     This record contains the country-level data associated with an IP address for
    ///     the IP's represented country. The represented country is the country
    ///     represented by something like a military base.
    ///     Do not use any of the country names as a database or dictionary
    ///     key. Use the <see cref="NamedEntity.GeoNameId" /> or <see cref="Country.IsoCode" />
    ///     instead.
    /// </summary>
    public record RepresentedCountry : Country
    {
        /// <summary>
        ///     A string indicating the type of entity that is representing the
        ///     country. Currently we only return <c>military</c> but this could
        ///     expand to include other types in the future.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("type")]
        [MapKey("type")]
        public string? Type { get; init; }
    }
}
