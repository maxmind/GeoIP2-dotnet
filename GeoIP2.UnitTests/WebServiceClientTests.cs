#region

using MaxMind.GeoIP2.Exceptions;
using MaxMind.GeoIP2.Http;
using MaxMind.GeoIP2.Model;
using MaxMind.GeoIP2.Responses;
using MaxMind.GeoIP2.UnitTests.Mock;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static MaxMind.GeoIP2.UnitTests.ResponseHelper;

#endregion

namespace MaxMind.GeoIP2.UnitTests
{
    [TestFixture]
    public class WebServiceClientTests
    {
        public delegate Task<AbstractCountryResponse> ClientRunner(WebServiceClient c, string ip = "1.2.3.4");

        // I don't love running the sync tests with async, but the alternative
        // seems to be a lot of code duplication.
        // "Async" added to the name so that Nunit can tell them apart.
        private static readonly object[][] TestCases =
        {
            new object[] {"country", (ClientRunner) (async (c, i) => c.Country(i)), typeof (CountryResponse)},
            new object[]
            {"countryAsync", (ClientRunner) (async (c, i) => await c.CountryAsync(i)), typeof (CountryResponse)},
            new object[] {"city", (ClientRunner) (async (c, i) => c.City(i)), typeof (CityResponse)},
            new object[] {"cityAsync", (ClientRunner) (async (c, i) => await c.CityAsync(i)), typeof (CityResponse)},
            new object[] {"insights", (ClientRunner) (async (c, i) => c.Insights(i)), typeof (InsightsResponse)},
            new object[]
            {"insightsAsync", (ClientRunner) (async (c, i) => await c.InsightsAsync(i)), typeof (InsightsResponse)}
        };

        private WebServiceClient CreateClient(string type, string ipAddress = "1.2.3.4",
            HttpStatusCode status = HttpStatusCode.OK, string contentType = null, string content = "")
        {
            var service = type.Replace("Async", "");
            if (contentType == null)
            {
                contentType = $"application/vnd.maxmind.com-{service}+json";
            }
            var stringContent = new StringContent(content);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            stringContent.Headers.Add("Content-Length", content.Length.ToString());
            var message = new HttpResponseMessage(status)
            {
                Content = stringContent
            };

            // HttpClient mock
            var uri = new Uri($"https://geoip.maxmind.com/geoip/v2.1/{service}/{ipAddress}");
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(HttpMethod.Get, uri.ToString())
                .WithHeaders("Accept", "application/json")
                .Respond(message);

            // HttpWebRequest mock
            var contentsBytes = Encoding.UTF8.GetBytes(content);
            var responseStream = new MemoryStream(contentsBytes);

            var syncWebRequest = new MockSyncClient(new Response(uri, status, contentType, responseStream));

            return new WebServiceClient(6, "0123456789", new List<string> { "en" },
                httpMessageHandler: mockHttp, syncWebRequest: syncWebRequest);
        }

        [Test, TestCaseSource(nameof(TestCases))]
        public void AddressNotFoundShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var ip = "1.2.3.16";
            var client = CreateClient(type, ip, HttpStatusCode.NotFound,
                content: ErrorJson("IP_ADDRESS_NOT_FOUND", "The value 1.2.3.16 is not in the database."));

            // Not using Assert.Throws due to https://github.com/nunit/nunit/issues/464
            Assert.That(async () => await cr(client, ip),
                Throws.TypeOf<AddressNotFoundException>()
                    .And.Message.Contains("The value 1.2.3.16 is not in the database"));
        }

        [Test, TestCaseSource(nameof(TestCases))]
        public async Task AddressReservedShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var ip = "1.2.3.17";
            var client = CreateClient(type, ip, HttpStatusCode.Forbidden,
                content: ErrorJson("IP_ADDRESS_RESERVED", "The value 1.2.3.17 belongs to a reserved or private range."));

