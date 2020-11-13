#region

using MaxMind.Db;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#endregion

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     City-level data associated with an IP address.
    /// </summary>
    /// <remarks>
    ///     Do not use any of the city names as a database or dictionary
    ///     key. Use the <see cred="GeoNameId" /> instead.
    /// </remarks>
    public class City : NamedEntity
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public City()
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        [Constructor]
        public City(int? confidence = null,
            [Parameter("geoname_id")] long? geoNameId = null,
            IReadOnlyDictionary<string, string>? names = null,
            IReadOnlyList<string>? locales = null)
            : base(geoNameId, names, locales)
        {
            Confidence = confidence;
        }

        /// <summary>
        ///     A value from 0-100 indicating MaxMind's confidence that the city
        ///     is correct. This value is only set when using the Insights
        ///     web service or the Enterprise database.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("confidence")]
        public int? Confidence { get; internal set; }
    }
}