using MaxMind.GeoIP2.Model;
using System.Collections.Generic;
using Xunit;

namespace MaxMind.GeoIP2.UnitTests
{
    public class NamedEntityTests
    {
        [Fact]
        public void CanGetSingleName()
        {
            var c = new City
            {
                Names = new Dictionary<string, string> { { "en", "Foo" } },
                Locales = new List<string> { "en" }
            };

            Assert.Equal("Foo", c.Name);
        }

        [Fact]
        public void NameReturnsCorrectLocale()
        {
            var c = new City
            {
                Names = new Dictionary<string, string> { { "en", "Mexico City" }, { "es", "Ciudad de México" } },
                Locales = new List<string> { "es" }
            };

            Assert.Equal("Ciudad de México", c.Name);
        }
    }
}
