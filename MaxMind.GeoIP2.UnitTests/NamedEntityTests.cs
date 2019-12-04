#region

using System.Collections.Generic;
using MaxMind.GeoIP2.Model;
using Xunit;

#endregion

namespace MaxMind.GeoIP2.UnitTests
{
    public class NamedEntityTests
    {
        [Fact]
        public void CanGetSingleName()
        {
            var c = new City(
                names: new Dictionary<string, string> { { "en", "Foo" } },
                locales: new List<string> { "en" }
                );

            Assert.Equal("Foo", c.Name);
        }

        [Fact]
        public void NameReturnsCorrectLocale()
        {
            var c = new City(
                names: new Dictionary<string, string> { { "en", "Mexico City" }, { "es", "Ciudad de México" } },
                locales: new List<string> { "es" }
                );

            Assert.Equal("Ciudad de México", c.Name);
        }
    }
}