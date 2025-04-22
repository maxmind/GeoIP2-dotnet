#region

using MaxMind.Db;
using MaxMind.GeoIP2.Exceptions;
using MaxMind.GeoIP2.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Xunit;

#endregion

namespace MaxMind.GeoIP2.UnitTests
{
    public class DatabaseReaderTests
    {
        private readonly string _anonymousIpDatabaseFile;
        private readonly string _anonymousPlusDatabaseFile;
        private readonly string _asnDatabaseFile;
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
            _anonymousPlusDatabaseFile = Path.Combine(databaseDir, "GeoIP-Anonymous-Plus-Test.mmdb");
            _asnDatabaseFile = Path.Combine(databaseDir, "GeoLite2-ASN-Test.mmdb");
            _cityDatabaseFile = Path.Combine(databaseDir, "GeoIP2-City-Test.mmdb");
            _connectionTypeDatabaseFile = Path.Combine(databaseDir, "GeoIP2-Connection-Type-Test.mmdb");
            _countryDatabaseFile = Path.Combine(databaseDir, "GeoIP2-Country-Test.mmdb");
            _domainDatabaseFile = Path.Combine(databaseDir, "GeoIP2-Domain-Test.mmdb");
            _enterpriseDatabaseFile = Path.Combine(databaseDir, "GeoIP2-Enterprise-Test.mmdb");
            _ispDatabaseFile = Path.Combine(databaseDir, "GeoIP2-ISP-Test.mmdb");
        }

        [Fact]
        public void DatabaseReader_HasDatabaseMetadata()
        {
            using var reader = new DatabaseReader(_domainDatabaseFile);
            Assert.Equal("GeoIP2-Domain", reader.Metadata.DatabaseType);
        }

        [Fact]
        public void DatabaseReaderInMemoryMode_ValidResponse()
        {
            using var reader = new DatabaseReader(_cityDatabaseFile, FileAccessMode.Memory);
            var response = reader.City("81.2.69.160");
            Assert.Equal("London", response.City.Name);
        }

        [Fact]
        public void DatabaseReaderWithStreamConstructor_ValidResponse()
        {
            using var streamReader = File.OpenText(_cityDatabaseFile);
            using var reader = new DatabaseReader(streamReader.BaseStream);
            var response = reader.City("81.2.69.160");
            Assert.Equal("London", response.City.Name);
        }

        [Fact]
        public void InvalidCountryMethodForCityDatabase_ExceptionThrown()
        {
            using var reader = new DatabaseReader(_cityDatabaseFile);
            var exception = Record.Exception(() => reader.Country("10.10.10.10"));
            Assert.NotNull(exception);
            Assert.Contains("A GeoIP2-City database cannot be opened with the", exception.Message);
            Assert.IsType<InvalidOperationException>(exception);
        }

        [Fact]
        public void AnonymousIP_ValidResponse()
        {
            using var reader = new DatabaseReader(_anonymousIpDatabaseFile);
            var ipAddress = "1.2.0.1";
            var response = reader.AnonymousIP(ipAddress);
            Assert.True(response.IsAnonymous);
            Assert.True(response.IsAnonymousVpn);
            Assert.False(response.IsHostingProvider);
            Assert.False(response.IsPublicProxy);
            Assert.False(response.IsResidentialProxy);
            Assert.False(response.IsTorExitNode);
            Assert.Equal(ipAddress, response.IPAddress);
            Assert.Equal("1.2.0.0/16", response.Network?.ToString());
        }

        [Fact]
        public void AnonymousIP_ValidResponseWithAllTrue()
        {
            using var reader = new DatabaseReader(_anonymousIpDatabaseFile);
            var ipAddress = "81.2.69.1";
            var response = reader.AnonymousIP(ipAddress);
            Assert.True(response.IsAnonymous);
            Assert.True(response.IsAnonymousVpn);
            Assert.True(response.IsHostingProvider);
            Assert.True(response.IsPublicProxy);
            Assert.True(response.IsResidentialProxy);
            Assert.True(response.IsTorExitNode);
            Assert.Equal(ipAddress, response.IPAddress);
            Assert.Equal("81.2.69.0/24", response.Network?.ToString());
        }

        [Fact]
        public void AnonymousPlus_ValidResponse()
        {
            using var reader = new DatabaseReader(_anonymousPlusDatabaseFile);
            var ipAddress = "1.2.0.1";
            var response = reader.AnonymousPlus(ipAddress);
            Assert.Equal(30, response.AnonymizerConfidence);
            Assert.True(response.IsAnonymous);
            Assert.True(response.IsAnonymousVpn);
            Assert.False(response.IsHostingProvider);
            Assert.False(response.IsPublicProxy);
            Assert.False(response.IsResidentialProxy);
            Assert.False(response.IsTorExitNode);
            Assert.Equal(ipAddress, response.IPAddress);
            Assert.Equal("1.2.0.1/32", response.Network?.ToString());
#if NET6_0_OR_GREATER
            Assert.Equal(new DateOnly(2025, 4, 14), response.NetworkLastSeen);
#endif
            Assert.Equal("foo", response.ProviderName);
        }

