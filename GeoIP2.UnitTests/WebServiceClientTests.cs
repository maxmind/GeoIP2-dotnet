using System;
using System.Collections.Generic;
using System.Linq;
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
            var restResponse = new RestResponse<OmniResponse> {Content = null, ResponseUri = new Uri("http://foo.com/omni/1.2.3.4")};
            restClient.Stub(r => r.Execute<OmniResponse>(Arg<IRestRequest>.Is.Anything)).Return(restResponse);

            var wsc = new WebServiceClient(0, "abcdef", new List<string> {"en"}, restClient);
            var result = wsc.Omni("1.2.3.4");
        }


    }
}
