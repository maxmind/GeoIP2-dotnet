#region

using MaxMind.GeoIP2.Exceptions;
using MaxMind.GeoIP2.Http;
using MaxMind.GeoIP2.Model;
using MaxMind.GeoIP2.Responses;
using MaxMind.GeoIP2.UnitTests.Mock;
#if !NET461
using Microsoft.Extensions.Options;
#endif
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static MaxMind.GeoIP2.UnitTests.ResponseHelper;

#endregion

namespace MaxMind.GeoIP2.UnitTests
{
    public class WebServiceClientTests
    {
        public delegate Task<AbstractCountryResponse> ClientRunner(WebServiceClient c, string ip = "1.2.3.4");

        // I don't love running the sync tests with async, but the alternative
        // seems to be a lot of code duplication.
        // "Async" added to the name so that Nunit can tell them apart.
        public static readonly object[][] TestCases =
        {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
            new object[] {"country", (ClientRunner) (async (c, i) => c.Country(i)), typeof(CountryResponse)},
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
            new object[] {"city", (ClientRunner) (async (c, i) => c.City(i)), typeof(CityResponse)},
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
            new object[] {"insights", (ClientRunner) (async (c, i) => c.Insights(i)), typeof(InsightsResponse)},
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
            new object[]
                {"countryAsync", (ClientRunner) (async (c, i) => await c.CountryAsync(i)), typeof(CountryResponse)},
            new object[] {"cityAsync", (ClientRunner) (async (c, i) => await c.CityAsync(i)), typeof(CityResponse)},
            new object[]
                {"insightsAsync", (ClientRunner) (async (c, i) => await c.InsightsAsync(i)), typeof(InsightsResponse)}
        };

        public delegate Task<AbstractCountryResponse> MeClientRunner(WebServiceClient c);

        public static readonly object[][] MeTestCases =
        {
            new object[]
            {
                "country", (MeClientRunner) (c => Task.FromResult<AbstractCountryResponse>(c.Country())),
                typeof(CountryResponse)
            },
            new object[]
            {
                "city", (MeClientRunner) (c => Task.FromResult<AbstractCountryResponse>(c.City())), typeof(CityResponse)
            },
            new object[]
            {
                "insights", (MeClientRunner) (c => Task.FromResult<AbstractCountryResponse>(c.Insights())),
                typeof(InsightsResponse)
            },
            new object[]
                {"countryAsync", (MeClientRunner) (async c => await c.CountryAsync()), typeof(CountryResponse)},
            new object[] {"cityAsync", (MeClientRunner) (async c => await c.CityAsync()), typeof(CityResponse)},
            new object[]
                {"insightsAsync", (MeClientRunner) (async c => await c.InsightsAsync()), typeof(InsightsResponse)}
        };

        private static WebServiceClient CreateClient(string type, string ipAddress = "1.2.3.4",
            HttpStatusCode status = HttpStatusCode.OK, string? contentType = null, string content = "")
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

            var syncWebRequest = new MockSyncClient(new Response(uri, status, contentType, contentsBytes));

            return new WebServiceClient(6, "0123456789",
                locales: new List<string> { "en" },
                host: "geoip.maxmind.com",
                timeout: 3000,
                httpClient: new HttpClient(mockHttp),
                syncWebRequest: syncWebRequest
            );
        }

