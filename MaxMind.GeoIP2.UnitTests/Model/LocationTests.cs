#region

using MaxMind.GeoIP2.Model;
using Xunit;

#endregion

namespace MaxMind.GeoIP2.UnitTests.Model
{
    public class LocationTests
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData(50.0, null)]
        [InlineData(null, 0.0)]
        public void HasCoordinatesFailure(double? latitude, double? longitude)
        {
            var location = new Location
            {
                Latitude = latitude,
                Longitude = longitude
            };

            Assert.False(location.HasCoordinates);
        }

        [Fact]
        public void HasCoordinatesSuccess()
        {
            var location = new Location { Latitude = 50.0, Longitude = 0.0 };
            Assert.True(location.HasCoordinates);
        }
    }
}