#region

using MaxMind.GeoIP2.Model;
using NUnit.Framework;

#endregion

namespace MaxMind.GeoIP2.UnitTests.Model
{
    [TestFixture]
    public class LocationTests
    {
        [Test]
        [TestCase(null, null)]
        [TestCase(50.0, null)]
        [TestCase(null, 0.0)]
        public void HasCoordinatesFailure(double? latitude, double? longitude)
        {
            var location = new Location
                (latitude: latitude, longitude: longitude);

            Assert.IsFalse(location.HasCoordinates);
        }

        [Test]
        public void HasCoordinatesSuccess()
        {
            var location = new Location(latitude: 50.0, longitude: 0.0);
            Assert.IsTrue(location.HasCoordinates);
        }
    }
}