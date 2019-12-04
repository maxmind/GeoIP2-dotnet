#region

using MaxMind.GeoIP2.Responses;
using Newtonsoft.Json;
using Xunit;
using static MaxMind.GeoIP2.UnitTests.ResponseHelper;

#endregion

namespace MaxMind.GeoIP2.UnitTests
{
    public class DeserializationTests
    {
        private void CanDeserializeCountryResponse(CountryResponse resp)
        {
            Assert.Equal("NA", resp.Continent.Code);
            Assert.Equal(42, resp.Continent.GeoNameId);
            Assert.Equal("North America", resp.Continent.Name);

            Assert.Equal(1, resp.Country.GeoNameId);
            Assert.False(resp.Country.IsInEuropeanUnion);
            Assert.Equal("US", resp.Country.IsoCode);
            Assert.Equal(56, resp.Country.Confidence);
            Assert.Equal("United States", resp.Country.Name);

            Assert.Equal(2, resp.RegisteredCountry.GeoNameId);
            Assert.True(resp.RegisteredCountry.IsInEuropeanUnion);
            Assert.Equal("DE", resp.RegisteredCountry.IsoCode);
            Assert.Equal("Germany", resp.RegisteredCountry.Name);

            Assert.Equal(4, resp.RepresentedCountry.GeoNameId);
            Assert.True(resp.RepresentedCountry.IsInEuropeanUnion);
            Assert.Equal("GB", resp.RepresentedCountry.IsoCode);
            Assert.Equal("United Kingdom", resp.RepresentedCountry.Name);
            Assert.Equal("military", resp.RepresentedCountry.Type);

            Assert.Equal("1.2.3.4", resp.Traits.IPAddress);
        }

        private void CanDeserializeInsightsResponse(InsightsResponse insights)
        {
            Assert.Equal(76, insights.City.Confidence);
            Assert.Equal(9876, insights.City.GeoNameId);
            Assert.Equal("Minneapolis", insights.City.Name);

            Assert.Equal("NA", insights.Continent.Code);
            Assert.Equal(42, insights.Continent.GeoNameId);
            Assert.Equal("North America", insights.Continent.Name);

            Assert.Equal(99, insights.Country.Confidence);
            Assert.Equal(1, insights.Country.GeoNameId);
            Assert.False(insights.Country.IsInEuropeanUnion);
            Assert.Equal("US", insights.Country.IsoCode);
            Assert.Equal("United States of America", insights.Country.Name);

            Assert.Equal(1500, insights.Location.AccuracyRadius);
            Assert.Equal(44.979999999999997, insights.Location.Latitude);
            Assert.Equal(93.263599999999997, insights.Location.Longitude);
            Assert.Equal(765, insights.Location.MetroCode);
            Assert.Equal("America/Chicago", insights.Location.TimeZone);
            Assert.Equal(50000, insights.Location.AverageIncome);
            Assert.Equal(100, insights.Location.PopulationDensity);

            Assert.Equal(11, insights.MaxMind.QueriesRemaining);

            Assert.Equal("55401", insights.Postal.Code);
            Assert.Equal(33, insights.Postal.Confidence);

            Assert.Equal(2, insights.RegisteredCountry.GeoNameId);
            Assert.True(insights.RegisteredCountry.IsInEuropeanUnion);
            Assert.Equal("DE", insights.RegisteredCountry.IsoCode);
            Assert.Equal("Germany", insights.RegisteredCountry.Name);

            Assert.Equal(3, insights.RepresentedCountry.GeoNameId);
            Assert.True(insights.RepresentedCountry.IsInEuropeanUnion);
            Assert.Equal("GB", insights.RepresentedCountry.IsoCode);
            Assert.Equal("United Kingdom", insights.RepresentedCountry.Name);
            Assert.Equal("military", insights.RepresentedCountry.Type);

            Assert.Equal(2, insights.Subdivisions.Count);
            Assert.Equal(88, insights.Subdivisions[0].Confidence);
            Assert.Equal(574635, insights.Subdivisions[0].GeoNameId);
            Assert.Equal("MN", insights.Subdivisions[0].IsoCode);
            Assert.Equal("Minnesota", insights.Subdivisions[0].Name);
            Assert.Equal("TT", insights.Subdivisions[1].IsoCode);

            Assert.Equal(1234, insights.Traits.AutonomousSystemNumber);
            Assert.Equal("AS Organization", insights.Traits.AutonomousSystemOrganization);
            Assert.Equal("example.com", insights.Traits.Domain);
            Assert.Equal("1.2.3.4", insights.Traits.IPAddress);
            Assert.True(insights.Traits.IsAnonymous);
            Assert.True(insights.Traits.IsAnonymousVpn);
            Assert.True(insights.Traits.IsHostingProvider);
            Assert.True(insights.Traits.IsPublicProxy);
            Assert.True(insights.Traits.IsTorExitNode);
#pragma warning disable 0618
            Assert.True(insights.Traits.IsAnonymousProxy);
            Assert.True(insights.Traits.IsSatelliteProvider);
#pragma warning restore 0618
            Assert.Equal("Comcast", insights.Traits.Isp);

            var network = insights.Traits.Network;
            Assert.Equal("1.2.3.0", network.NetworkAddress.ToString());
            Assert.Equal(24, network.PrefixLength);

            Assert.Equal("Blorg", insights.Traits.Organization);
            Assert.Equal(1.5, insights.Traits.StaticIPScore);
            Assert.Equal(1, insights.Traits.UserCount);
            Assert.Equal("college", insights.Traits.UserType);
        }

        [Fact]
        public void CanDeserializeCountryResponseNewtonsoftJson()
        {
            CanDeserializeCountryResponse(JsonConvert.DeserializeObject<CountryResponse>(CountryJson, new NetworkConverter()));
        }

        [Fact]
        public void CanDeserializeInsightsResponseNewtonsoftJson()
        {
            CanDeserializeInsightsResponse(JsonConvert.DeserializeObject<InsightsResponse>(InsightsJson, new NetworkConverter()));
        }
    }
}
