using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaxMind.GeoIP2.Exceptions
{
    public class GeoIP2AuthenticationException : GeoIP2Exception
    {
        public GeoIP2AuthenticationException(string message) : base(message)
        {
            
        }
    }
}
