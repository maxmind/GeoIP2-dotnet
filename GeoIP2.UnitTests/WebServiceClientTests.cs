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
        [Test]
        [ExpectedException(typeof(GeoIP2HttpException), ExpectedMessage = "message body", MatchType = MessageMatch.Contains)]
        public void EmptyBodyShouldThrowException()
        {
            var restClient = MockRepository.GenerateStub<IRestClient>();
            var restResponse = new RestResponse<OmniResponse>
            {
                Content = null, 
                ResponseUri = new Uri("http://foo.com/omni/1.2.3.4"), 
                StatusCode = (HttpStatusCode)200
            };
            restClient.Stub(r => r.Execute<OmniResponse>(Arg<IRestRequest>.Is.Anything)).Return(restResponse);

            var wsc = new WebServiceClient(0, "abcdef", new List<string> {"en"}, restClient);
            var result = wsc.Omni("1.2.3.4");
        }

        [Test]
        [ExpectedException(typeof (GeoIP2InvalidRequestException), ExpectedMessage = "not a valid ip address",
            MatchType = MessageMatch.Contains)]
        public void WebServiceErrorShouldThrowException()
        {
            var restClient = MockRepository.GenerateStub<IRestClient>();
            var content = "{\"code\":\"IP_ADDRESS_INVALID\","
                            + "\"error\":\"The value 1.2.3 is not a valid ip address\"}";
            var restResponse = new RestResponse<OmniResponse>
            {
                Content = content,
                ResponseUri = new Uri("http://foo.com/omni/1.2.3"),
                StatusCode = (HttpStatusCode)400
            };
            restClient.Stub(r => r.Execute<OmniResponse>(Arg<IRestRequest>.Is.Anything)).Return(restResponse);

            var wsc = new WebServiceClient(0, "abcdef", new List<string> {"en"}, restClient);
            var result = wsc.Omni("1.2.3");
        }

        [Test]
        [ExpectedException(typeof (GeoIP2HttpException), ExpectedMessage = "with no body",
            MatchType = MessageMatch.Contains)]
        public void NoErrorBodyShouldThrowException()
        {
            var restClient = MockRepository.GenerateStub<IRestClient>();
            var restResponse = new RestResponse<OmniResponse>
            {
                Content = null, 
                ResponseUri = new Uri("http://foo.com/omni/1.2.3.4"), 
                StatusCode = (HttpStatusCode)400
            };
            restClient.Stub(r => r.Execute<OmniResponse>(Arg<IRestRequest>.Is.Anything)).Return(restResponse);

            var wsc = new WebServiceClient(0, "abcdef", new List<string> {"en"}, restClient);
            var result = wsc.Omni("1.2.3.4");
        }

        [Test]
        [ExpectedException(typeof (GeoIP2HttpException), ExpectedMessage = "does not specify code or error keys",
            MatchType = MessageMatch.Contains)]
        public void WeirdErrorBodyShouldThrowException()
        {
            var restClient = MockRepository.GenerateStub<IRestClient>();
            var restResponse = new RestResponse<OmniResponse>
            {
                Content = "{\"weird\": 42}", 
                ResponseUri = new Uri("http://foo.com/omni/1.2.3.4"), 
                StatusCode = (HttpStatusCode)400
            };
            restClient.Stub(r => r.Execute<OmniResponse>(Arg<IRestRequest>.Is.Anything)).Return(restResponse);

            var wsc = new WebServiceClient(0, "abcdef", new List<string> {"en"}, restClient);
            var result = wsc.Omni("1.2.3.4");
        }

        [Test]
        [ExpectedException(typeof (GeoIP2HttpException), ExpectedMessage = "it did not include the expected JSON body",
            MatchType = MessageMatch.Contains)]
        public void UnexpectedErrorBodyShouldThrowException()
        {
            var restClient = MockRepository.GenerateStub<IRestClient>();
            var restResponse = new RestResponse<OmniResponse>
            {
                Content = "{\"invalid\": }", 
                ResponseUri = new Uri("http://foo.com/omni/1.2.3.4"), 
                StatusCode = (HttpStatusCode)400
            };
            restClient.Stub(r => r.Execute<OmniResponse>(Arg<IRestRequest>.Is.Anything)).Return(restResponse);

            var wsc = new WebServiceClient(0, "abcdef", new List<string> {"en"}, restClient);
            var result = wsc.Omni("1.2.3.4");
        }
    }
}
