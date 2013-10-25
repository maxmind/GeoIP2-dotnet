using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaxMind.GeoIP2.Exceptions
{
    /// <summary>
    /// This exception is thrown when your account does not have any queries remaining for the called service.
    /// </summary>
    public class OutOfQueriesException : GeoIP2Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutOfQueriesException"/> class.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public OutOfQueriesException(string message)
            : base(message)
        {

        }
    }
}
