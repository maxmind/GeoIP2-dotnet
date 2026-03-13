using MaxMind.Db;
using System.Text.Json.Serialization;

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Contains data for the subdivisions associated with an IP address.
    ///     Do not use any of the subdivision names as a database or dictionary
    ///     key. Use the <see cref="NamedEntity.GeoNameId" /> or <see cref="IsoCode" />
    ///     instead.
    /// </summary>
    public record Subdivision : NamedEntity
    {
        /// <summary>
        ///     This is a value from 0-100 indicating MaxMind's confidence that
        ///     the subdivision is correct. This value is only set when using the
        ///     Insights web service or the Enterprise database.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("confidence")]
        [MapKey("confidence")]
        public int? Confidence { get; init; }

        /// <summary>
        ///     This is a string up to three characters long contain the
        ///     subdivision portion of the
        ///     <a
        ///         href="https://en.wikipedia.org/wiki/ISO_3166-2">
        ///         ISO 3166-2 code
        ///     </a>
        ///     .
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("iso_code")]
        [MapKey("iso_code")]
        public string? IsoCode { get; init; }
    }
}
