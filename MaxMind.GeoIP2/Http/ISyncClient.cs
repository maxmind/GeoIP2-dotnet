#region

using System;

#endregion

#if NETSTANDARD2_0 || NETSTANDARD2_1
namespace MaxMind.GeoIP2.Http
{
    internal interface ISyncClient
    {
        Response Get(Uri uri);
    }
}
#endif