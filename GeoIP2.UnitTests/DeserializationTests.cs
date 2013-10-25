using System;
using System.Collections.Generic;
using System.IO;
using MaxMind.Db;
using MaxMind.GeoIP2.Responses;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using RestSharp.Deserializers;

namespace MaxMind.GeoIP2.UnitTests
{
    [TestFixture]
    public class DeserializationTests
    {
        private string _omniBody = "{" + "\"city\":{"
            + "\"confidence\":76," + "\"geoname_id\":9876," + "\"names\":{"
            + "\"en\":\"Minneapolis\"" + "}" + "}," + "\"continent\":{"
            + "\"code\":\"NA\"," + "\"geoname_id\":42," + "\"names\":{"
            + "\"en\":\"North America\"" + "}" + "}," + "\"country\":{"
            + "\"confidence\":99," + "\"iso_code\":\"US\","
            + "\"geoname_id\":1," + "\"names\":{"
            + "\"en\":\"United States of America\"" + "}" + "},"
            + "\"location\":{" + "\"accuracy_radius\":1500,"
            + "\"latitude\":44.98," + "\"longitude\":93.2636,"
            + "\"metro_code\":765," + "\"time_zone\":\"America/Chicago\""
            + "}," + "\"postal\":{\"confidence\": 33, \"code\":\"55401\"},"
            + "\"registered_country\":{" + "\"geoname_id\":2,"
            + "\"iso_code\":\"CA\"," + "\"names\":{" + "\"en\":\"Canada\""
            + "}" + "}," + "\"represented_country\":{" + "\"geoname_id\":3,"
            + "\"iso_code\":\"GB\"," + "\"names\":{"
            + "\"en\":\"United Kingdom\"" + "}," + "\"type\":\"C<military>\""
            + "}," + "\"subdivisions\":[{" + "\"confidence\":88,"
            + "\"geoname_id\":574635," + "\"iso_code\":\"MN\"," + "\"names\":{"
            + "\"en\":\"Minnesota\"" + "}" + "}," + "{\"iso_code\":\"TT\"}],"
            + "\"traits\":{" + "\"autonomous_system_number\":1234,"
            + "\"autonomous_system_organization\":\"AS Organization\","
            + "\"domain\":\"example.com\"," + "\"ip_address\":\"1.2.3.4\","
            + "\"is_anonymous_proxy\":true,"
            + "\"is_satellite_provider\":true," + "\"isp\":\"Comcast\","
            + "\"organization\":\"Blorg\"," + "\"user_type\":\"college\""
            + "}," + "\"maxmind\":{\"queries_remaining\":11}" + "}";


        private static string _countryBody = "{\"continent\":{"
            + "\"code\":\"NA\"," + "\"geoname_id\":42,"
            + "\"names\":{\"en\":\"North America\"}" + "}," + "\"country\":{"
            + "\"geoname_id\":1," + "\"iso_code\":\"US\","
            + "\"confidence\":56," + "\"names\":{\"en\":\"United States\"}"
            + "}," + "\"registered_country\":{" + "\"geoname_id\":2,"
            + "\"iso_code\":\"CA\"," + "\"names\":{\"en\":\"Canada\"}},"
            + "\"represented_country\":{" + "\"geoname_id\":4,"
            + "\"iso_code\":\"GB\"," + "\"names\":{\"en\":\"United Kingdom\"},"
            + "\"type\":\"military\"}," + "\"traits\":{"
            + "\"ip_address\":\"1.2.3.4\"" + "}}";

        [Test]
        public void CanDeserializeCountryResponseRestSharp()
        {
            var d = new JsonDeserializer();
            var r = new RestResponse();
            r.Content = _countryBody;
            CanDeserializeCountryResponse(d.Deserialize<CountryResponse>(r));
        }

        [Test]
        public void CanDeserializeOmniResponseRestSharp()
        {
            var d = new JsonDeserializer();
            var r = new RestResponse();
            r.Content = _omniBody;
            CanDeserializeOmniResponse(d.Deserialize<OmniResponse>(r));
        }

        [Test]
        public void CanDeserializeCountryResponseNewtonsoftJson()
        {
            CanDeserializeCountryResponse(JsonConvert.DeserializeObject<CountryResponse>(_countryBody));
        }

        [Test]
        public void CanDeserializeOmniResponseNewtonsoftJson()
        {
            CanDeserializeOmniResponse(JsonConvert.DeserializeObject<OmniResponse>(_omniBody));
        }

