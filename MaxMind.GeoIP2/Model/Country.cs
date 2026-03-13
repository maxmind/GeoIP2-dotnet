using MaxMind.Db;
using System.Text.Json.Serialization;

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Contains data for the country record associated with an IP address.
    ///     Do not use any of the country names as a database or dictionary
    ///     key. Use the <see cref="NamedEntity.GeoNameId" /> or <see cref="IsoCode" />
    ///     instead.
    /// </summary>
    public record Country : NamedEntity
    {
        /// <summary>
        ///     A value from 0-100 indicating MaxMind's confidence that the country
        ///     is correct. This value is only set when using the Insights
        ///     web service or the Enterprise database.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("confidence")]
        [MapKey("confidence")]
        public int? Confidence { get; init; }

        /// <summary>
        ///     This is true if the country is a member state of the
        ///     European Union. This is available from  all location
        ///     services and databases.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("is_in_european_union")]
        [MapKey("is_in_european_union")]
        public bool IsInEuropeanUnion { get; init; }

        /// <summary>
        ///     The
        ///     <a
        ///         href="https://en.wikipedia.org/wiki/ISO_3166-1">
        ///         two-character ISO
        ///         3166-1 alpha code
        ///     </a>
        ///     for the country.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("iso_code")]
        [MapKey("iso_code")]
        public string? IsoCode { get; init; }
    }
}
