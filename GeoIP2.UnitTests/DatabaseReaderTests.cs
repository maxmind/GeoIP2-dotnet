#region

using MaxMind.Db;
using MaxMind.GeoIP2.Exceptions;
using MaxMind.GeoIP2.Responses;
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
        private readonly string _anonymousIpDatabaseFile;
        private readonly string _cityDatabaseFile;
        private readonly string _connectionTypeDatabaseFile;
        private readonly string _countryDatabaseFile;
        private readonly string _domainDatabaseFile;
        private readonly string _enterpriseDatabaseFile;
        private readonly string _ispDatabaseFile;

        public DatabaseReaderTests()
        {
            var databaseDir = Path.Combine(TestUtils.TestDirectory, "TestData", "MaxMind-DB", "test-data");

            _anonymousIpDatabaseFile = Path.Combine(databaseDir, "GeoIP2-Anonymous-IP-Test.mmdb");
            _cityDatabaseFile = Path.Combine(databaseDir, "GeoIP2-City-Test.mmdb");
            _connectionTypeDatabaseFile = Path.Combine(databaseDir, "GeoIP2-Connection-Type-Test.mmdb");
            _countryDatabaseFile = Path.Combine(databaseDir, "GeoIP2-Country-Test.mmdb");
            _domainDatabaseFile = Path.Combine(databaseDir, "GeoIP2-Domain-Test.mmdb");
            _enterpriseDatabaseFile = Path.Combine(databaseDir, "GeoIP2-Enterprise-Test.mmdb");
            _ispDatabaseFile = Path.Combine(databaseDir, "GeoIP2-ISP-Test.mmdb");
        }

        [Test]
        public void DatabaseReader_HasDatabaseMetadata()
        {
            using (var reader = new DatabaseReader(_domainDatabaseFile))
            {
                Assert.That(reader.Metadata.DatabaseType, Is.EqualTo("GeoIP2-Domain"));
            }
        }

        [Test]
        public void DatabaseReaderInMemoryMode_ValidResponse()
        {
            using (var reader = new DatabaseReader(_cityDatabaseFile, FileAccessMode.Memory))
            {
                var response = reader.City("81.2.69.160");
                Assert.That(response.City.Name, Is.EqualTo("London"));
            }
        }

        [Test]
        public void DatabaseReaderWithStreamConstructor_ValidResponse()
        {
            using (var streamReader = File.OpenText(_cityDatabaseFile))
            {
                using (var reader = new DatabaseReader(streamReader.BaseStream))
                {
                    var response = reader.City("81.2.69.160");
                    Assert.That(response.City.Name, Is.EqualTo("London"));
                }
            }
        }

        [Test]
        public void InvalidCountryMethodForCityDatabase_ExceptionThrown()
        {
            using (var reader = new DatabaseReader(_cityDatabaseFile))
            {
                Assert.Throws(Is.TypeOf<InvalidOperationException>()
#if !NETCOREAPP1_0
                    .And.Message.Contains("A GeoIP2-City database cannot be opened with the Country method"),
#else
                    .And.Message.Contains("A GeoIP2-City database cannot be opened with the given method"),
#endif
                    () => reader.Country("10.10.10.10"));
            }
        }

        [Test]
        public void AnonymousIP_ValidResponse()
        {
            using (var reader = new DatabaseReader(_anonymousIpDatabaseFile))
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
        public void ConnectionType_ValidResponse()
        {
            using (var reader = new DatabaseReader(_connectionTypeDatabaseFile))
            {
                var ipAddress = "1.0.1.0";
                var response = reader.ConnectionType(ipAddress);
                Assert.That(response.ConnectionType, Is.EqualTo("Cable/DSL"));
                Assert.That(response.IPAddress, Is.EqualTo(ipAddress));
            }
        }

        [Test]
        public void Domain_ValidResponse()
        {
            using (var reader = new DatabaseReader(_domainDatabaseFile))
            {
                var ipAddress = "1.2.0.0";
                var response = reader.Domain(ipAddress);
                Assert.That(response.Domain, Is.EqualTo("maxmind.com"));
                Assert.That(response.IPAddress, Is.EqualTo(ipAddress));
            }
        }

        [Test]
        public void Enterprise_ValidResponse()
        {
            using (var reader = new DatabaseReader(_enterpriseDatabaseFile))
            {
                var ipAddress = "74.209.24.0";
                var response = reader.Enterprise(ipAddress);
                Assert.That(response.City.Confidence, Is.EqualTo(11));
                Assert.That(response.Country.Confidence, Is.EqualTo(99));
                Assert.That(response.Country.GeoNameId, Is.EqualTo(6252001));
                Assert.That(response.Location.AccuracyRadius, Is.EqualTo(27));
                Assert.That(response.Traits.ConnectionType, Is.EqualTo("Cable/DSL"));
                Assert.IsTrue(response.Traits.IsLegitimateProxy);
                Assert.That(response.Traits.IPAddress, Is.EqualTo(ipAddress));
            }
        }

        [Test]
        public void Isp_ValidResponse()
        {
            using (var reader = new DatabaseReader(_ispDatabaseFile))
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
        public void Country_ValidResponse()
        {
            using (var reader = new DatabaseReader(_countryDatabaseFile))
            {
                var response = reader.Country("81.2.69.160");
                Assert.That(response.Country.IsoCode, Is.EqualTo("GB"));
            }
        }

        [Test]
        public void CountryWithIPAddressClass_ValidResponse()
        {
            using (var reader = new DatabaseReader(_countryDatabaseFile))
            {
                var response = reader.Country(IPAddress.Parse("81.2.69.160"));
                Assert.That(response.Country.IsoCode, Is.EqualTo("GB"));
            }
        }

        [Test]
        public void City_ValidResponse()
        {
            using (var reader = new DatabaseReader(_cityDatabaseFile))
            {
                var response = reader.City("81.2.69.160");
                Assert.That(response.City.Name, Is.EqualTo("London"));
                Assert.That(response.Location.AccuracyRadius, Is.EqualTo(100));
            }
        }

        [Test]
        public void TryCity_ValidResponse()
        {
            using (var reader = new DatabaseReader(_cityDatabaseFile))
            {
                CityResponse response;
                var lookupSuccess = reader.TryCity("81.2.69.160", out response);
                Assert.IsTrue(lookupSuccess);
                Assert.That(response.City.Name, Is.EqualTo("London"));
            }
        }

        [Test]
        public void City_ResponseHasIPAddress()
        {
            using (var reader = new DatabaseReader(_cityDatabaseFile))
            {
                var response = reader.City("81.2.69.160");
                Assert.That(response.Traits.IPAddress, Is.EqualTo("81.2.69.160"));
            }
        }

        [Test]
        public void CityWithIPAddressClass_ValidResponse()
        {
            using (var reader = new DatabaseReader(_cityDatabaseFile))
            {
                var response = reader.City(IPAddress.Parse("81.2.69.160"));
                Assert.That(response.City.Name, Is.EqualTo("London"));
            }
        }

        [Test]
        public void CityWithDefaultLocale_ValidResponse()
        {
            using (var reader = new DatabaseReader(_cityDatabaseFile))
            {
                var response = reader.City("81.2.69.160");
                Assert.That(response.City.Name, Is.EqualTo("London"));
            }
        }

        [Test]
        public void CityWithLocaleList_ValidResponse()
        {
            using (
                var reader = new DatabaseReader(_cityDatabaseFile, new List<string> { "xx", "ru", "pt-BR", "es", "en" }))
            {
                var response = reader.City("81.2.69.160");
                Assert.That(response.City.Name, Is.EqualTo("Лондон"));
            }
        }

        [Test]
        public void CityWithUnknownAddress_ExceptionThrown()
        {
            using (var reader = new DatabaseReader(_cityDatabaseFile))
            {
                Assert.Throws(Is.TypeOf<AddressNotFoundException>()
                    .And.Message.Contains("10.10.10.10 is not in the database"),
                    () => reader.City("10.10.10.10"));
            }
        }

        [Test]
        public void TryCityUnknownAddress_False()
        {
            using (var reader = new DatabaseReader(_cityDatabaseFile))
            {
                CityResponse response;
                var status = reader.TryCity("10.10.10.10", out response);
                Assert.IsFalse(status);
                Assert.IsNull(response);
            }
        }
    }
}
