using MaxMind.GeoIP2.Model;
using MaxMind.GeoIP2.Responses;
using System.Collections.Generic;
using Xunit;

namespace MaxMind.GeoIP2.UnitTests
{
    public class ResponseTests
    {
        public static TheoryData<AbstractCountryResponse> LocaleResponses => new()
        {
            new CountryResponse(),
            new CityResponse(),
            new InsightsResponse(),
        };

        [Theory]
        [MemberData(nameof(LocaleResponses))]
        public void WithLocalesPreservesTypeAndOriginalResponse(AbstractCountryResponse response)
        {
            response = response with
            {
                Country = new Country { Names = new Dictionary<string, string> { ["en"] = "English", ["ja"] = "Japanese" } },
            };
            if (response is AbstractCityResponse city)
            {
                response = city with
                {
                    City = new City { Names = response.Country.Names },
                    Subdivisions = [new Subdivision { Names = response.Country.Names }],
                };
            }

            var copy = (AbstractCountryResponse)response.WithLocales(["ja"]);
            Assert.Equal(response.GetType(), copy.GetType());
            Assert.NotSame(response, copy);
            Assert.Equal("English", response.Country.Name);
            Assert.Equal("Japanese", copy.Country.Name);
            Assert.NotSame(response.Country, copy.Country);
            Assert.Same(response.Traits, copy.Traits);

            if (response is AbstractCityResponse originalCity)
            {
                var copiedCity = (AbstractCityResponse)copy;
                Assert.Equal("English", originalCity.City.Name);
                Assert.Equal("Japanese", copiedCity.City.Name);
                Assert.Equal("English", originalCity.Subdivisions[0].Name);
                Assert.Equal("Japanese", copiedCity.Subdivisions[0].Name);
                Assert.NotSame(originalCity.Subdivisions, copiedCity.Subdivisions);
            }

            if (response is InsightsResponse originalInsights)
            {
                Assert.Same(originalInsights.Anonymizer, ((InsightsResponse)copy).Anonymizer);
            }
        }

        [Fact]
        public void InsightsConstruction()
        {
            var city = new City();
            var insightsReponse = new InsightsResponse { City = city };

            Assert.Equal(insightsReponse.City, city);
        }
    }
}
