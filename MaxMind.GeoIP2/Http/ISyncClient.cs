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
