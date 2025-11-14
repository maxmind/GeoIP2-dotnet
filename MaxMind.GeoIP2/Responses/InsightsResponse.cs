#region

using MaxMind.GeoIP2.Model;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#endregion

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     This class provides a model for the data returned by the GeoIP2
    ///     Insights web service.
    /// </summary>
    public class InsightsResponse : AbstractCityResponse
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public InsightsResponse()
        {
            Anonymizer = new Anonymizer();
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public InsightsResponse(
            Anonymizer? anonymizer = null,
            City? city = null,
            Continent? continent = null,
            Country? country = null,
            Location? location = null,
            Model.MaxMind? maxMind = null,
            Postal? postal = null,
            Country? registeredCountry = null,
            RepresentedCountry? representedCountry = null,
            IReadOnlyList<Subdivision>? subdivisions = null,
            Traits? traits = null)
            : base(
                city, continent, country, location, maxMind, postal, registeredCountry, representedCountry, subdivisions,
                traits)
        {
            Anonymizer = anonymizer ?? new Anonymizer();
        }

        /// <summary>
        ///     Constructor for backward compatibility
        /// </summary>
        [Obsolete("Use constructor with anonymizer parameter")]
        public InsightsResponse(
            City? city,
            Continent? continent,
            Country? country,
            Location? location,
            Model.MaxMind? maxMind,
            Postal? postal,
            Country? registeredCountry,
            RepresentedCountry? representedCountry,
            IReadOnlyList<Subdivision>? subdivisions,
            Traits? traits)
            : this(
                null, // anonymizer
                city,
                continent,
                country,
                location,
                maxMind,
                postal,
                registeredCountry,
                representedCountry,
                subdivisions,
                traits)
        {
        }

        /// <summary>
        ///     Gets anonymizer-related data for the requested IP address.
        ///     This is available from the GeoIP2 Insights web service.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("anonymizer")]
        public Anonymizer Anonymizer { get; internal set; }
    }
}
