#region

using MaxMind.GeoIP2.Exceptions;
using MaxMind.GeoIP2.Model;
using MaxMind.GeoIP2.Responses;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;
using static MaxMind.GeoIP2.UnitTests.ResponseHelper;

#endregion

namespace MaxMind.GeoIP2.UnitTests
{
    public class WebServiceClientTests : IDisposable
    {
        private readonly WireMockServer _server;

        public WebServiceClientTests()
        {
            _server = WireMockServer.Start();
        }


        // I don't love running the sync tests with async, but the alternative
        // seems to be a lot of code duplication.
        // "Async" added to the name so that Nunit can tell them apart.
        public static readonly TheoryData<string, string, string> TestCases = new()
        {
            // testCaseName,     methodName,     expectedTypeName
            { "country",        "Country",      nameof(CountryResponse) },
            { "city",           "City",         nameof(CityResponse) },
            { "insights",       "Insights",     nameof(InsightsResponse) },
            { "countryAsync",   "CountryAsync", nameof(CountryResponse) },
            { "cityAsync",      "CityAsync",    nameof(CityResponse) },
            { "insightsAsync",  "InsightsAsync",nameof(InsightsResponse) }
        };


        public static readonly TheoryData<string, string, string> MeTestCases = new()
        {
            // testCaseName,     methodName,     expectedTypeName
            { "country",        "Country",      nameof(CountryResponse) },
            { "city",           "City",         nameof(CityResponse) },
            { "insights",       "Insights",     nameof(InsightsResponse) },
            { "countryAsync",   "CountryAsync", nameof(CountryResponse) },
            { "cityAsync",      "CityAsync",    nameof(CityResponse) },
            { "insightsAsync",  "InsightsAsync",nameof(InsightsResponse) }
        };
        private bool _disposed;

        private WebServiceClient CreateClient(string type, string ipAddress = "1.2.3.4",
            HttpStatusCode status = HttpStatusCode.OK, string? contentType = null, string content = "")
        {
            var service = type.Replace("Async", "");

            contentType ??= $"application/vnd.maxmind.com-{service}+json";

            _server
              .Given(
                Request.Create()
                .WithPath($"/geoip/v2.1/{service}/{ipAddress}")
                .UsingGet()
                )
              .RespondWith(
                Response.Create()
                  .WithStatusCode(status)
                  .WithHeader("Content-Type", contentType)
                  .WithBody(content)
              );

            var host = _server.Urls[0].Replace("http://", "");

            return new WebServiceClient(6, "0123456789",
                locales: new List<string> { "en" },
                host: host,
                timeout: 3000,
                disableHttps: true
            );
        }

        // Helper to invoke the correct client method based on methodName string
        private static async Task<AbstractCountryResponse> InvokeClientMethod(WebServiceClient client, string methodName, string ip = "1.2.3.4")
        {
            return methodName switch
            {
                "Country" => client.Country(ip),
                "City" => client.City(ip),
                "Insights" => client.Insights(ip),
                "CountryAsync" => await client.CountryAsync(ip),
                "CityAsync" => await client.CityAsync(ip),
                "InsightsAsync" => await client.InsightsAsync(ip),
                _ => throw new ArgumentException($"Unknown method name: {methodName}", nameof(methodName)),
            };
        }

        // Helper to invoke the correct client method for the "me" endpoint
        private static async Task<AbstractCountryResponse> InvokeMeClientMethod(WebServiceClient client, string methodName)
        {
            return methodName switch
            {
                "Country" => client.Country(),
                "City" => client.City(),
                "Insights" => client.Insights(),
                "CountryAsync" => await client.CountryAsync(),
                "CityAsync" => await client.CityAsync(),
                "InsightsAsync" => await client.InsightsAsync(),
                _ => throw new ArgumentException($"Unknown method name: {methodName}", nameof(methodName)),
            };
        }

