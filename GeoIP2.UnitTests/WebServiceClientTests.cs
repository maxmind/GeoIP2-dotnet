using MaxMind.GeoIP2.Exceptions;
using MaxMind.GeoIP2.Model;
using MaxMind.GeoIP2.Responses;
using NUnit.Framework;
using RestSharp;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Net;
using static MaxMind.GeoIP2.UnitTests.ResponseHelper;

namespace MaxMind.GeoIP2.UnitTests
{
    [TestFixture]
    public class WebServiceClientTests
    {
        public InsightsResponse RunClientGivenResponse(RestResponse response)
        {
            response.ContentLength = response.Content.Length;

            var restClient = MockRepository.GenerateStub<IRestClient>();

            restClient.Stub(r => r.Execute(Arg<IRestRequest>.Is.Anything)).Return(response);

            var wsc = new WebServiceClient(0, "abcdef", new List<string> { "en" });
            return wsc.Insights("1.2.3.4", restClient);
        }

        [Test]
        [ExpectedException(typeof(AddressNotFoundException),
            ExpectedMessage = "The value 1.2.3.16 is not in the database", MatchType = MessageMatch.Contains)]
        public void AddressNotFoundShouldThrowException()
        {
            var restResponse = new RestResponse
            {
                Content =
                    "{\"code\":\"IP_ADDRESS_NOT_FOUND\", \"error\":\"The value 1.2.3.16 is not in the database.\"}",
                ResponseUri = new Uri("http://foo.com/insights/1.2.3.4"),
                StatusCode = (HttpStatusCode)404
            };

            RunClientGivenResponse(restResponse);
        }

        [Test]
        [ExpectedException(typeof(AddressNotFoundException),
            ExpectedMessage = "The value 1.2.3.17 belongs to a reserved or private range",
            MatchType = MessageMatch.Contains)]
        public void AddressReservedShouldThrowException()
        {
            var restResponse = new RestResponse
            {
                Content =
                    "{\"code\":\"IP_ADDRESS_RESERVED\",\"error\":\"The value 1.2.3.17 belongs to a reserved or private range.\"}",
                ResponseUri = new Uri("http://foo.com/insights/1.2.3.4"),
                StatusCode = (HttpStatusCode)400
            };

            RunClientGivenResponse(restResponse);
        }

        [Test]
        [ExpectedException(typeof(HttpException), ExpectedMessage = "Cannot satisfy your Accept-Charset requirements",
            MatchType = MessageMatch.Contains)]
        public void BadCharsetRequirementShouldThrowException()
        {
            var restResponse = new RestResponse
            {
                Content = "Cannot satisfy your Accept-Charset requirements",
                ContentType = "text/plain",
                ResponseUri = new Uri("http://foo.com/insights/1.2.3.4"),
                StatusCode = (HttpStatusCode)406
            };

            RunClientGivenResponse(restResponse);
        }

        [Test]
        [ExpectedException(typeof(GeoIP2Exception), ExpectedMessage = "but it does not appear to be JSON",
            MatchType = MessageMatch.Contains)]
        public void BadContentTypeShouldThrowException()
        {
            var restResponse = new RestResponse
            {
                Content = CountryJson,
                ContentType = "bad/content-type",
                ResponseUri = new Uri("http://foo.com/insights/1.2.3.4"),
                StatusCode = HttpStatusCode.OK
            };

            RunClientGivenResponse(restResponse);
        }

        [Test]
        public void CorrectlyFormattedCityResponseShouldDeserializeIntoResponseObject()
        {
            var restResponse = new RestResponse
            {
                Content = InsightsJson,
                ContentType = "application/json",
                ResponseUri = new Uri("http://foo.com/insights/1.2.3.4"),
                StatusCode = HttpStatusCode.OK
            };

            restResponse.ContentLength = restResponse.Content.Length;

            var restClient = MockRepository.GenerateStub<IRestClient>();

            restClient.Stub(r => r.Execute(Arg<IRestRequest>.Is.Anything)).Return(restResponse);

            var wsc = new WebServiceClient(0, "abcdef", new List<string> { "en" });
            var result = wsc.City("1.2.3.4", restClient);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<CityResponse>());
        }

