#region

using MaxMind.GeoIP2.Http;
using System;

#endregion

namespace MaxMind.GeoIP2.UnitTests.Mock
{
    internal class MockSyncClient : ISyncClient
    {
        private readonly Response _response;

        public MockSyncClient(Response response)
        {
            _response = response;
        }

        public Response Get(Uri uri)
        {
            return _response;
        }
    }
}