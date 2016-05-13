#if !NETSTANDARD1_4
// In .NET Core, the HttpWebRequest does not offer synchronous Send/Get mehtods.
#region

using System;

#endregion

namespace MaxMind.GeoIP2.Http
{
    internal interface ISyncClient
    {
        Response Get(Uri uri);
    }
}
#endif