        [Test]
        public void CorrectlyFormattedCountryResponseShouldDeserializeIntoResponseObject()
        {
            var restResponse = new RestResponse
            {
                Content = InsightsJson,
                ContentType = "application/json",
                ResponseUri = new Uri("http://foo.com/insights/1.2.3.4"),
                StatusCode = HttpStatusCode.OK
            };

            restResponse.ContentLength = restResponse.Content.Length;

            var restClient = MockRepository.GenerateStub<IRestClient>();

            restClient.Stub(r => r.Execute(Arg<IRestRequest>.Is.Anything)).Return(restResponse);

            var wsc = new WebServiceClient(0, "abcdef", new List<string> { "en" });
            var result = wsc.Country("1.2.3.4", restClient);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<CountryResponse>());
        }

        [Test]
        public void CorrectlyFormattedInsightsResponseShouldDeserializeIntoResponseObject()
        {
            var restResponse = new RestResponse
            {
                Content = InsightsJson,
                ContentType = "application/json",
                ResponseUri = new Uri("http://foo.com/insights/1.2.3.4"),
                StatusCode = HttpStatusCode.OK
            };

            var result = RunClientGivenResponse(restResponse);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<InsightsResponse>());
        }

        [Test]
        [ExpectedException(typeof(HttpException), ExpectedMessage = "message body", MatchType = MessageMatch.Contains)]
        public void EmptyBodyShouldThrowException()
        {
            var restResponse = new RestResponse
            {
                Content = null,
                ResponseUri = new Uri("http://foo.com/insights/1.2.3.4"),
                StatusCode = HttpStatusCode.OK
            };

            RunClientGivenResponse(restResponse);
        }

        [Test]
        [ExpectedException(typeof(HttpException),
            ExpectedMessage = "Error received while making request: Internal error message",
            MatchType = MessageMatch.Exact)]
        public void ErrorExceptionSetShouldThrowException()
        {
            var restResponse = new RestResponse
            {
                ResponseUri = new Uri("http://foo.com/insights/1.2.3.4"),
                StatusCode = HttpStatusCode.OK,
                ErrorException = new Exception("fake exception"),
                ErrorMessage = "Internal error message",
                ResponseStatus = ResponseStatus.Error
            };

            RunClientGivenResponse(restResponse);
        }

        [Test]
        [ExpectedException(typeof(GeoIP2Exception),
            ExpectedMessage = "The specified IP address was incorrectly formatted", MatchType = MessageMatch.Contains)]
        public void IncorrectlyFormattedIPAddressShouldThrowException()
        {
            var client = new WebServiceClient(0, "abcde", new List<string> { "en" });
            client.Insights("foo");
        }

        [Test]
        [ExpectedException(typeof(HttpException), ExpectedMessage = "Received a server (500) error",
            MatchType = MessageMatch.Contains)]
        public void InternalServerErrorShouldThrowException()
        {
            var restResponse = new RestResponse
            {
                ResponseUri = new Uri("http://foo.com/insights/1.2.3.4"),
                StatusCode = HttpStatusCode.InternalServerError
            };

            RunClientGivenResponse(restResponse);
        }

        [Test]
        [ExpectedException(typeof(AuthenticationException),
            ExpectedMessage = "You have supplied an invalid MaxMind user ID and/or license key",
            MatchType = MessageMatch.Contains)]
        public void InvalidAuthShouldThrowException()
        {
            var restResponse = new RestResponse
            {
                Content =
                    "{\"code\":\"AUTHORIZATION_INVALID\",\"error\":\"You have supplied an invalid MaxMind user ID and/or license key in the Authorization header.\"}",
                ResponseUri = new Uri("http://foo.com/insights/1.2.3.4"),
                StatusCode = (HttpStatusCode)401
            };

            RunClientGivenResponse(restResponse);
        }

        [Test]
        public void MissingKeys()
        {
            var restResponse = new RestResponse
            {
                Content = "{}",
                ContentType = "application/json",
                ResponseUri = new Uri("http://foo.com/insights/1.2.3.4"),
                StatusCode = HttpStatusCode.OK
            };

            var insights = RunClientGivenResponse(restResponse);

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

        [Test]
        [ExpectedException(typeof(AuthenticationException),
            ExpectedMessage = "You have not supplied a MaxMind license key in the Authorization header",
            MatchType = MessageMatch.Contains)]
        public void MissingLicenseShouldThrowException()
        {
            var restResponse = new RestResponse
            {
                Content =
                    "{\"code\":\"LICENSE_KEY_REQUIRED\",\"error\":\"You have not supplied a MaxMind license key in the Authorization header.\"}",
                ResponseUri = new Uri("http://foo.com/insights/1.2.3.4"),
                StatusCode = (HttpStatusCode)401
            };

            RunClientGivenResponse(restResponse);
        }

        [Test]
        [ExpectedException(typeof(AuthenticationException),
            ExpectedMessage = "You have not supplied a MaxMind user ID in the Authorization header",
            MatchType = MessageMatch.Contains)]
        public void MissingUserIDShouldThrowException()
        {
            var restResponse = new RestResponse
            {
                Content =
                    "{\"code\":\"USER_ID_REQUIRED\",\"error\":\"You have not supplied a MaxMind user ID in the Authorization header.\"}",
                ResponseUri = new Uri("http://foo.com/insights/1.2.3.4"),
                StatusCode = (HttpStatusCode)401
            };

            RunClientGivenResponse(restResponse);
        }

        [Test]
        [ExpectedException(typeof(HttpException), ExpectedMessage = "with no body",
            MatchType = MessageMatch.Contains)]
        public void NoErrorBodyShouldThrowException()
        {
            var restResponse = new RestResponse
            {
                Content = null,
                ResponseUri = new Uri("http://foo.com/insights/1.2.3.4"),
                StatusCode = (HttpStatusCode)400
            };

            RunClientGivenResponse(restResponse);
        }

        [Test]
        [ExpectedException(typeof(OutOfQueriesException),
            ExpectedMessage = "The license key you have provided is out of queries", MatchType = MessageMatch.Contains)]
        public void OutOfQueriesShouldThrowException()
        {
            var restResponse = new RestResponse
            {
                Content =
                    "{\"code\":\"OUT_OF_QUERIES\",\"error\":\"The license key you have provided is out of queries. Please purchase more queries to use this service.\"}",
                ResponseUri = new Uri("http://foo.com/insights/1.2.3.4"),
                StatusCode = (HttpStatusCode)402
            };

            RunClientGivenResponse(restResponse);
        }

        [Test]
        [ExpectedException(typeof(HttpException),
            ExpectedMessage = "Received an unexpected response for http://foo.com/insights/1.2.3.4 (status code: 300)",
            MatchType = MessageMatch.Exact)]
        public void SurprisingStatusShouldThrowException()
        {
            var restResponse = new RestResponse
            {
                ResponseUri = new Uri("http://foo.com/insights/1.2.3.4"),
                StatusCode = (HttpStatusCode)300
            };

            RunClientGivenResponse(restResponse);
        }

        [Test]
        [ExpectedException(typeof(GeoIP2Exception),
            ExpectedMessage = "Received a 200 response but not decode it as JSON", MatchType = MessageMatch.Contains)]
        public void UndeserializableJsonShouldThrowException()
        {
            var restResponse = new RestResponse
            {
                Content = "{\"invalid\":yes}",
                ContentType = "application/json",
                ResponseUri = new Uri("http://foo.com/insights/1.2.3.4"),
                StatusCode = (HttpStatusCode)200
            };

            RunClientGivenResponse(restResponse);
        }

        [Test]
        [ExpectedException(typeof(HttpException), ExpectedMessage = "it did not include the expected JSON body",
            MatchType = MessageMatch.Contains)]
        public void UnexpectedErrorBodyShouldThrowException()
        {
            var restResponse = new RestResponse
            {
                Content = "{\"invalid\": }",
                ResponseUri = new Uri("http://foo.com/insights/1.2.3.4"),
                StatusCode = (HttpStatusCode)400
            };

            RunClientGivenResponse(restResponse);
        }

        [Test]
        [ExpectedException(typeof(InvalidRequestException), ExpectedMessage = "not a valid ip address",
            MatchType = MessageMatch.Contains)]
        public void WebServiceErrorShouldThrowException()
        {
            var restResponse = new RestResponse
            {
                Content = "{\"code\":\"IP_ADDRESS_INVALID\","
                          + "\"error\":\"The value 1.2.3 is not a valid ip address\"}",
                ResponseUri = new Uri("http://foo.com/insights/1.2.3"),
                StatusCode = (HttpStatusCode)400
            };

            RunClientGivenResponse(restResponse);
        }

        [Test]
        [ExpectedException(typeof(HttpException), ExpectedMessage = "does not specify code or error keys",
            MatchType = MessageMatch.Contains)]
        public void WeirdErrorBodyShouldThrowException()
        {
            var restResponse = new RestResponse
            {
                Content = "{\"weird\": 42}",
                ResponseUri = new Uri("http://foo.com/insights/1.2.3.4"),
                StatusCode = (HttpStatusCode)400
            };

            RunClientGivenResponse(restResponse);
        }
    }
}