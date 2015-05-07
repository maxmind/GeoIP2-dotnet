using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaxMind.GeoIP2.Responses
{
    /// <summary>
    /// Abstract class that represents a generic response.
    /// </summary>
    public abstract class AbstractResponse
    {
        // This is simplify the database API. Also, we may need to use the locales in the future.
        protected internal virtual void SetLocales(List<string> locales)
        {
        }
    }
}