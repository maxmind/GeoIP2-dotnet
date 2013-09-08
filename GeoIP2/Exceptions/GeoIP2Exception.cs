using System;

namespace MaxMind.GeoIP2.Exceptions
{
    public class GeoIP2Exception : ApplicationException
    {
        public GeoIP2Exception(string message) : base(message) {}

        public GeoIP2Exception(string message, Exception innerException) : base(message, innerException){}
    }
}