        public void CanDeserializeCountryResponse(CountryResponse resp)
        {
            resp.SetLocales(new List<string> { "en" });

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

        public void CanDeserializeOmniResponse(OmniResponse omni)
        {
            omni.SetLocales(new List<string> { "en" });

            Assert.AreEqual(76, omni.City.Confidence);
            Assert.AreEqual(9876, omni.City.GeoNameId);
            Assert.AreEqual("Minneapolis", omni.City.Name);

            Assert.AreEqual("NA", omni.Continent.Code);
            Assert.AreEqual(42, omni.Continent.GeoNameId);
            Assert.AreEqual("North America", omni.Continent.Name);

            Assert.AreEqual(99, omni.Country.Confidence);
            Assert.AreEqual(1, omni.Country.GeoNameId);
            Assert.AreEqual("US", omni.Country.IsoCode);
            Assert.AreEqual("United States of America", omni.Country.Name);

            Assert.AreEqual(1500, omni.Location.AccuracyRadius);
            Assert.AreEqual(44.979999999999997, omni.Location.Latitude);
            Assert.AreEqual(93.263599999999997, omni.Location.Longitude);
            Assert.AreEqual(765, omni.Location.MetroCode);
            Assert.AreEqual("America/Chicago", omni.Location.TimeZone);

            Assert.AreEqual(11, omni.MaxMind.QueriesRemaining);

            Assert.AreEqual("55401", omni.Postal.Code);
            Assert.AreEqual(33, omni.Postal.Confidence);

            Assert.AreEqual(2, omni.RegisteredCountry.GeoNameId);
            Assert.AreEqual("CA", omni.RegisteredCountry.IsoCode);
            Assert.AreEqual("Canada", omni.RegisteredCountry.Name);

            Assert.AreEqual(3, omni.RepresentedCountry.GeoNameId);
            Assert.AreEqual("GB", omni.RepresentedCountry.IsoCode);
            Assert.AreEqual("United Kingdom", omni.RepresentedCountry.Name);
            Assert.AreEqual("C<military>", omni.RepresentedCountry.Type);

            Assert.AreEqual(2, omni.Subdivisions.Count);
            omni.Subdivisions[0].Locales = new List<string> { "en" };
            Assert.AreEqual(88, omni.Subdivisions[0].Confidence);
            Assert.AreEqual(574635, omni.Subdivisions[0].GeoNameId);
            Assert.AreEqual("MN", omni.Subdivisions[0].IsoCode);
            Assert.AreEqual("Minnesota", omni.Subdivisions[0].Name);
            Assert.AreEqual("TT", omni.Subdivisions[1].IsoCode);

            Assert.AreEqual(1234, omni.Traits.AutonomousSystemNumber);
            Assert.AreEqual("AS Organization", omni.Traits.AutonomousSystemOrganization);
            Assert.AreEqual("example.com", omni.Traits.Domain);
            Assert.AreEqual("1.2.3.4", omni.Traits.IPAddress);
            Assert.AreEqual(true, omni.Traits.IsAnonymousProxy);
            Assert.AreEqual(true, omni.Traits.IsSatelliteProvider);
            Assert.AreEqual("Comcast", omni.Traits.Isp);
            Assert.AreEqual("Blorg", omni.Traits.Organization);
            Assert.AreEqual("college", omni.Traits.UserType);
        }

        [Test]
        public void CanDeserializeFromDatabaseJToken()
        {
            var reader = new Reader(Path.Combine("..", "..", "TestData", "GeoLite2-City.mmdb"));

            var obj = reader.Find("74.125.227.161");
            var response = obj.ToObject<OmniResponse>();
            response.SetLocales(new List<string> { "en" });

            Assert.That(response.City.GeoNameId, Is.EqualTo(5375480));
            Assert.That(response.City.Name, Is.EqualTo("Mountain View"));

            Assert.That(response.Continent.Code, Is.EqualTo("NA"));
            Assert.That(response.Continent.GeoNameId, Is.EqualTo(6255149));
            Assert.That(response.Continent.Name, Is.EqualTo("North America"));

            Assert.That(response.Country.GeoNameId, Is.EqualTo(6252001));
            Assert.That(response.Country.IsoCode, Is.EqualTo("US"));
            Assert.That(response.Country.Name, Is.EqualTo("United States"));

            Assert.That(response.Location.Latitude, Is.EqualTo(37.419200000000004));
            Assert.That(response.Location.Longitude, Is.EqualTo(-122.0574));
            Assert.That(response.Location.MetroCode, Is.EqualTo(807));
            Assert.That(response.Location.TimeZone, Is.EqualTo("America/Los_Angeles"));

            Assert.That(response.Postal.Code, Is.EqualTo("94043"));

            Assert.That(response.RegisteredCountry.GeoNameId, Is.EqualTo(6252001));
            Assert.That(response.RegisteredCountry.IsoCode, Is.EqualTo("US"));
            Assert.That(response.RegisteredCountry.Name, Is.EqualTo("United States"));

            Assert.That(response.Subdivisions[0].GeoNameId, Is.EqualTo(5332921));
            Assert.That(response.Subdivisions[0].IsoCode, Is.EqualTo("CA"));
            Assert.That(response.Subdivisions[0].Name, Is.EqualTo("California"));
        }
    }
}