        // See https://github.com/xunit/xunit/issues/1517
#pragma warning disable xUnit1026
        [Theory, MemberData(nameof(TestCases))]
        public async Task AddressNotFoundShouldThrowException(string type, ClientRunner cr, Type _)
        {
            var client = CreateClient(type, status: HttpStatusCode.NotFound,
                content: ErrorJson("IP_ADDRESS_NOT_FOUND", "The value 1.2.3.16 is not in the database."));

            var exception = await Record.ExceptionAsync(async () => await cr(client));
            Assert.NotNull(exception);
            Assert.IsType<AddressNotFoundException>(exception);
            Assert.Contains("The value 1.2.3.16 is not in the database", exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task AddressReservedShouldThrowException(string type, ClientRunner cr, Type _)
        {
            var client = CreateClient(type, status: HttpStatusCode.Forbidden,
                content: ErrorJson("IP_ADDRESS_RESERVED",
                    "The value 1.2.3.17 belongs to a reserved or private range."));

            var exception = await Record.ExceptionAsync(async () => await cr(client));
            Assert.NotNull(exception);
            Assert.IsType<AddressNotFoundException>(exception);
            Assert.Contains("The value 1.2.3.17 belongs to a reserved or private range", exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task BadCharsetRequirementShouldThrowException(string type, ClientRunner cr, Type _)
        {
            var client = CreateClient(type, status: HttpStatusCode.NotAcceptable,
                content: "Cannot satisfy your Accept-Charset requirements",
                contentType: "text/plain");

            var exception = await Record.ExceptionAsync(async () => await cr(client));
            Assert.NotNull(exception);
            Assert.IsType<HttpException>(exception);
            Assert.Contains("Cannot satisfy your Accept-Charset requirements", exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task BadContentTypeShouldThrowException(string type, ClientRunner cr, Type _)
        {
            var client = CreateClient(type, status: HttpStatusCode.OK,
                content: CountryJson, contentType: "bad/content-type");

            var exception = await Record.ExceptionAsync(async () => await cr(client));
            Assert.NotNull(exception);
            Assert.IsType<GeoIP2Exception>(exception);
            Assert.Contains("but it does not appear to be JSON", exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task EmptyBodyShouldThrowException(string type, ClientRunner cr, Type _)
        {
            var client = CreateClient(type);

            var exception = await Record.ExceptionAsync(async () => await cr(client));
            Assert.NotNull(exception);
            Assert.IsType<HttpException>(exception);
            Assert.Contains("message body", exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task InternalServerErrorShouldThrowException(string type, ClientRunner cr, Type _)
        {
            var client = CreateClient(type, status: HttpStatusCode.InternalServerError,
                content: "Internal Server Error");

            var exception = await Record.ExceptionAsync(async () => await cr(client));
            Assert.NotNull(exception);
            Assert.IsType<HttpException>(exception);
            Assert.Contains("Received a server (500) error", exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task IncorrectlyFormattedIPAddressShouldThrowException(string type, ClientRunner cr, Type _)
        {
            var exception = await Record.ExceptionAsync(async () => await cr(CreateClient(type), "foo"));
            Assert.NotNull(exception);
            Assert.IsType<GeoIP2Exception>(exception);
            Assert.Contains("The specified IP address was incorrectly formatted", exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task PermissionRequiredShouldThrowException(string type, ClientRunner cr, Type _)
        {
            var msg = "You do not have permission to use this web service.";
            var client = CreateClient(type, status: HttpStatusCode.Forbidden,
                content:
                ErrorJson("PERMISSION_REQUIRED", msg));

            var exception = await Record.ExceptionAsync(async () => await cr(client));
            Assert.NotNull(exception);
            Assert.IsType<PermissionRequiredException>(exception);
            Assert.Contains(msg, exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task AuthenticationErrorShouldThrowException(string type, ClientRunner cr, Type _)
        {
            var errors = new List<string>
            {
                "ACCOUNT_ID_REQUIRED",
                "ACCOUNT_ID_UNKNOWN",
                "AUTHORIZATION_INVALID",
                "LICENSE_KEY_REQUIRED",
            };
            foreach (var error in errors)
            {
                var msg = "Appropriate user-readable error message";
                var client = CreateClient(type, status: HttpStatusCode.Unauthorized,
                    content:
                    ErrorJson(error, msg));

                var exception = await Record.ExceptionAsync(async () => await cr(client));
                Assert.NotNull(exception);
                Assert.IsType<AuthenticationException>(exception);
                Assert.Contains(msg, exception.Message);
            }
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task NoErrorBodyShouldThrowException(string type, ClientRunner cr, Type _)
        {
            var client = CreateClient(type, status: HttpStatusCode.Forbidden);

            var exception = await Record.ExceptionAsync(async () => await cr(client));
            Assert.NotNull(exception);
            Assert.IsType<HttpException>(exception);
            Assert.Contains("with no body", exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task OutOfQueriesShouldThrowException(string type, ClientRunner cr, Type _)
        {
            var client = CreateClient(type, status: HttpStatusCode.PaymentRequired,
                content:
                ErrorJson("OUT_OF_QUERIES",
                    "The license key you have provided is out of queries. Please purchase more queries to use this service."));

            var exception = await Record.ExceptionAsync(async () => await cr(client));
            Assert.NotNull(exception);
            Assert.IsType<OutOfQueriesException>(exception);
            Assert.Contains("The license key you have provided is out of queries", exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task InsufficientFundsShouldThrowException(string type, ClientRunner cr, Type _)
        {
            var msg =
                "The license key you have provided is out of queries. Please purchase more queries to use this service.";
            var client = CreateClient(type, status: HttpStatusCode.PaymentRequired,
                content:
                ErrorJson("INSUFFICIENT_FUNDS", msg));
            var exception = await Record.ExceptionAsync(async () => await cr(client));
            Assert.NotNull(exception);
            Assert.IsType<OutOfQueriesException>(exception);
            Assert.Contains(msg, exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task SurprisingStatusShouldThrowException(string type, ClientRunner cr, Type _)
        {
            var client = CreateClient(type, status: HttpStatusCode.MultipleChoices);
            var exception = await Record.ExceptionAsync(async () => await cr(client));
            Assert.NotNull(exception);
            Assert.IsType<HttpException>(exception);
            Assert.Contains("Received an unexpected response for", exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task UndeserializableJsonShouldThrowException(string type, ClientRunner cr, Type _)
        {
            var client = CreateClient(type, status: HttpStatusCode.OK,
                content: "{\"invalid\":yes}");

            var exception = await Record.ExceptionAsync(async () => await cr(client));
            Assert.NotNull(exception);
            Assert.IsType<GeoIP2Exception>(exception);
            Assert.Contains("Received a 200 response but not decode it as JSON", exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task UnexpectedErrorBodyShouldThrowExceptionAsync(string type, ClientRunner cr, Type _)
        {
            var client = CreateClient(type, status: HttpStatusCode.Forbidden,
                content: "{\"invalid\": }");

            var exception = await Record.ExceptionAsync(async () => await cr(client));
            Assert.NotNull(exception);
            Assert.IsType<HttpException>(exception);
            Assert.Contains("it did not include the expected JSON body", exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task WebServiceErrorShouldThrowException(string type, ClientRunner cr, Type _)
        {
            var client = CreateClient(type, status: HttpStatusCode.Forbidden,
                content: ErrorJson("IP_ADDRESS_INVALID",
                    "The value 1.2.3 is not a valid IP address"));
            var exception = await Record.ExceptionAsync(async () => await cr(client));
            Assert.NotNull(exception);
            Assert.IsType<InvalidRequestException>(exception);
            Assert.Contains("not a valid IP address", exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task WeirdErrorBodyShouldThrowExceptionAsync(string type, ClientRunner cr, Type _)
        {
            var client = CreateClient(type, status: HttpStatusCode.Forbidden,
                content: "{\"weird\": 42}");
            var exception = await Record.ExceptionAsync(async () => await cr(client));
            Assert.NotNull(exception);
            Assert.IsType<HttpException>(exception);
            Assert.Contains("does not specify code or error keys", exception.Message);
        }
#pragma warning restore xUnit1026

        [Theory, MemberData(nameof(TestCases))]
        public async Task CorrectlyFormattedResponseShouldDeserializeIntoResponseObject(string type, ClientRunner cr,
            Type t)
        {
            var client = CreateClient(type, content: CountryJson);
            var result = await cr(client);

            Assert.NotNull(result);
            Assert.Equal(t, result.GetType());
            Assert.Equal("1.2.3.0/24", result.Traits.Network?.ToString());
        }

        [Theory, MemberData(nameof(MeTestCases))]
        public async Task MeEndpointIsCalledCorrectly(string type, MeClientRunner cr,
            Type t)
        {
            var client = CreateClient(type, "me", content: CountryJson);
            var result = await cr(client);

            Assert.NotNull(result);
            Assert.Equal(t, result.GetType());
        }

        [Fact]
        public void MissingKeys()
        {
            var insights = CreateClient("insights", content: "{}").Insights("1.2.3.4");

            var city = insights.City;
            Assert.NotNull(city);
            Assert.Null(city.Confidence);

            var continent = insights.Continent;
            Assert.NotNull(continent);
            Assert.Null(continent.Code);

            var country = insights.Country;
            Assert.NotNull(country);
            Assert.False(country.IsInEuropeanUnion);

            var location = insights.Location;
            Assert.NotNull(location);
            Assert.Null(location.AccuracyRadius);
            Assert.Null(location.Latitude);
            Assert.Null(location.Longitude);
            Assert.Null(location.MetroCode);
            Assert.Null(location.TimeZone);
            Assert.Null(location.PopulationDensity);
            Assert.Null(location.AverageIncome);

            var maxmind = insights.MaxMind;
            Assert.NotNull(maxmind);
            Assert.Null(maxmind.QueriesRemaining);

            Assert.NotNull(insights.Postal);

            var registeredCountry = insights.RegisteredCountry;
            Assert.NotNull(registeredCountry);
            Assert.False(registeredCountry.IsInEuropeanUnion);

            var representedCountry = insights.RepresentedCountry;
            Assert.NotNull(representedCountry);
            Assert.False(representedCountry.IsInEuropeanUnion);
            Assert.Null(representedCountry.Type);

            var subdivisions = insights.Subdivisions;
            Assert.NotNull(subdivisions);
            Assert.Empty(subdivisions);

            var subdiv = insights.MostSpecificSubdivision;
            Assert.NotNull(subdiv);
            Assert.Null(subdiv.IsoCode);
            Assert.Null(subdiv.Confidence);

            var traits = insights.Traits;
            Assert.NotNull(traits);
            Assert.Null(traits.AutonomousSystemNumber);
            Assert.Null(traits.AutonomousSystemOrganization);
            Assert.Null(traits.Domain);
            Assert.Null(traits.IPAddress);
            Assert.Null(traits.Isp);
            Assert.Null(traits.Organization);
            Assert.Null(traits.StaticIPScore);
            Assert.Null(traits.UserCount);
            Assert.Null(traits.UserType);
#pragma warning disable 0618
            Assert.False(traits.IsAnonymousProxy);
            Assert.False(traits.IsSatelliteProvider);
#pragma warning restore 0618

            foreach (var c in new[]
            {
                country, registeredCountry,
                representedCountry
            })
            {
                Assert.Null(c.Confidence);
                Assert.Null(c.IsoCode);
            }

            foreach (var r in new NamedEntity[]
            {
                city,
                continent, country, registeredCountry, representedCountry,
                subdiv
            })
            {
                Assert.Null(r.GeoNameId);
                Assert.Null(r.Name);
                Assert.Empty(r.Names);
                Assert.Equal("", r.ToString());
            }
        }

        [Fact]
        public void Constructors()
        {
            var id = 42;
            var key = "1234567890ab";

            // This is mostly to test that the various ways to call this
            // compile. If you have to change these tests to get them to
            // compile and you aren't doing a major release, you are
            // probably doing something wrong.
            Assert.NotNull(new WebServiceClient(id, key));
            Assert.NotNull(new WebServiceClient(id, key, new List<string>()));
            Assert.NotNull(new WebServiceClient(accountId: id, licenseKey: key));
        }

        #region NetCoreTests

#if !NET461

        [Fact]
        public async Task WebServiceOptionsConstructor()
        {
            var stringContent = new StringContent(CountryJson);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue(
                "application/vnd.maxmind.com-country+json");
            stringContent.Headers.Add("Content-Length", CountryJson.Length.ToString());
            var message = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = stringContent
            };

            // HttpClient mock
            var uri = new Uri("https://test.maxmind.com/geoip/v2.1/country/me");
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(HttpMethod.Get, uri.ToString())
                .WithHeaders("Accept", "application/json")
                .Respond(message);

            var options = Options.Create(new WebServiceClientOptions
            {
                AccountId = 6,
                LicenseKey = "0123456789",
                Host = "test.maxmind.com",
                Timeout = 3000,
                Locales = new List<string> { "en" }
            });

            var client = new WebServiceClient(
                new HttpClient(mockHttp),
                options
            );

            var result = await client.CountryAsync();

            Assert.NotNull(result);
            Assert.Equal("1.2.3.0/24", result.Traits.Network?.ToString());
        }
#endif

        #endregion
    }
}
