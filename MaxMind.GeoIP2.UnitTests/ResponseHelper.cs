namespace MaxMind.GeoIP2.UnitTests
{
    internal static class ResponseHelper
    {
        public static string InsightsJson = @"
            {
                ""city"": {
                    ""confidence"": 76,
                    ""geoname_id"": 9876,
                    ""names"": {""en"": ""Minneapolis""}
                },
                ""continent"": {
                    ""code"": ""NA"",
                    ""geoname_id"": 42,
                    ""names"": {""en"": ""North America""}
                },
                ""country"": {
                    ""confidence"": 99,
                    ""iso_code"": ""US"",
                    ""geoname_id"": 1,
                    ""names"": {""en"": ""United States of America""}
                },
                ""location"": {
                    ""accuracy_radius"": 1500,
                    ""average_income"": 50000,
                    ""latitude"": 44.98,
                    ""longitude"": 93.2636,
                    ""metro_code"": 765,
                    ""population_density"": 100,
                    ""time_zone"": ""America/Chicago""
                },
                ""postal"": {
                    ""confidence"": 33,
                    ""code"": ""55401""
                },
                ""registered_country"": {
                    ""geoname_id"": 2,
                    ""is_in_european_union"": true,
                    ""iso_code"": ""DE"",
                    ""names"": {""en"": ""Germany""}
                },
                ""represented_country"": {
                    ""geoname_id"": 3,
                    ""is_in_european_union"": true,
                    ""iso_code"": ""GB"",
                    ""names"": {""en"": ""United Kingdom""},
                    ""type"": ""military""
                },
                ""subdivisions"": [
                    {
                        ""confidence"": 88,
                        ""geoname_id"": 574635,
                        ""iso_code"": ""MN"",
                        ""names"": {""en"": ""Minnesota""}
                    },
                    {""iso_code"": ""TT""}
                ],
                ""traits"": {
                    ""autonomous_system_number"": 1234,
                    ""autonomous_system_organization"": ""AS Organization"",
                    ""domain"": ""example.com"",
                    ""ip_address"": ""1.2.3.4"",
                    ""is_anonymous"": true,
                    ""is_anonymous_proxy"": true,
                    ""is_anonymous_vpn"": true,
                    ""is_hosting_provider"": true,
                    ""is_public_proxy"": true,
                    ""is_residential_proxy"": true,
                    ""is_satellite_provider"": true,
                    ""is_tor_exit_node"": true,
                    ""isp"": ""Comcast"",
                    ""mobile_country_code"": ""310"",
                    ""mobile_network_code"": ""004"",
                    ""network"": ""1.2.3.0/24"",
                    ""organization"": ""Blorg"",
                    ""static_ip_score"": 1.5,
                    ""user_count"": 1,
                    ""user_type"": ""college""
                },
                ""maxmind"": {""queries_remaining"": 11}
            }";

        public static string CountryJson = @"
            {
                ""continent"": {
                    ""code"": ""NA"",
                    ""geoname_id"": 42,
                    ""names"": {""en"": ""North America""}
                },
                ""country"": {
                    ""geoname_id"": 1,
                    ""iso_code"": ""US"",
                    ""confidence"": 56,
                    ""names"": {""en"": ""United States""}
                },
                ""registered_country"": {
                    ""geoname_id"": 2,
                    ""is_in_european_union"": true ,
                    ""iso_code"": ""DE"",
                    ""names"": {""en"": ""Germany""}
                },
                ""represented_country"": {
                    ""geoname_id"": 4,
                    ""is_in_european_union"": true ,
                    ""iso_code"": ""GB"",
                    ""names"": {""en"": ""United Kingdom""},
                    ""type"": ""military""
                },
                ""traits"": {
                    ""ip_address"": ""1.2.3.4"",
                    ""network"": ""1.2.3.0/24""
                }
            }";

        public static string ErrorJson(string code, string message)
        {
            return $@"{{""code"": ""{code}"", ""error"": ""{message}""}}";
        }
    }
}
