using MaxMind.GeoIP2.Model;
using MaxMind.GeoIP2.Responses;
using Xunit;

namespace MaxMind.GeoIP2.UnitTests
{
    public class ResponseTests
    {
        [Fact]
        public void InsightsConstruction()
        {
            var city = new City();
            var insightsReponse = new InsightsResponse { City = city };

            Assert.Equal(insightsReponse.City, city);
        }
    }
}
