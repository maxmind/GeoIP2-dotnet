using System.Collections.Generic;
using MaxMind.GeoIP2.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeoIP2.UnitTests
{
    [TestClass]
    public class NamedEntityTests
    {

        [TestMethod]
        public void CanGetSingleName()
        {
            var c = new City();
            c.Languages = new List<string>{"en"};
            c.Names = new Dictionary<string, string>{{"en", "Foo"}};

            Assert.AreEqual("Foo", c.Name);
        }

        [TestMethod]
        public void NameReturnsCorrectLanguage()
        {
            var c = new City();
            c.Languages = new List<string>{"es"};
            c.Names = new Dictionary<string, string>{{"en", "Mexico City"}, {"es", "Ciudad de México"}};

            Assert.AreEqual("Ciudad de México", c.Name);
        }
    }
}