#region

using MaxMind.Db;
using MaxMind.GeoIP2.Responses;
using Newtonsoft.Json;
using NUnit.Framework;
using System.IO;
using System.Net;
using System.Reflection;
using static MaxMind.GeoIP2.UnitTests.ResponseHelper;

#endregion

namespace MaxMind.GeoIP2.UnitTests
{
    [TestFixture]
    public class DeserializationTests
    {
        public void CanDeserializeCountryResponse(CountryResponse resp)
        {
            Assert.That(resp.Continent.Code, Is.EqualTo("NA"));
            Assert.That(resp.Continent.GeoNameId, Is.EqualTo(42));
            Assert.That(resp.Continent.Name, Is.EqualTo("North America"));

            Assert.That(resp.Country.GeoNameId, Is.EqualTo(1));
            Assert.That(resp.Country.IsoCode, Is.EqualTo("US"));
            Assert.That(resp.Country.Confidence, Is.EqualTo(56));
            Assert.That(resp.Country.Name, Is.EqualTo("United States"));

            Assert.That(resp.RegisteredCountry.GeoNameId, Is.EqualTo(2));
            Assert.That(resp.RegisteredCountry.IsoCode, Is.EqualTo("CA"));
            Assert.That(resp.RegisteredCountry.Name, Is.EqualTo("Canada"));

            Assert.That(resp.RepresentedCountry.GeoNameId, Is.EqualTo(4));
            Assert.That(resp.RepresentedCountry.IsoCode, Is.EqualTo("GB"));
            Assert.That(resp.RepresentedCountry.Name, Is.EqualTo("United Kingdom"));
            Assert.That(resp.RepresentedCountry.Type, Is.EqualTo("military"));

            Assert.That(resp.Traits.IPAddress, Is.EqualTo("1.2.3.4"));
        }

        public void CanDeserializeInsightsResponse(InsightsResponse insights)
        {
            Assert.AreEqual(76, insights.City.Confidence);
            Assert.AreEqual(9876, insights.City.GeoNameId);
            Assert.AreEqual("Minneapolis", insights.City.Name);

            Assert.AreEqual("NA", insights.Continent.Code);
            Assert.AreEqual(42, insights.Continent.GeoNameId);
            Assert.AreEqual("North America", insights.Continent.Name);

            Assert.AreEqual(99, insights.Country.Confidence);
            Assert.AreEqual(1, insights.Country.GeoNameId);
            Assert.AreEqual("US", insights.Country.IsoCode);
            Assert.AreEqual("United States of America", insights.Country.Name);

            Assert.AreEqual(1500, insights.Location.AccuracyRadius);
            Assert.AreEqual(44.979999999999997, insights.Location.Latitude);
            Assert.AreEqual(93.263599999999997, insights.Location.Longitude);
            Assert.AreEqual(765, insights.Location.MetroCode);
            Assert.AreEqual("America/Chicago", insights.Location.TimeZone);
            Assert.AreEqual(50000, insights.Location.AverageIncome);
            Assert.AreEqual(100, insights.Location.PopulationDensity);

            Assert.AreEqual(11, insights.MaxMind.QueriesRemaining);

            Assert.AreEqual("55401", insights.Postal.Code);
            Assert.AreEqual(33, insights.Postal.Confidence);

            Assert.AreEqual(2, insights.RegisteredCountry.GeoNameId);
            Assert.AreEqual("CA", insights.RegisteredCountry.IsoCode);
            Assert.AreEqual("Canada", insights.RegisteredCountry.Name);

            Assert.AreEqual(3, insights.RepresentedCountry.GeoNameId);
            Assert.AreEqual("GB", insights.RepresentedCountry.IsoCode);
            Assert.AreEqual("United Kingdom", insights.RepresentedCountry.Name);
            Assert.AreEqual("military", insights.RepresentedCountry.Type);

            Assert.AreEqual(2, insights.Subdivisions.Count);
            Assert.AreEqual(88, insights.Subdivisions[0].Confidence);
            Assert.AreEqual(574635, insights.Subdivisions[0].GeoNameId);
            Assert.AreEqual("MN", insights.Subdivisions[0].IsoCode);
            Assert.AreEqual("Minnesota", insights.Subdivisions[0].Name);
            Assert.AreEqual("TT", insights.Subdivisions[1].IsoCode);

            Assert.AreEqual(1234, insights.Traits.AutonomousSystemNumber);
            Assert.AreEqual("AS Organization", insights.Traits.AutonomousSystemOrganization);
            Assert.AreEqual("example.com", insights.Traits.Domain);
            Assert.AreEqual("1.2.3.4", insights.Traits.IPAddress);
#pragma warning disable 0618
            Assert.AreEqual(true, insights.Traits.IsAnonymousProxy);
            Assert.AreEqual(true, insights.Traits.IsSatelliteProvider);
#pragma warning restore 0618
            Assert.AreEqual("Comcast", insights.Traits.Isp);
            Assert.AreEqual("Blorg", insights.Traits.Organization);
            Assert.AreEqual("college", insights.Traits.UserType);
        }

        [Test]
        public void CanDeserializeCountryResponseNewtonsoftJson()
        {
            CanDeserializeCountryResponse(JsonConvert.DeserializeObject<CountryResponse>(CountryJson));
        }

        // XXX - not sure this tests anything new now.
        [Test]
        public void CanDeserializeFromDatabaseType()
        {
            var reader =
                new DatabaseReader(Path.Combine(Program.CurrentDirectory, "TestData", "MaxMind-DB", "test-data", "GeoIP2-City-Test.mmdb"));

            var response = reader.City(IPAddress.Parse("216.160.83.56"));

            Assert.That(response.City.GeoNameId, Is.EqualTo(5803556));
            Assert.That(response.City.Name, Is.EqualTo("Milton"));

            Assert.That(response.Continent.Code, Is.EqualTo("NA"));
            Assert.That(response.Continent.GeoNameId, Is.EqualTo(6255149));
            Assert.That(response.Continent.Name, Is.EqualTo("North America"));

            Assert.That(response.Country.GeoNameId, Is.EqualTo(6252001));
            Assert.That(response.Country.IsoCode, Is.EqualTo("US"));
            Assert.That(response.Country.Name, Is.EqualTo("United States"));

            Assert.That(response.Location.Latitude, Is.EqualTo(47.2513));
            Assert.That(response.Location.Longitude, Is.EqualTo(-122.3149));
            Assert.That(response.Location.MetroCode, Is.EqualTo(819));
            Assert.That(response.Location.TimeZone, Is.EqualTo("America/Los_Angeles"));

            Assert.That(response.Postal.Code, Is.EqualTo("98354"));

            Assert.That(response.RegisteredCountry.GeoNameId, Is.EqualTo(2635167));
            Assert.That(response.RegisteredCountry.IsoCode, Is.EqualTo("GB"));
            Assert.That(response.RegisteredCountry.Name, Is.EqualTo("United Kingdom"));

            Assert.That(response.Subdivisions[0].GeoNameId, Is.EqualTo(5815135));
            Assert.That(response.Subdivisions[0].IsoCode, Is.EqualTo("WA"));
            Assert.That(response.Subdivisions[0].Name, Is.EqualTo("Washington"));
        }

        [Test]
        public void CanDeserializeInsightsResponseNewtonsoftJson()
        {
            CanDeserializeInsightsResponse(JsonConvert.DeserializeObject<InsightsResponse>(InsightsJson));
        }
    }
}