#region

using MaxMind.GeoIP2.Exceptions;
using MaxMind.GeoIP2.Http;
using MaxMind.GeoIP2.Model;
using MaxMind.GeoIP2.Responses;
using NSubstitute;
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

            var syncWebRequest = Substitute.For<ISyncClient>();
            syncWebRequest
                .Get(uri)
                .Returns(new Response(uri, status, contentType, responseStream));

            return new WebServiceClient(6, "0123456789", new List<string> { "en" },
                httpMessageHandler: mockHttp, syncWebRequest: syncWebRequest);
        }

        [Test, TestCaseSource(nameof(TestCases))]
        [ExpectedException(typeof(AddressNotFoundException),
            ExpectedMessage = "The value 1.2.3.16 is not in the database", MatchType = MessageMatch.Contains)]
        public async Task AddressNotFoundShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var ip = "1.2.3.16";
            var client = CreateClient(type, ip, HttpStatusCode.NotFound,
                content: ErrorJson("IP_ADDRESS_NOT_FOUND", "The value 1.2.3.16 is not in the database."));

            await cr(client, ip);
        }

        [Test, TestCaseSource(nameof(TestCases))]
        [ExpectedException(typeof(AddressNotFoundException),
            ExpectedMessage = "The value 1.2.3.17 belongs to a reserved or private range",
            MatchType = MessageMatch.Contains)]
        public async Task AddressReservedShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var ip = "1.2.3.17";
            var client = CreateClient(type, ip, HttpStatusCode.Forbidden,
                content: ErrorJson("IP_ADDRESS_RESERVED", "The value 1.2.3.17 belongs to a reserved or private range."));

            await cr(client, ip);
        }

        [Test, TestCaseSource(nameof(TestCases))]
        [ExpectedException(typeof(HttpException), ExpectedMessage = "Cannot satisfy your Accept-Charset requirements",
            MatchType = MessageMatch.Contains)]
        public async Task BadCharsetRequirementShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var client = CreateClient(type, status: HttpStatusCode.NotAcceptable,
                content: "Cannot satisfy your Accept-Charset requirements",
                contentType: "text/plain");

            await cr(client);
        }

        [Test, TestCaseSource(nameof(TestCases))]
        [ExpectedException(typeof(GeoIP2Exception), ExpectedMessage = "but it does not appear to be JSON",
            MatchType = MessageMatch.Contains)]
        public async Task BadContentTypeShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var client = CreateClient(type, status: HttpStatusCode.OK,
                content: CountryJson, contentType: "bad/content-type");
            await cr(client);
        }

        [Test, TestCaseSource(nameof(TestCases))]
        [ExpectedException(typeof(HttpException), ExpectedMessage = "message body", MatchType = MessageMatch.Contains)]
        public async Task EmptyBodyShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var client = CreateClient(type);
            await cr(client);
        }

        [Test, TestCaseSource(nameof(TestCases))]
        [ExpectedException(typeof(HttpException), ExpectedMessage = "Received a server (500) error",
            MatchType = MessageMatch.Contains)]
        public async Task InternalServerErrorShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var client = CreateClient(type, status: HttpStatusCode.InternalServerError,
                content: "Internal Server Error");
            await cr(client);
        }

        [Test, TestCaseSource(nameof(TestCases))]
        [ExpectedException(typeof(GeoIP2Exception),
            ExpectedMessage = "The specified IP address was incorrectly formatted", MatchType = MessageMatch.Contains)]
        public async Task IncorrectlyFormattedIPAddressShouldThrowException(string type, ClientRunner cr, Type t)
        {
            await cr(CreateClient(type), "foo");
        }

        [Test, TestCaseSource(nameof(TestCases))]
        [ExpectedException(typeof(AuthenticationException),
            ExpectedMessage = "You have supplied an invalid MaxMind user ID and/or license key",
            MatchType = MessageMatch.Contains)]
        public async Task InvalidAuthShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var client = CreateClient(type, status: HttpStatusCode.Unauthorized,
                content:
                    ErrorJson("AUTHORIZATION_INVALID",
                        "You have supplied an invalid MaxMind user ID and/or license key in the Authorization header."));
            await cr(client);
        }

        [Test, TestCaseSource(nameof(TestCases))]
        [ExpectedException(typeof(AuthenticationException),
            ExpectedMessage = "You have not supplied a MaxMind license key in the Authorization header",
            MatchType = MessageMatch.Contains)]
        public async Task MissingLicenseShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var client = CreateClient(type, status: HttpStatusCode.Unauthorized,
                content:
                    ErrorJson("LICENSE_KEY_REQUIRED",
                        "You have not supplied a MaxMind license key in the Authorization header."));
            await cr(client);
        }

        [Test, TestCaseSource(nameof(TestCases))]
        [ExpectedException(typeof(AuthenticationException),
            ExpectedMessage = "You have not supplied a MaxMind user ID in the Authorization header",
            MatchType = MessageMatch.Contains)]
        public async Task MissingUserIdShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var client = CreateClient(type, status: HttpStatusCode.Unauthorized,
                content:
                    ErrorJson("USER_ID_REQUIRED", "You have not supplied a MaxMind user ID in the Authorization header."));
            await cr(client);
        }

        [Test, TestCaseSource(nameof(TestCases))]
        [ExpectedException(typeof(HttpException), ExpectedMessage = "with no body",
            MatchType = MessageMatch.Contains)]
        public async Task NoErrorBodyShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var client = CreateClient(type, status: HttpStatusCode.Forbidden);
            await cr(client);
        }

        [Test, TestCaseSource(nameof(TestCases))]
        [ExpectedException(typeof(OutOfQueriesException),
            ExpectedMessage = "The license key you have provided is out of queries", MatchType = MessageMatch.Contains)]
        public async Task OutOfQueriesShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var client = CreateClient(type, status: HttpStatusCode.PaymentRequired,
                content:
                    ErrorJson("OUT_OF_QUERIES",
                        "The license key you have provided is out of queries. Please purchase more queries to use this service."));
            await cr(client);
        }

        [Test, TestCaseSource(nameof(TestCases))]
        [ExpectedException(typeof(HttpException),
            ExpectedMessage = "Received an unexpected response for",
            MatchType = MessageMatch.Contains)]
        public async Task SurprisingStatusShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var client = CreateClient(type, status: HttpStatusCode.MultipleChoices);
            await cr(client);
        }

        [Test, TestCaseSource(nameof(TestCases))]
        [ExpectedException(typeof(GeoIP2Exception),
            ExpectedMessage = "Received a 200 response but not decode it as JSON", MatchType = MessageMatch.Contains)]
        public async Task UndeserializableJsonShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var client = CreateClient(type, status: HttpStatusCode.OK,
                content: "{\"invalid\":yes}");

            await cr(client);
        }

        [Test, TestCaseSource(nameof(TestCases))]
        [ExpectedException(typeof(HttpException), ExpectedMessage = "it did not include the expected JSON body",
            MatchType = MessageMatch.Contains)]
        public async Task UnexpectedErrorBodyShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var client = CreateClient(type, status: HttpStatusCode.Forbidden,
                content: "{\"invalid\": }");
            await cr(client);
        }

        [Test, TestCaseSource(nameof(TestCases))]
        [ExpectedException(typeof(InvalidRequestException), ExpectedMessage = "not a valid IP address",
            MatchType = MessageMatch.Contains)]
        public async Task WebServiceErrorShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var client = CreateClient(type, status: HttpStatusCode.Forbidden,
                content: ErrorJson("IP_ADDRESS_INVALID",
                    "The value 1.2.3 is not a valid IP address"));

            await cr(client);
        }

        [Test, TestCaseSource(nameof(TestCases))]
        [ExpectedException(typeof(HttpException), ExpectedMessage = "does not specify code or error keys",
            MatchType = MessageMatch.Contains)]
        public async Task WeirdErrorBodyShouldThrowException(string type, ClientRunner cr, Type t)
        {
            var client = CreateClient(type, status: HttpStatusCode.Forbidden,
                content: "{\"weird\": 42}");
            await cr(client);
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