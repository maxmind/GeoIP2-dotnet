#region

using System.Collections.Generic;

#endregion

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    ///     Abstract class that represents a generic response.
    /// </summary>
    public abstract class AbstractResponse
    {
        /// <summary>
        ///     This is simplify the database API. Also, we may need to use the locales in the future.
        /// </summary>
        /// <param name="locales"></param>
        protected internal virtual void SetLocales(IEnumerable<string> locales)
        {
        }
    }
}