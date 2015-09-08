#region

using MaxMind.GeoIP2.Model;
using MaxMind.GeoIP2.Responses;
using NUnit.Framework;

#endregion

namespace MaxMind.GeoIP2.UnitTests
{
    [TestFixture]
    public class ResponseTests
    {
        [Test]
        public void InsightsConstruction()
        {
            var city = new City();
            var insightsReponse = new InsightsResponse(city);

            Assert.AreEqual(insightsReponse.City, city);
        }
    }
}