        // See https://github.com/xunit/xunit/issues/1517
#pragma warning disable xUnit1026
        [Theory, MemberData(nameof(TestCases))]
        public async Task AddressNotFoundShouldThrowException(string testCaseName, string methodName, string _)
        {
            var client = CreateClient(testCaseName, status: HttpStatusCode.NotFound,
                content: ErrorJson("IP_ADDRESS_NOT_FOUND", "The value 1.2.3.16 is not in the database."));

            var exception = await Record.ExceptionAsync(() => InvokeClientMethod(client, methodName));
            Assert.NotNull(exception);
            Assert.IsType<AddressNotFoundException>(exception);
            Assert.Contains("The value 1.2.3.16 is not in the database", exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task AddressReservedShouldThrowException(string testCaseName, string methodName, string _)
        {
            var client = CreateClient(testCaseName, status: HttpStatusCode.Forbidden,
                content: ErrorJson("IP_ADDRESS_RESERVED",
                    "The value 1.2.3.17 belongs to a reserved or private range."));

            var exception = await Record.ExceptionAsync(() => InvokeClientMethod(client, methodName));
            Assert.NotNull(exception);
            Assert.IsType<AddressNotFoundException>(exception);
            Assert.Contains("The value 1.2.3.17 belongs to a reserved or private range", exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task BadCharsetRequirementShouldThrowException(string testCaseName, string methodName, string _)
        {
            var client = CreateClient(testCaseName, status: HttpStatusCode.NotAcceptable,
                content: "Cannot satisfy your Accept-Charset requirements",
                contentType: "text/plain");

            var exception = await Record.ExceptionAsync(() => InvokeClientMethod(client, methodName));
            Assert.NotNull(exception);
            Assert.IsType<HttpException>(exception);
            Assert.Contains("Cannot satisfy your Accept-Charset requirements", exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task BadContentTypeShouldThrowException(string testCaseName, string methodName, string _)
        {
            var client = CreateClient(testCaseName, status: HttpStatusCode.OK,
                content: CountryJson, contentType: "bad/content-type");

            var exception = await Record.ExceptionAsync(() => InvokeClientMethod(client, methodName));
            Assert.NotNull(exception);
            Assert.IsType<GeoIP2Exception>(exception);
            Assert.Contains("but it does not appear to be JSON", exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task EmptyBodyShouldThrowException(string testCaseName, string methodName, string _)
        {
            var client = CreateClient(testCaseName);

            var exception = await Record.ExceptionAsync(() => InvokeClientMethod(client, methodName));
            Assert.NotNull(exception);
            Assert.IsType<HttpException>(exception);
            Assert.Contains("message body", exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task InternalServerErrorShouldThrowException(string testCaseName, string methodName, string _)
        {
            var client = CreateClient(testCaseName, status: HttpStatusCode.InternalServerError,
                content: "Internal Server Error");

            var exception = await Record.ExceptionAsync(() => InvokeClientMethod(client, methodName));
            Assert.NotNull(exception);
            Assert.IsType<HttpException>(exception);
            Assert.Contains("Received a server (500) error", exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task IncorrectlyFormattedIPAddressShouldThrowException(string testCaseName, string methodName, string _)
        {
            var exception = await Record.ExceptionAsync(() => InvokeClientMethod(CreateClient(testCaseName), methodName, "foo"));
            Assert.NotNull(exception);
            Assert.IsType<GeoIP2Exception>(exception);
            Assert.Contains("The specified IP address was incorrectly formatted", exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task PermissionRequiredShouldThrowException(string testCaseName, string methodName, string _)
        {
            var msg = "You do not have permission to use this web service.";
            var client = CreateClient(testCaseName, status: HttpStatusCode.Forbidden,
                content:
                ErrorJson("PERMISSION_REQUIRED", msg));

            var exception = await Record.ExceptionAsync(() => InvokeClientMethod(client, methodName));
            Assert.NotNull(exception);
            Assert.IsType<PermissionRequiredException>(exception);
            Assert.Contains(msg, exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task AuthenticationErrorShouldThrowException(string testCaseName, string methodName, string _)
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
                var client = CreateClient(testCaseName, status: HttpStatusCode.Unauthorized,
                    content:
                    ErrorJson(error, msg));

                var exception = await Record.ExceptionAsync(() => InvokeClientMethod(client, methodName));
                Assert.NotNull(exception);
                Assert.IsType<AuthenticationException>(exception);
                Assert.Contains(msg, exception.Message);
            }
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task NoErrorBodyShouldThrowException(string testCaseName, string methodName, string _)
        {
            var client = CreateClient(testCaseName, status: HttpStatusCode.Forbidden);

            var exception = await Record.ExceptionAsync(() => InvokeClientMethod(client, methodName));
            Assert.NotNull(exception);
            Assert.IsType<HttpException>(exception);
            Assert.Contains("with no body", exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task OutOfQueriesShouldThrowException(string testCaseName, string methodName, string _)
        {
            var client = CreateClient(testCaseName, status: HttpStatusCode.PaymentRequired,
                content:
                ErrorJson("OUT_OF_QUERIES",
                    "The license key you have provided is out of queries. Please purchase more queries to use this service."));

            var exception = await Record.ExceptionAsync(() => InvokeClientMethod(client, methodName));
            Assert.NotNull(exception);
            Assert.IsType<OutOfQueriesException>(exception);
            Assert.Contains("The license key you have provided is out of queries", exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task InsufficientFundsShouldThrowException(string testCaseName, string methodName, string _)
        {
            var msg =
                "The license key you have provided is out of queries. Please purchase more queries to use this service.";
            var client = CreateClient(testCaseName, status: HttpStatusCode.PaymentRequired,
                content:
                ErrorJson("INSUFFICIENT_FUNDS", msg));
            var exception = await Record.ExceptionAsync(() => InvokeClientMethod(client, methodName));
            Assert.NotNull(exception);
            Assert.IsType<OutOfQueriesException>(exception);
            Assert.Contains(msg, exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task SurprisingStatusShouldThrowException(string testCaseName, string methodName, string _)
        {
            var client = CreateClient(testCaseName, status: HttpStatusCode.MultipleChoices);
            var exception = await Record.ExceptionAsync(() => InvokeClientMethod(client, methodName));
            Assert.NotNull(exception);
            Assert.IsType<HttpException>(exception);
            Assert.Contains("Received an unexpected response for", exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task UndeserializableJsonShouldThrowException(string testCaseName, string methodName, string _)
        {
            var client = CreateClient(testCaseName, status: HttpStatusCode.OK,
                content: "{\"invalid\":yes}");

            var exception = await Record.ExceptionAsync(() => InvokeClientMethod(client, methodName));
            Assert.NotNull(exception);
            Assert.IsType<GeoIP2Exception>(exception);
            Assert.Contains("Received a 200 response but not decode it as JSON", exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task UnexpectedErrorBodyShouldThrowExceptionAsync(string testCaseName, string methodName, string _)
        {
            var client = CreateClient(testCaseName, status: HttpStatusCode.Forbidden,
                content: "{\"invalid\": }");

            var exception = await Record.ExceptionAsync(() => InvokeClientMethod(client, methodName));
            Assert.NotNull(exception);
            Assert.IsType<HttpException>(exception);
            Assert.Contains("it did not include the expected JSON body", exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task WebServiceErrorShouldThrowException(string testCaseName, string methodName, string _)
        {
            var client = CreateClient(testCaseName, status: HttpStatusCode.Forbidden,
                content: ErrorJson("IP_ADDRESS_INVALID",
                    "The value 1.2.3 is not a valid IP address"));
            var exception = await Record.ExceptionAsync(() => InvokeClientMethod(client, methodName));
            Assert.NotNull(exception);
            Assert.IsType<InvalidRequestException>(exception);
            Assert.Contains("not a valid IP address", exception.Message);
        }

        [Theory, MemberData(nameof(TestCases))]
        public async Task WeirdErrorBodyShouldThrowExceptionAsync(string testCaseName, string methodName, string _)
        {
            var client = CreateClient(testCaseName, status: HttpStatusCode.Forbidden,
                content: "{\"weird\": 42}");
            var exception = await Record.ExceptionAsync(() => InvokeClientMethod(client, methodName));
            Assert.NotNull(exception);
            Assert.IsType<HttpException>(exception);
            Assert.Contains("does not specify code or error keys", exception.Message);
        }
#pragma warning restore xUnit1026

        [Theory, MemberData(nameof(TestCases))]
        public async Task CorrectlyFormattedResponseShouldDeserializeIntoResponseObject(string testCaseName, string methodName,
            string expectedTypeName)
        {
            var client = CreateClient(testCaseName, content: CountryJson);
            var result = await InvokeClientMethod(client, methodName);

            Assert.NotNull(result);
            Assert.Equal(expectedTypeName, result.GetType().Name);
            Assert.Equal("1.2.3.0/24", result.Traits.Network?.ToString());
        }

        [Theory, MemberData(nameof(MeTestCases))]
        public async Task MeEndpointIsCalledCorrectly(string testCaseName, string methodName,
            string expectedTypeName)
        {
            var client = CreateClient(testCaseName, "me", content: CountryJson);
            var result = await InvokeMeClientMethod(client, methodName);

            Assert.NotNull(result);
            Assert.Equal(expectedTypeName, result.GetType().Name);
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
#pragma warning disable 618
            Assert.Null(location.MetroCode);
#pragma warning restore 618
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
            Assert.False(traits.IsAnycast);

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

        [Fact]
        public async Task WebServiceOptionsConstructor()
        {
            _server
              .Given(
                Request.Create()
                .WithPath("/geoip/v2.1/country/me")
                .UsingGet()
                )
              .RespondWith(
                Response.Create()
                  .WithStatusCode(HttpStatusCode.OK)
                  .WithHeader("Content-Type", "application/vnd.maxmind.com-country+json")
                  .WithBody(CountryJson)
              );

            var options = Options.Create(new WebServiceClientOptions
            {
                AccountId = 6,
                LicenseKey = "0123456789",
                Host = _server.Urls[0].Replace("http://", ""),
                DisableHttps = true,
                Timeout = 3000,
                Locales = new List<string> { "en" }
            });

            var client = new WebServiceClient(
                new HttpClient(),
                options
            );

            var result = await client.CountryAsync();

            Assert.NotNull(result);
            Assert.Equal("1.2.3.0/24", result.Traits.Network?.ToString());
        }

        #endregion

        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _server.Stop();
            }

            _disposed = true;
        }
    }
}
