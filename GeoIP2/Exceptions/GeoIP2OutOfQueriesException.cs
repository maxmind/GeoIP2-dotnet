using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaxMind.GeoIP2.Exceptions
{
    public class GeoIP2OutOfQueriesException : GeoIP2Exception
    {
        public GeoIP2OutOfQueriesException(string message) : base(message)
        {
            
        }
    }
}
