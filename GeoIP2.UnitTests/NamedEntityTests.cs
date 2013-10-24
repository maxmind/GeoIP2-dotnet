using System.Collections.Generic;
using MaxMind.GeoIP2.Model;
using NUnit.Framework;

namespace MaxMind.GeoIP2.UnitTests
{
    [TestFixture]
    public class NamedEntityTests
    {

        [Test]
        public void CanGetSingleName()
        {
            var c = new City();
            c.Locales = new List<string> { "en" };
            c.Names = new Dictionary<string, string> { { "en", "Foo" } };

            Assert.AreEqual("Foo", c.Name);
        }

        [Test]
        public void NameReturnsCorrectLocale()
        {
            var c = new City();
            c.Locales = new List<string> { "es" };
            c.Names = new Dictionary<string, string> { { "en", "Mexico City" }, { "es", "Ciudad de México" } };

            Assert.AreEqual("Ciudad de México", c.Name);
        }
    }
}