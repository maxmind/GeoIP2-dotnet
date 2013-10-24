using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaxMind.GeoIP2.Exceptions
{
    /// <summary>
    /// This exception is thrown when there is an authentication error.
    /// </summary>
    public class GeoIP2AuthenticationException : GeoIP2Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoIP2AuthenticationException"/> class.
        /// </summary>
        /// <param name="message">A message explaining the cause of the error.</param>
        public GeoIP2AuthenticationException(string message)
            : base(message)
        {

        }
    }
}