        [Fact]
        public void Asn_ValidResponse()
        {
            using var reader = new DatabaseReader(_asnDatabaseFile);
            var ipAddressStr = "1.128.0.0";
            var response = reader.Asn(ipAddressStr);
            CheckAsn(response, ipAddressStr);

            Assert.True(reader.TryAsn(ipAddressStr, out response!));
            CheckAsn(response, ipAddressStr);

            var ipAddress = IPAddress.Parse(ipAddressStr);
            response = reader.Asn(ipAddress);
            CheckAsn(response, ipAddressStr);

            Assert.True(reader.TryAsn(ipAddress, out response!));
            CheckAsn(response, ipAddressStr);
        }

        private static void CheckAsn(AsnResponse response, string ipAddress)
        {
            Assert.Equal(1221, response.AutonomousSystemNumber);
            Assert.Equal("Telstra Pty Ltd", response.AutonomousSystemOrganization);
            Assert.Equal(ipAddress, response.IPAddress);
            Assert.Equal("1.128.0.0/11", response.Network?.ToString());
        }

        [Fact]
        public void ConnectionType_ValidResponse()
        {
            using var reader = new DatabaseReader(_connectionTypeDatabaseFile);
            var ipAddress = "1.0.1.0";
            var response = reader.ConnectionType(ipAddress);
            Assert.Equal("Cellular", response.ConnectionType);
            Assert.Equal(ipAddress, response.IPAddress);
            Assert.Equal("1.0.1.0/24", response.Network?.ToString());
        }

        [Fact]
        public void Domain_ValidResponse()
        {
            using var reader = new DatabaseReader(_domainDatabaseFile);
            var ipAddress = "1.2.0.0";
            var response = reader.Domain(ipAddress);
            Assert.Equal("maxmind.com", response.Domain);
            Assert.Equal(ipAddress, response.IPAddress);
            Assert.Equal("1.2.0.0/16", response.Network?.ToString());
        }

        [Fact]
        public void Enterprise_ValidResponse()
        {
            using var reader = new DatabaseReader(_enterpriseDatabaseFile);
            var ipAddress = "74.209.24.0";
            var response = reader.Enterprise(ipAddress);
            Assert.Equal(11, response.City.Confidence);
            Assert.Equal(99, response.Country.Confidence);
            Assert.False(response.Country.IsInEuropeanUnion);
            Assert.Equal(6252001, response.Country.GeoNameId);
            Assert.Equal(27, response.Location.AccuracyRadius);
            Assert.False(response.RegisteredCountry.IsInEuropeanUnion);
            Assert.False(response.RepresentedCountry.IsInEuropeanUnion);
            Assert.Equal("Cable/DSL", response.Traits.ConnectionType);
            Assert.True(response.Traits.IsLegitimateProxy);
            Assert.Equal(ipAddress, response.Traits.IPAddress);
            Assert.Equal("74.209.16.0/20", response.Traits.Network?.ToString());

            response = reader.Enterprise("149.101.100.0");
            Assert.Equal("310", response.Traits.MobileCountryCode);
            Assert.Equal("004", response.Traits.MobileNetworkCode);

            response = reader.Enterprise("214.1.1.0");
            Assert.True(response.Traits.IsAnycast);
        }

        [Fact]
        public void Isp_ValidResponse()
        {
            using var reader = new DatabaseReader(_ispDatabaseFile);
            var ipAddress = "1.128.0.0";
            var response = reader.Isp(ipAddress);
            Assert.Equal(1221, response.AutonomousSystemNumber);
            Assert.Equal("Telstra Pty Ltd", response.AutonomousSystemOrganization);
            Assert.Equal("Telstra Internet", response.Isp);
            Assert.Equal("Telstra Internet", response.Organization);
            Assert.Equal(ipAddress, response.IPAddress);
            Assert.Equal("1.128.0.0/11", response.Network?.ToString());

            response = reader.Isp("149.101.100.0");
            Assert.Equal("310", response.MobileCountryCode);
            Assert.Equal("004", response.MobileNetworkCode);
        }

        [Fact]
        public void Country_ValidResponse()
        {
            using var reader = new DatabaseReader(_countryDatabaseFile);
            var response = reader.Country("81.2.69.160");
            Assert.Equal("GB", response.Country.IsoCode);
            Assert.False(response.Country.IsInEuropeanUnion);
            Assert.False(response.RegisteredCountry.IsInEuropeanUnion);
            Assert.False(response.RepresentedCountry.IsInEuropeanUnion);
            Assert.Equal("81.2.69.160/27", response.Traits.Network?.ToString());

            response = reader.Country("214.1.1.0");
            Assert.True(response.Traits.IsAnycast);
        }

