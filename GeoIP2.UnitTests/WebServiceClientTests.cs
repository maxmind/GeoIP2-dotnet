using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using MaxMind.GeoIP2.Exceptions;
using MaxMind.GeoIP2.Responses;
using NUnit.Framework;
using RestSharp;
using Rhino.Mocks;

namespace MaxMind.GeoIP2.UnitTests
{
    [TestFixture]
    public class WebServiceClientTests
    {
        public void RunClientGivenResponse(RestResponse<OmniResponse> response)
        {
            var restClient = MockRepository.GenerateStub<IRestClient>();

            restClient.Stub(r => r.Execute<OmniResponse>(Arg<IRestRequest>.Is.Anything)).Return(response);

            var wsc = new WebServiceClient(0, "abcdef", new List<string> {"en"}, restClient);
            var result = wsc.Omni("1.2.3.4");
        }

        [Test]
        [ExpectedException(typeof(GeoIP2HttpException), ExpectedMessage = "message body", MatchType = MessageMatch.Contains)]
        public void EmptyBodyShouldThrowException()
        {
            var restResponse = new RestResponse<OmniResponse>
            {
                Content = null, 
                ResponseUri = new Uri("http://foo.com/omni/1.2.3.4"), 
                StatusCode = (HttpStatusCode)200
            };

            RunClientGivenResponse(restResponse);
        }

        [Test]
        [ExpectedException(typeof (GeoIP2InvalidRequestException), ExpectedMessage = "not a valid ip address",
            MatchType = MessageMatch.Contains)]
        public void WebServiceErrorShouldThrowException()
        {
            var restResponse = new RestResponse<OmniResponse>
            {
                Content = "{\"code\":\"IP_ADDRESS_INVALID\","
                          + "\"error\":\"The value 1.2.3 is not a valid ip address\"}",
                ResponseUri = new Uri("http://foo.com/omni/1.2.3"),
                StatusCode = (HttpStatusCode)400
            };

            RunClientGivenResponse(restResponse);
        }

        [Test]
        [ExpectedException(typeof (GeoIP2HttpException), ExpectedMessage = "with no body",
            MatchType = MessageMatch.Contains)]
        public void NoErrorBodyShouldThrowException()
        {
            var restResponse = new RestResponse<OmniResponse>
            {
                Content = null, 
                ResponseUri = new Uri("http://foo.com/omni/1.2.3.4"), 
                StatusCode = (HttpStatusCode)400
            };

            RunClientGivenResponse(restResponse);
        }

        [Test]
        [ExpectedException(typeof (GeoIP2HttpException), ExpectedMessage = "does not specify code or error keys",
            MatchType = MessageMatch.Contains)]
        public void WeirdErrorBodyShouldThrowException()
        {
            var restResponse = new RestResponse<OmniResponse>
            {
                Content = "{\"weird\": 42}", 
                ResponseUri = new Uri("http://foo.com/omni/1.2.3.4"), 
                StatusCode = (HttpStatusCode)400
            };

            RunClientGivenResponse(restResponse);
        }

        [Test]
        [ExpectedException(typeof (GeoIP2HttpException), ExpectedMessage = "it did not include the expected JSON body",
            MatchType = MessageMatch.Contains)]
        public void UnexpectedErrorBodyShouldThrowException()
        {
            var restResponse = new RestResponse<OmniResponse>
            {
                Content = "{\"invalid\": }", 
                ResponseUri = new Uri("http://foo.com/omni/1.2.3.4"), 
                StatusCode = (HttpStatusCode)400
            };

            RunClientGivenResponse(restResponse);
        }

        [Test]
        [ExpectedException(typeof (GeoIP2HttpException), ExpectedMessage = "Received a server (500) error", MatchType = MessageMatch.Contains),]
        public void InternalServerErrorShouldThrowException()
        {
            var restResponse = new RestResponse<OmniResponse>
            {
                ResponseUri = new Uri("http://foo.com/omni/1.2.3.4"), 
                StatusCode = (HttpStatusCode)500
            };

            RunClientGivenResponse(restResponse);
        }

        [Test]
        [ExpectedException(typeof (GeoIP2HttpException), ExpectedMessage = "Received a very surprising HTTP status (300) for", MatchType = MessageMatch.Contains),]
        public void SurprisingStatusShouldThrowException()
        {
            var restResponse = new RestResponse<OmniResponse>
            {
                ResponseUri = new Uri("http://foo.com/omni/1.2.3.4"), 
                StatusCode = (HttpStatusCode)300
            };

            RunClientGivenResponse(restResponse);
        }

        [Test]
        [ExpectedException(typeof (GeoIP2HttpException), ExpectedMessage = "Cannot satisfy your Accept-Charset requirements", MatchType = MessageMatch.Contains),]
        public void BadCharsetRequirementShouldThrowException()
        {
            var restResponse = new RestResponse<OmniResponse>
            {
                Content = "Cannot satisfy your Accept-Charset requirements",
                ContentType = "text/plain",
                ResponseUri = new Uri("http://foo.com/omni/1.2.3.4"), 
                StatusCode = (HttpStatusCode)406
            };

            RunClientGivenResponse(restResponse);
        }
    }
}