            Assert.That(async () => await cr(client, ip),
                Throws.TypeOf<AddressNotFoundException>()
                    .And.Message.Contains("The value 1.2.3.17 belongs to a reserved or private range"));
        }

        [Test, TestCaseSource(nameof(TestCases))]
        public void BadCharsetRequirementShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var client = CreateClient(type, status: HttpStatusCode.NotAcceptable,
                content: "Cannot satisfy your Accept-Charset requirements",
                contentType: "text/plain");

            Assert.That(async () => await cr(client),
                Throws.TypeOf<HttpException>()
                    .And.Message.Contains("Cannot satisfy your Accept-Charset requirements"));
        }

        [Test, TestCaseSource(nameof(TestCases))]
        public void BadContentTypeShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var client = CreateClient(type, status: HttpStatusCode.OK,
                content: CountryJson, contentType: "bad/content-type");
            Assert.That(async () => await cr(client),
                Throws.TypeOf<GeoIP2Exception>()
                    .And.Message.Contains("but it does not appear to be JSON"));
        }

        [Test, TestCaseSource(nameof(TestCases))]
        public void EmptyBodyShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var client = CreateClient(type);
            Assert.That(async () => await cr(client),
                Throws.TypeOf<HttpException>()
                    .And.Message.Contains("message body"));
        }

        [Test, TestCaseSource(nameof(TestCases))]
        public void InternalServerErrorShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var client = CreateClient(type, status: HttpStatusCode.InternalServerError,
                content: "Internal Server Error");
            Assert.That(async () => await cr(client),
                Throws.TypeOf<HttpException>()
                    .And.Message.Contains("Received a server (500) error"));
        }

        [Test, TestCaseSource(nameof(TestCases))]
        public void IncorrectlyFormattedIPAddressShouldThrowException(string type, ClientRunner cr, Type t)
        {
            Assert.That(async () => await cr(CreateClient(type), "foo"),
                Throws.TypeOf<GeoIP2Exception>()
                    .And.Message.Contains("The specified IP address was incorrectly formatted"));
        }

        [Test, TestCaseSource(nameof(TestCases))]
        public void InvalidAuthShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var client = CreateClient(type, status: HttpStatusCode.Unauthorized,
                content:
                    ErrorJson("AUTHORIZATION_INVALID",
                        "You have supplied an invalid MaxMind user ID and/or license key in the Authorization header."));
            Assert.That(async () => await cr(client),
                Throws.TypeOf<AuthenticationException>()
                    .And.Message.Contains("You have supplied an invalid MaxMind user ID and/or license key"));
        }

        [Test, TestCaseSource(nameof(TestCases))]
        public void MissingLicenseShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var client = CreateClient(type, status: HttpStatusCode.Unauthorized,
                content:
                    ErrorJson("LICENSE_KEY_REQUIRED",
                        "You have not supplied a MaxMind license key in the Authorization header."));
            Assert.That(async () => await cr(client),
                Throws.TypeOf<AuthenticationException>()
                    .And.Message.Contains("You have not supplied a MaxMind license key in the Authorization header"));
        }

        [Test, TestCaseSource(nameof(TestCases))]
        public void MissingUserIdShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var client = CreateClient(type, status: HttpStatusCode.Unauthorized,
                content:
                    ErrorJson("USER_ID_REQUIRED", "You have not supplied a MaxMind user ID in the Authorization header."));
            Assert.That(async () => await cr(client),
                Throws.TypeOf<AuthenticationException>()
                    .And.Message.Contains("You have not supplied a MaxMind user ID in the Authorization header."));
        }

        [Test, TestCaseSource(nameof(TestCases))]
        public void NoErrorBodyShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var client = CreateClient(type, status: HttpStatusCode.Forbidden);
            Assert.That(async () => await cr(client),
                Throws.TypeOf<HttpException>()
                    .And.Message.Contains("with no body"));
        }

        [Test, TestCaseSource(nameof(TestCases))]
        public void OutOfQueriesShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var client = CreateClient(type, status: HttpStatusCode.PaymentRequired,
                content:
                    ErrorJson("OUT_OF_QUERIES",
                        "The license key you have provided is out of queries. Please purchase more queries to use this service."));
            Assert.That(async () => await cr(client),
                Throws.TypeOf<OutOfQueriesException>()
                    .And.Message.Contains("The license key you have provided is out of queries"));
        }

        [Test, TestCaseSource(nameof(TestCases))]
        public void SurprisingStatusShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var client = CreateClient(type, status: HttpStatusCode.MultipleChoices);
            Assert.That(async () => await cr(client),
                Throws.TypeOf<HttpException>()
                    .And.Message.Contains("Received an unexpected response for"));
        }

        [Test, TestCaseSource(nameof(TestCases))]
        public void UndeserializableJsonShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var client = CreateClient(type, status: HttpStatusCode.OK,
                content: "{\"invalid\":yes}");

            Assert.That(async () => await cr(client),
                Throws.TypeOf<GeoIP2Exception>()
                    .And.Message.Contains("Received a 200 response but not decode it as JSON"));
        }

        [Test, TestCaseSource(nameof(TestCases))]
        public void UnexpectedErrorBodyShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var client = CreateClient(type, status: HttpStatusCode.Forbidden,
                content: "{\"invalid\": }");

            Assert.That(async () => await cr(client),
                Throws.TypeOf<HttpException>()
                    .And.Message.Contains("it did not include the expected JSON body"));
        }

        [Test, TestCaseSource(nameof(TestCases))]
        public void WebServiceErrorShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var client = CreateClient(type, status: HttpStatusCode.Forbidden,
                content: ErrorJson("IP_ADDRESS_INVALID",
                    "The value 1.2.3 is not a valid IP address"));

            Assert.That(async () => await cr(client),
                Throws.TypeOf<InvalidRequestException>()
                    .And.Message.Contains("not a valid IP address"));
        }

        [Test, TestCaseSource(nameof(TestCases))]
        public void WeirdErrorBodyShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var client = CreateClient(type, status: HttpStatusCode.Forbidden,
                content: "{\"weird\": 42}");
            Assert.That(async () => await cr(client),
                Throws.TypeOf<HttpException>()
                    .And.Message.Contains("does not specify code or error keys"));
        }

        [Test, TestCaseSource(nameof(TestCases))]
        public async Task CorrectlyFormattedResponseShouldDeserializeIntoResponseObject(string type, ClientRunner cr,
            Type t)
        {
            var client = CreateClient(type, content: CountryJson);
            var result = await cr(client);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetType(), Is.EqualTo(t));
        }

        [Test, TestCaseSource(nameof(TestCases))]
        public async Task MeEndpointIsCalledCorrectly(string type, ClientRunner cr,
            Type t)
        {
            var client = CreateClient(type, "me", content: CountryJson);
            var result = await cr(client, null);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void MissingKeys()
        {
            var insights = CreateClient("insights", content: "{}").Insights("1.2.3.4");

            var city = insights.City;
            Assert.IsNotNull(city);
            Assert.IsNull(city.Confidence);

            var continent = insights.Continent;
            Assert.IsNotNull(continent);
            Assert.IsNull(continent.Code);

            var country = insights.Country;
            Assert.IsNotNull(country);

            var location = insights.Location;
            Assert.IsNotNull(location);
            Assert.IsNull(location.AccuracyRadius);
            Assert.IsNull(location.Latitude);
            Assert.IsNull(location.Longitude);
            Assert.IsNull(location.MetroCode);
            Assert.IsNull(location.TimeZone);
            Assert.IsNull(location.PopulationDensity);
            Assert.IsNull(location.AverageIncome);

            var maxmind = insights.MaxMind;
            Assert.IsNotNull(maxmind);
            Assert.IsNull(maxmind.QueriesRemaining);

            Assert.IsNotNull(insights.Postal);

            var registeredCountry = insights.RegisteredCountry;
            Assert.IsNotNull(registeredCountry);

            var representedCountry = insights.RepresentedCountry;
            Assert.IsNotNull(representedCountry);
            Assert.IsNull(representedCountry.Type);

            var subdivisions = insights.Subdivisions;
            Assert.IsNotNull(subdivisions);
            Assert.AreEqual(0, subdivisions.Count);

            var subdiv = insights.MostSpecificSubdivision;
            Assert.IsNotNull(subdiv);
            Assert.IsNull(subdiv.IsoCode);
            Assert.IsNull(subdiv.Confidence);

            var traits = insights.Traits;
            Assert.IsNotNull(traits);
            Assert.IsNull(traits.AutonomousSystemNumber);
            Assert.IsNull(traits.AutonomousSystemOrganization);
            Assert.IsNull(traits.Domain);
            Assert.IsNull(traits.IPAddress);
            Assert.IsNull(traits.Isp);
            Assert.IsNull(traits.Organization);
            Assert.IsNull(traits.UserType);
#pragma warning disable 0618
            Assert.IsFalse(traits.IsAnonymousProxy);
            Assert.IsFalse(traits.IsSatelliteProvider);
#pragma warning restore 0618
            Assert.AreEqual(
                "Traits [IsAnonymousProxy=False, IsSatelliteProvider=False, ]",
                traits.ToString());

            foreach (var c in new[]
            {
                country, registeredCountry,
                representedCountry
            })
            {
                Assert.IsNull(c.Confidence);
                Assert.IsNull(c.IsoCode);
            }

            foreach (var r in new NamedEntity[]
            {
                city,
                continent, country, registeredCountry, representedCountry,
                subdiv
            })
            {
                Assert.IsNull(r.GeoNameId);
                Assert.IsNull(r.Name);
                Assert.AreEqual(0, r.Names.Count);
                Assert.AreEqual("", r.ToString());
            }
        }
    }
}