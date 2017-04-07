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
            var c = new City
            {
                Locales = new List<string> {"en"},
                Names = new Dictionary<string, string> {{"en", "Foo"}}
            };

            Assert.Equal("Foo", c.Name);
        }

        [Fact]
        public void NameReturnsCorrectLocale()
        {
            var c = new City
            {
                Locales = new List<string> {"es"},
                Names = new Dictionary<string, string> {{"en", "Mexico City"}, {"es", "Ciudad de México"}}
            };

            Assert.Equal("Ciudad de México", c.Name);
        }
    }
}