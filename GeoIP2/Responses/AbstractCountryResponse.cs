using System.Collections.Generic;
using MaxMind.GeoIP2.Model;
using Newtonsoft.Json;

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    /// Abstract class for country-level response.
    /// </summary>
    public abstract class AbstractCountryResponse : AbstractResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractCountryResponse"/> class.
        /// </summary>
        public AbstractCountryResponse()
        {
            Continent = new Continent();
            Country = new Country();
            MaxMind = new Model.MaxMind();
            RegisteredCountry = new Country();
            RepresentedCountry = new RepresentedCountry();
            Traits = new Traits();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractCountryResponse"/> class.
        /// </summary>
        public AbstractCountryResponse(Continent continent = null, Country country = null, Model.MaxMind maxMind = null, Country registeredCountry = null, RepresentedCountry representedCountry = null, Traits traits = null)
        {
            Continent = continent ?? new Continent();
            Country = country ?? new Country();
            MaxMind = maxMind ?? new Model.MaxMind();
            RegisteredCountry = registeredCountry ?? new Country();
            RepresentedCountry = representedCountry ?? new RepresentedCountry();
            Traits = traits ?? new Traits();
        }

        /// <summary>
        /// Gets the continent for the requested IP address.
        /// </summary>
        public Continent Continent { get; internal set; }

        /// <summary>
        /// Gets the country for the requested IP address. This
        /// object represents the country where MaxMind believes
        /// the end user is located
        /// </summary>
        public Country Country { get; internal set; }

        /// <summary>
        /// Gets the MaxMind record containing data related to your account
        /// </summary>
        public Model.MaxMind MaxMind { get; internal set; }

        /// <summary>
        /// Registered country record for the requested IP address. This
        /// record represents the country where the ISP has registered a
        /// given IP block and may differ from the user's country.
        /// </summary>
        [JsonProperty("registered_country")]
        public Country RegisteredCountry { get; internal set; }

        /// <summary>
        /// Represented country record for the requested IP address. The
        /// represented country is used for things like military bases or
        /// embassies. It is only present when the represented country
        /// differs from the country.
        /// </summary>
        [JsonProperty("represented_country")]
        public RepresentedCountry RepresentedCountry { get; internal set; }

        /// <summary>
        /// Gets the traits for the requested IP address.
        /// </summary>
        public Traits Traits { get; internal set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.GetType().Name + " ["
                   + (Continent != null ? "Continent=" + Continent + ", " : "")
                   + (Country != null ? "Country=" + Country + ", " : "")
                   + (RegisteredCountry != null ? "RegisteredCountry=" + RegisteredCountry + ", " : "")
                   + (RepresentedCountry != null ? "RepresentedCountry=" + RepresentedCountry + ", " : "")
                   + (Traits != null ? "Traits=" + Traits : "")
                   + "]";
        }

        /// <summary>
        /// Sets the locales on all the NamedEntity properties.
        /// </summary>
        /// <param name="locales">The locales specified by the user.</param>
        internal override void SetLocales(List<string> locales)
        {
            if (Continent != null)
                Continent.Locales = locales;

            if (Country != null)
                Country.Locales = locales;

            if (RegisteredCountry != null)
                RegisteredCountry.Locales = locales;

            if (RepresentedCountry != null)
                RepresentedCountry.Locales = locales;
        }
    }
}
