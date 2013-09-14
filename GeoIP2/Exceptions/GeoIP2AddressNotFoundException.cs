using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MaxMind.GeoIP2.Exceptions
{
    public class GeoIP2AddressNotFoundException : GeoIP2Exception
    {
        public GeoIP2AddressNotFoundException(string message) : base(message)
        {
            
        }
    }
}