        [Fact]
        public void CountryWithIPAddressClass_ValidResponse()
        {
            using var reader = new DatabaseReader(_countryDatabaseFile);
            var response = reader.Country(IPAddress.Parse("81.2.69.160"));
            Assert.Equal("GB", response.Country.IsoCode);
            Assert.False(response.Country.IsInEuropeanUnion);
            Assert.Equal("US", response.RegisteredCountry.IsoCode);
            Assert.False(response.RegisteredCountry.IsInEuropeanUnion);
            Assert.False(response.RepresentedCountry.IsInEuropeanUnion);
            Assert.Equal("81.2.69.160/27", response.Traits.Network?.ToString());
        }

        [Fact]
        public void City_ValidResponse()
        {
            using var reader = new DatabaseReader(_cityDatabaseFile);
            var response = reader.City("81.2.69.160");
            Assert.Equal("London", response.City.Name);
            Assert.False(response.Country.IsInEuropeanUnion);
            Assert.Equal(100, response.Location.AccuracyRadius);
            Assert.False(response.RegisteredCountry.IsInEuropeanUnion);
            Assert.False(response.RepresentedCountry.IsInEuropeanUnion);
            Assert.Equal("81.2.69.160/27", response.Traits.Network?.ToString());

            response = reader.City("214.1.1.0");
            Assert.True(response.Traits.IsAnycast);
        }

        [Fact]
        public void TryCity_ValidResponse()
        {
            using var reader = new DatabaseReader(_cityDatabaseFile);
            var lookupSuccess = reader.TryCity("81.2.69.160", out var response);
            Assert.True(lookupSuccess);
            Assert.False(response?.Country.IsInEuropeanUnion);
            Assert.Equal("London", response?.City.Name);
            Assert.False(response?.RegisteredCountry.IsInEuropeanUnion);
            Assert.False(response?.RepresentedCountry.IsInEuropeanUnion);
            Assert.Equal("81.2.69.160/27", response?.Traits.Network?.ToString());
        }

        [Fact]
        public void City_ResponseHasIPAddress()
        {
            using var reader = new DatabaseReader(_cityDatabaseFile);
            var response = reader.City("81.2.69.160");
            Assert.Equal("81.2.69.160", response.Traits.IPAddress);
        }

        [Fact]
        public void City_ManyFields()
        {
            using var reader = new DatabaseReader(_cityDatabaseFile);
            var response = reader.City(IPAddress.Parse("216.160.83.56"));

            Assert.Equal(5803556, response.City.GeoNameId);
            Assert.Equal("Milton", response.City.Name);

            Assert.Equal("NA", response.Continent.Code);
            Assert.Equal(6255149, response.Continent.GeoNameId);
            Assert.Equal("North America", response.Continent.Name);

            Assert.Equal(6252001, response.Country.GeoNameId);
            Assert.False(response.Country.IsInEuropeanUnion);
            Assert.Equal("US", response.Country.IsoCode);
            Assert.Equal("United States", response.Country.Name);

            Assert.Equal(47.2513, response.Location.Latitude);
            Assert.Equal(-122.3149, response.Location.Longitude);
#pragma warning disable 0618
            Assert.Equal(819, response.Location.MetroCode);
#pragma warning restore 0618
            Assert.Equal("America/Los_Angeles", response.Location.TimeZone);

            Assert.Equal("98354", response.Postal.Code);

            Assert.Equal(2635167, response.RegisteredCountry.GeoNameId);
            Assert.False(response.RegisteredCountry.IsInEuropeanUnion);
            Assert.Equal("GB", response.RegisteredCountry.IsoCode);
            Assert.Equal("United Kingdom", response.RegisteredCountry.Name);

            Assert.False(response.RepresentedCountry.IsInEuropeanUnion);

            Assert.Equal(5815135, response.Subdivisions[0].GeoNameId);
            Assert.Equal("WA", response.Subdivisions[0].IsoCode);
            Assert.Equal("Washington", response.Subdivisions[0].Name);
        }

        [Fact]
        public void CityWithLocaleList_ValidResponse()
        {
            using var reader = new DatabaseReader(_cityDatabaseFile, new List<string> { "xx", "ru", "pt-BR", "es", "en" });
            var response = reader.City("81.2.69.160");
            Assert.Equal("Лондон", response.City.Name);
        }

        [Fact]
        public void CityWithUnknownAddress_ExceptionThrown()
        {
            using var reader = new DatabaseReader(_cityDatabaseFile);
            var exception = Record.Exception(() => reader.City("10.10.10.10"));
            Assert.NotNull(exception);
            Assert.Contains("10.10.10.10 is not in the database", exception.Message);
            Assert.IsType<AddressNotFoundException>(exception);
        }

        [Fact]
        public void TryCityUnknownAddress_False()
        {
            using var reader = new DatabaseReader(_cityDatabaseFile);
            var status = reader.TryCity("10.10.10.10", out var response);
            Assert.False(status);
            Assert.Null(response);
        }
    }
}
