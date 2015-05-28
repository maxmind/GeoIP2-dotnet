using Newtonsoft.Json.Linq;

namespace MaxMind.GeoIP2.UnitTests
{
    class ResponseHelper
    {
        public static JObject InsightsJObject => new JObject
        {
            {
                "city", new JObject
                {
                    {"confidence", 76},
                    {"geoname_id", 9876},
                    {
                        "names", new JObject
                        {
                            { "en","Minneapolis"}
                        }
                    },
                }
            },
            {
                "continent", new JObject
                {
                    {"code", "NA"},
                    {"geoname_id", 42},
                    {
                        "names", new JObject
                        {
                            {"en", "North America"}
                        }
                    }
                }
            },
            {
                "country", new JObject
                {
                    {"confidence", 99},
                    {"iso_code", "US"},
                    {"geoname_id", 1},
                    {
                        "names", new JObject
                        {
                            {"en", "United States of America"}
                        }
                    }
                }
            },
            {
                "location", new JObject
                {
                    {"accuracy_radius", 1500},
                    {"latitude", 44.98},
                    {"longitude", 93.2636},
                    {"metro_code", 765},
                    {"time_zone", "America/Chicago"},
                    {"average_income", 50000 },
                    {"estimated_population", 100 }
                }
            },
            {
                "postal", new JObject
                {
                    {
                        "confidence",
                        33
                    },
                    {"code", "55401"},
                }
            },
            {
                "registered_country", new JObject
                {
                    {"geoname_id", 2},
                    {"iso_code", "CA"},
                    {"names", new JObject {{"en", "Canada"}}}
                }
            },
            {
                "represented_country", new JObject
                {
                    {"geoname_id", 3},
                    {"iso_code", "GB"},
                    {
                        "names", new JObject
                        {
                            {"en", "United Kingdom"}
                        }
                    },
                    {"type", "military"}
                }
            },
            {
                "subdivisions", new JArray
                {
                    new JObject
                    {
                        {"confidence", 88},
                        {"geoname_id", 574635},
                        {"iso_code", "MN"},
                        {"names", new JObject {{"en", "Minnesota"}}},
                    }
                    ,
                    new JObject {{"iso_code", "TT"}}
                }
            },
            {
                "traits", new JObject
                {
                    {"autonomous_system_number", 1234},
                    {"autonomous_system_organization", "AS Organization"},
                    {"domain", "example.com"},
                    {"ip_address", "1.2.3.4"},
                    {"is_anonymous_proxy", true},
                    {"is_satellite_provider", true},
                    {"isp", "Comcast"},
                    {"organization", "Blorg"},
                    {"user_type", "college"}
                }
            },
            {"maxmind", new JObject {{"queries_remaining", 11}}}
        };

        public static string InsightsJson => InsightsJObject.ToString();

        public static JObject CountryJObject => new JObject
        {
            {
                "continent", new JObject
                {
                    {"code", "NA"},
                    {"geoname_id", 42},
                    {"names", new JObject {{"en", "North America"}}}
                }
            },
            {
                "country",
                new JObject
                {
                    {"geoname_id", 1},
                    {"iso_code", "US"},
                    {"confidence", 56},
                    {"names", new JObject {{"en", "United States"}}}
                }
            },
            {
                "registered_country",
                new JObject {{"geoname_id", 2}, {"iso_code", "CA"}, {"names", new JObject {{"en", "Canada"}}}}
            },
            {
                "represented_country",
                new JObject
                {
                    {"geoname_id", 4},
                    {"iso_code", "GB"},
                    {"names", new JObject {{"en", "United Kingdom"}}},
                    {"type", "military"}
                }
            },

            {"traits", new JObject {{"ip_address", "1.2.3.4"}}}
        };

        public static string CountryJson => CountryJObject.ToString();
    }
}
