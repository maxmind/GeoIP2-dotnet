#region

using MaxMind.Db;
using MaxMind.GeoIP2.Model;
using System.Collections.Generic;

#endregion

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     This class provides a model for the data returned by the GeoIP2 Enterprise
    ///     database.
    /// </summary>
    /// <remarks>
    ///     Constructor
    /// </remarks>
    [method: Constructor]
    public class EnterpriseResponse(
        City? city = null,
        Continent? continent = null,
        Country? country = null,
        Location? location = null,
        Model.MaxMind? maxMind = null,
        Postal? postal = null,
        Country? registeredCountry = null,
        RepresentedCountry? representedCountry = null,
        IReadOnlyList<Subdivision>? subdivisions = null,
        Traits? traits = null) : AbstractCityResponse(
            city, continent, country, location, maxMind, postal, registeredCountry, representedCountry, subdivisions,
            traits)
    {
    }
}
