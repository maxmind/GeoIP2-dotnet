using System.Collections.Generic;
using System.IO;
using MaxMind.Db;
using MaxMind.GeoIP2.Exceptions;
using NUnit.Framework;

namespace MaxMind.GeoIP2.UnitTests
{
    [TestFixture]
    public class DatabaseReaderTests
    {
        private readonly string _databaseDir;
        private readonly string _databaseFile;

        public DatabaseReaderTests()
        {
            _databaseDir = Path.Combine("..", "..", "TestData", "MaxMind-DB", "test-data");
            _databaseFile = Path.Combine(_databaseDir, "GeoIP2-City-Test.mmdb");
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
            using (var reader = new DatabaseReader(_databaseFile, new List<string> { "xx", "ru", "pt-BR", "es", "en" }))
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
        public void TestStreamConstructor()
        {
            using (StreamReader streamReader = new StreamReader(_databaseFile))
            {
                using (var reader = new DatabaseReader(streamReader.BaseStream))
                {
                    var resp = reader.City("81.2.69.160");
                    Assert.That(resp.City.Name, Is.EqualTo("London"));
                }
            }
        }

        [Test]
        public void HasIPAddress()
        {
            using (var reader = new DatabaseReader(_databaseFile))
            {
                var resp = reader.CityIspOrg("81.2.69.160");
                Assert.That(resp.Traits.IPAddress, Is.EqualTo("81.2.69.160"));
            }
        }

        [Test]
        [ExpectedException(typeof(AddressNotFoundException), ExpectedMessage = "10.10.10.10 is not in the database", MatchType = MessageMatch.Contains)]
        public void UnknownAddress()
        {
            using (var reader = new DatabaseReader(_databaseFile))
            {
                reader.City("10.10.10.10");
            }
        }

        [Test]
        public void ConnectionType()
        {
            using (var reader = new DatabaseReader(Path.Combine(_databaseDir, "GeoIP2-Connection-Type-Test.mmdb")))
            {
                var ipAddress = "1.0.1.0";

                var response = reader.ConnectionType(ipAddress);
                // XXX -enum?
                Assert.That(response.ConnectionType, Is.EqualTo("Cable/DSL"));
                Assert.That(response.IPAddress, Is.EqualTo(ipAddress));

            }
        }

        [Test]
        public void Domain()
        {
            using (var reader = new DatabaseReader(Path.Combine(_databaseDir, "GeoIP2-Domain-Test.mmdb")))
            {
                var ipAddress = "1.2.0.0";
                var response = reader.Domain(ipAddress);
                Assert.That(response.Domain, Is.EqualTo("maxmind.com"));
                Assert.That(response.IPAddress, Is.EqualTo(ipAddress));
            }
        }

        [Test]
        public void Isp()
        {
            using (var reader = new DatabaseReader(Path.Combine(_databaseDir, "GeoIP2-ISP-Org-Test.mmdb")))
            {
                var ipAddress = "2001:1700::";
                var response = reader.Isp(ipAddress);
                Assert.That(response.AutonomousSystemNumber, Is.EqualTo(6730));
                Assert.That(response.AutonomousSystemOrganization, Is.EqualTo("Sunrise Communications AG"));

                // XXX - Add org/isp when available

                Assert.That(response.IPAddress, Is.EqualTo(ipAddress));

            }
        }
    }
}