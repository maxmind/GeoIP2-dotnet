using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MaxMind.GeoIP2.Model;

namespace MaxMind.GeoIP2.Responses
{

    /// <summary>
    /// This class provides a model for the data returned by the GeoIP2 City end
    /// point.
    /// 
    /// The only difference between the City, City/ISP/Org, and Omni response classes is
    /// which fields in each record may be populated.
    /// 
    /// <a href="http://dev.maxmind.com/geoip/geoip2/web-services">GeoIP2 Web
    ///      Services</a>
    /// </summary>
    public class CityResponse : CountryResponse
    {
        /// <summary>
        /// Gets the city for the requested IP address.
        /// </summary>
        public City City { get; internal set; }

        /// <summary>
        /// Gets the location for the requested IP address.
        /// </summary>
        public Location Location { get; internal set; }

        /// <summary>
        /// Gets the postal object for the requested IP address.
        /// </summary>
        public Postal Postal { get; internal set; }

        /// <summary>
        /// An <see cref="System.Collections.Generic.List{T}"/> of <see cref="Subdivision"/> objects representing
        /// the country subdivisions for the requested IP address. The number
        /// and type of subdivisions varies by country, but a subdivision is
        /// typically a state, province, county, etc. Subdivisions are
        /// ordered from most general (largest) to most specific (smallest).
        /// If the response did not contain any subdivisions, this method
        /// returns an empty array.
        /// </summary>
        public List<Subdivision> Subdivisions { get; internal set; }

        /// <summary>
        /// An object representing the most specific subdivision returned. If
        /// the response did not contain any subdivisions, this method
        /// returns an empty <see cref="Subdivision"/> object.
        /// </summary>
        public Subdivision MostSpecificSubdivision
        {
            get
            {
                if(Subdivisions == null || Subdivisions.Count == 0)
                    return new Subdivision();

                return Subdivisions.Last();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.GetType().Name + " [" 
                + (City != null ? "City=" + City + ", " : "")
                + (Location != null ? "Location=" + Location + ", " : "")
                + (Postal != null ? "Postal=" + Postal + ", " : "")
                + (Subdivisions != null ? "Subdivisions={" + string.Join(",", Subdivisions.Select(s => s.ToString()).ToArray()) + "}, " : "")
                + (Continent != null ? "Continent=" + Continent + ", " : "")
                + (Country != null ? "Country=" + Country + ", " : "")
                + (RegisteredCountry != null ? "RegisteredCountry=" + RegisteredCountry + ", " : "") 
                + (RepresentedCountry != null ? "RepresentedCountry=" + RepresentedCountry + ", " : "")
                + (Traits != null ? "Traits=" + Traits : "") 
                + "]";
        }

        /// <summary>
        /// Sets the languages on all the NamedEntity properties.
        /// </summary>
        /// <param name="languages">The languages specified by the user.</param>
        internal override void SetLanguages(List<string> languages)
        {
            base.SetLanguages(languages);

            if(City != null)
                City.Languages = languages;

            if(Subdivisions != null && Subdivisions.Count > 0)
                foreach (var subdivision in Subdivisions)
                    subdivision.Languages = languages;
        }
    }
}
