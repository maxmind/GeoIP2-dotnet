using System.Collections.Generic;

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     Abstract base record for all responses.
    /// </summary>
    public abstract record AbstractResponse
    {
        /// <summary>
        ///     Creates a copy of this response with locales set on all NamedEntity properties.
        /// </summary>
        /// <param name="locales">The locales specified by the user.</param>
        /// <returns>A new response with the locales set.</returns>
        internal virtual AbstractResponse WithLocales(IReadOnlyList<string> locales)
        {
            return this;
        }
    }
}
