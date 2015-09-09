﻿#region

using Newtonsoft.Json;
using System.Collections.Generic;

#endregion

namespace MaxMind.GeoIP2.Model
{
    /// <summary>
    ///     Contains data for the represented country associated with an IP address.
    ///     This class contains the country-level data associated with an IP address for
    ///     the IP's represented country. The represented country is the country
    ///     represented by something like a military base.
    /// </summary>
    public class RepresentedCountry : Country
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public RepresentedCountry()
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public RepresentedCountry(string type = null, int? confidence = null, int? geoNameId = null,
            string isoCode = null, Dictionary<string, string> names = null, List<string> locales = null)
            : base(confidence, geoNameId, isoCode, names, locales)
        {
            Type = type;
        }

        /// <summary>
        ///     A string indicating the type of entity that is representing the
        ///     country. Currently we only return <c>military</c> but this could
        ///     expand to include other types in the future.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; internal set; }
    }
}