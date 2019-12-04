#if !NETSTANDARD1_4
// In .NET Core, the HttpWebRequest does not offer synchronous Send/Get mehtods.
#region

using MaxMind.GeoIP2.Exceptions;
using System;
using System.Net;
using System.Net.Http.Headers;

#endregion

namespace MaxMind.GeoIP2.Http
{
    internal class SyncClient : ISyncClient
    {
        private readonly string _auth;
        private readonly int _timeout;
        private readonly string _userAgent;

        public SyncClient(
            string auth,
            int timeout,
            ProductInfoHeaderValue userAgent
            )
        {
            _auth = auth;
            _timeout = timeout;
            _userAgent = userAgent.ToString();
        }

        public Response Get(Uri uri)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Timeout = _timeout;
            request.UserAgent = _userAgent;
            request.Headers["Authorization"] = $"Basic {_auth}";

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                if (e.Status != WebExceptionStatus.ProtocolError)
                {
                    throw new HttpException(
                        $"Error received while making request: {e.Message}",
                        0, uri, e);
                }
                response = (HttpWebResponse)e.Response;
            }
            return new Response(uri, response.StatusCode, response.ContentType,
                response.GetResponseStream());
        }
    }
}
#endif
