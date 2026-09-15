using MaxMind.GeoIP2;
using System;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

// xUnit's NativeAOT runner requires .NET 9. Keep a small .NET 8 smoke test.
if (RuntimeFeature.IsDynamicCodeSupported || System.Text.Json.JsonSerializer.IsReflectionEnabledByDefault)
{
    throw new InvalidOperationException("Run the published NativeAOT executable with JSON reflection disabled.");
}

using var reader = new DatabaseReader(System.IO.Path.Combine(AppContext.BaseDirectory, "GeoIP2-City-Test.mmdb"));
if (reader.City("81.2.69.160").City.Name != "London")
{
    throw new InvalidOperationException("The native database lookup failed.");
}

using var client = new WebServiceClient(42, "test-key", locales: ["ja"], httpMessageHandler: new ResponseHandler());
var response = await client.InsightsAsync();
if (response.City.Name != "ミネアポリス" || response.Traits.Network?.ToString() != "1.2.3.0/24")
{
    throw new InvalidOperationException("Native JSON decoding or locale copying failed.");
}
Console.WriteLine(".NET 8 NativeAOT smoke test passed.");

internal sealed class ResponseHandler : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(MaxMind.GeoIP2.UnitTests.ResponseHelper.InsightsJson,
                System.Text.Encoding.UTF8, "application/json"),
        });
    }
}
