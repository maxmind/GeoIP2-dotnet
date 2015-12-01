#region

using MaxMind.Db;
using MaxMind.GeoIP2.Exceptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;

#endregion

namespace MaxMind.GeoIP2.UnitTests
{
    [TestFixture]
    public class DatabaseReaderTests
    {
        private readonly string _databaseDir;
        private readonly string _databaseFile;

        public DatabaseReaderTests()
        {
            _databaseDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "..", "..", "TestData", "MaxMind-DB", "test-data");
            _databaseFile = Path.Combine(_databaseDir, "GeoIP2-City-Test.mmdb");
        }

        [Test]
        public void AnonymousIP()
        {
            using (var reader = new DatabaseReader(Path.Combine(_databaseDir, "GeoIP2-Anonymous-IP-Test.mmdb")))
            {
                var ipAddress = "1.2.0.1";

                var response = reader.AnonymousIP(ipAddress);

                Assert.That(response.IsAnonymous, Is.True);
                Assert.That(response.IsAnonymousVpn, Is.True);
                Assert.That(response.IsHostingProvider, Is.False);
                Assert.That(response.IsPublicProxy, Is.False);
                Assert.That(response.IsTorExitNode, Is.False);
                Assert.That(response.IPAddress, Is.EqualTo(ipAddress));
            }
        }

        [Test]
        public void ConnectionType()
        {
            using (var reader = new DatabaseReader(Path.Combine(_databaseDir, "GeoIP2-Connection-Type-Test.mmdb")))
            {
                var ipAddress = "1.0.1.0";

                var response = reader.ConnectionType(ipAddress);
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
        public void HasIPAddress()
        {
            using (var reader = new DatabaseReader(_databaseFile))
            {
                var resp = reader.City("81.2.69.160");
                Assert.That(resp.Traits.IPAddress, Is.EqualTo("81.2.69.160"));
            }
        }

        [Test]
        public void InvalidMethod()
        {
            using (var reader = new DatabaseReader(_databaseFile))
            {
                Assert.Throws(Is.TypeOf<InvalidOperationException>()
                    .And.Message.Contains("A GeoIP2-City database cannot be opened with the Country method"),
                    () => reader.Country("10.10.10.10"));
            }
        }

        [Test]
        public void Isp()
        {
            using (var reader = new DatabaseReader(Path.Combine(_databaseDir, "GeoIP2-ISP-Test.mmdb")))
            {
                var ipAddress = "1.128.0.0";
                var response = reader.Isp(ipAddress);
                Assert.That(response.AutonomousSystemNumber, Is.EqualTo(1221));
                Assert.That(response.AutonomousSystemOrganization, Is.EqualTo("Telstra Pty Ltd"));
                Assert.That(response.Isp, Is.EqualTo("Telstra Internet"));
                Assert.That(response.Organization, Is.EqualTo("Telstra Internet"));

                Assert.That(response.IPAddress, Is.EqualTo(ipAddress));
            }
        }

        [Test]
        public void Metadata()
        {
            using (var reader = new DatabaseReader(Path.Combine(_databaseDir, "GeoIP2-Domain-Test.mmdb")))
            {
                Assert.That(reader.Metadata.DatabaseType, Is.EqualTo("GeoIP2-Domain"));
            }
        }

        [Test]
        public void TestCountry()
        {
            using (var reader = new DatabaseReader(Path.Combine(_databaseDir, "GeoIP2-Country-Test.mmdb")))
            {
                var resp = reader.Country("81.2.69.160");
                Assert.That(resp.Country.IsoCode, Is.EqualTo("GB"));
            }
        }

        [Test]
        public void TestCountryWithIPAddress()
        {
            using (var reader = new DatabaseReader(Path.Combine(_databaseDir, "GeoIP2-Country-Test.mmdb")))
            {
                var resp = reader.Country(IPAddress.Parse("81.2.69.160"));
                Assert.That(resp.Country.IsoCode, Is.EqualTo("GB"));
            }
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
                var resp = reader.City("81.2.69.160");
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
            using (var streamReader = new StreamReader(_databaseFile))
            {
                using (var reader = new DatabaseReader(streamReader.BaseStream))
                {
                    var resp = reader.City("81.2.69.160");
                    Assert.That(resp.City.Name, Is.EqualTo("London"));
                }
            }
        }

        [Test]
        public void TestWithIPAddress()
        {
            using (var reader = new DatabaseReader(_databaseFile))
            {
                var resp = reader.City(IPAddress.Parse("81.2.69.160"));
                Assert.That(resp.City.Name, Is.EqualTo("London"));
            }
        }

        [Test]
        public void UnknownAddress()
        {
            using (var reader = new DatabaseReader(_databaseFile))
            {
                Assert.Throws(Is.TypeOf<AddressNotFoundException>()
                    .And.Message.Contains("10.10.10.10 is not in the database"),
                    () => reader.City("10.10.10.10"));
            }
        }
    }
}