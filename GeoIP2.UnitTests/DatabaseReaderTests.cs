using System.Collections.Generic;
using System.IO;
using MaxMind.DB;
using MaxMind.GeoIP2.Exceptions;
using NUnit.Framework;

namespace MaxMind.GeoIP2.UnitTests
{
    [TestFixture]
    public class DatabaseReaderTests
    {
        private readonly string _databaseFile;

        public DatabaseReaderTests()
        {
            _databaseFile = Path.Combine("..", "..", "TestData", "MaxMind-DB", "test-data", "GeoIP2-City-Test.mmdb");
        }

        [Test]
        public void TestDefaultLocale()
        {
            using (var reader = new DatabaseReader(_databaseFile))
            {
                var resp = reader.City("81.2.69.160");
                Assert.That(resp.City.Name, Is.EqualTo("London"));
            }
        }

        [Test]
        public void TestLocaleList()
        {
            using (var reader = new DatabaseReader(_databaseFile, new List<string> {"xx", "ru", "pt-BR", "es", "en"}))
            {
                var resp = reader.Omni("81.2.69.160");
                Assert.That(resp.City.Name, Is.EqualTo("Лондон"));
            }
        }

        [Test]
        public void TestMemoryMode()
        {
            using (var reader = new DatabaseReader(_databaseFile, FileAccessMode.Memory))
            {
                var resp = reader.City("81.2.69.160");
                Assert.That(resp.City.Name, Is.EqualTo("London"));
            }
        }

        [Test]
        public void HasIPAddress()
        {
            using (var reader = new DatabaseReader(_databaseFile))
            {
                var resp = reader.CityIspOrg("81.2.69.160");
                Assert.That(resp.Traits.IpAddress, Is.EqualTo("81.2.69.160"));
            }
        }

        [Test]
        [ExpectedException(typeof(GeoIP2AddressNotFoundException), ExpectedMessage = "10.10.10.10 is not in the database", MatchType = MessageMatch.Contains)]
        public void UnknownAddress()
        {
            using (var reader = new DatabaseReader(_databaseFile))
            {
                reader.City("10.10.10.10");
            }
        }
    }
}