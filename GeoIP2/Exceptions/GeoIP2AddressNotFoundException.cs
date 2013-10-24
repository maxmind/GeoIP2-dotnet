using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MaxMind.GeoIP2.Exceptions
{
    /// <summary>
    /// This exception is thrown when the IP address is not found in the database.
    /// This generally means that the address was a private or reserved address.
    /// </summary>
    public class GeoIP2AddressNotFoundException : GeoIP2Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoIP2AddressNotFoundException"/> class.
        /// </summary>
        /// <param name="message">A message explaining the cause of the error.</param>
        public GeoIP2AddressNotFoundException(string message)
            : base(message)
        {

        }
    }
}
