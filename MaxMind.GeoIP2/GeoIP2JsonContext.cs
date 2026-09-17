using MaxMind.GeoIP2.Model;
using MaxMind.GeoIP2.Responses;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MaxMind.GeoIP2
{
    [JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
    [JsonSerializable(typeof(CountryResponse))]
    [JsonSerializable(typeof(CityResponse))]
    [JsonSerializable(typeof(InsightsResponse))]
    [JsonSerializable(typeof(WebServiceError))]
    internal partial class GeoIP2JsonContext : JsonSerializerContext
    {
        // A converter array in JsonSourceGenerationOptions triggers CS3016 in
        // this CLS-compliant assembly. Register converters through options instead.
        internal static GeoIP2JsonContext Shared { get; } = new(new JsonSerializerOptions
        {
            Converters = { new NetworkConverter() },
        });
    }
}
