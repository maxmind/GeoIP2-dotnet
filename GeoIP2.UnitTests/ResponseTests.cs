using System.Collections.Generic;
using MaxMind.GeoIP2.Model;
using MaxMind.GeoIP2.Responses;
using NUnit.Framework;

namespace MaxMind.GeoIP2.UnitTests
{
    [TestFixture]
    public class ResponseTests
    {

        [Test]
        public void OmniConstruction()
        {
            var city = new City();
            var omniReponse = new OmniResponse(city: city);

            Assert.AreEqual(omniReponse.City, city);
        }

    }
}