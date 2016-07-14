using System;
using System.IO;

namespace MaxMind.GeoIP2.UnitTests
{
    internal static class TestUtils
    {
        public static string TestDirectory { get; } = GetTestDirectory();

        private static string GetTestDirectory()
        {
            // check if environment variable MAXMIND_TEST_BASE_DIR is set
            string dbPath = Environment.GetEnvironmentVariable("MAXMIND_TEST_BASE_DIR");

            if (!string.IsNullOrEmpty(dbPath))
            {
                if (!Directory.Exists(dbPath))
                {
                    throw new Exception("Path set as environment variable MAXMIND_TEST_BASE_DIR does not exist!");
                }

                return dbPath;
            }

#if !NETCOREAPP1_0
            return Environment.CurrentDirectory;
#else
            return Directory.GetCurrentDirectory();
#endif
        }
    }
}
