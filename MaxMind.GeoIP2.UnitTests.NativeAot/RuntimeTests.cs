using System.Runtime.CompilerServices;
using System.Text.Json;
using Xunit;

namespace MaxMind.GeoIP2.UnitTests
{
    public class RuntimeTests
    {
        [Fact]
        public void RunsAsNativeAotWithoutJsonReflection()
        {
            Assert.False(RuntimeFeature.IsDynamicCodeSupported);
            Assert.False(JsonSerializer.IsReflectionEnabledByDefault);
        }
    }
}
