using System;
using System.IO;
using System.Linq;

namespace MaxMind.GeoIP2.UnitTests
{
    internal static class TestUtils
    {
        public static string TestDirectory { get; } = GetTestDirectory();

        private static string GetTestDirectory()
        {
            // check if environment variable MAXMIND_TEST_BASE_DIR is set
            var dbPath = Environment.GetEnvironmentVariable("MAXMIND_TEST_BASE_DIR");

            if (!string.IsNullOrEmpty(dbPath))
            {
                if (!Directory.Exists(dbPath))
                {
                    throw new Exception("Path set as environment variable MAXMIND_TEST_BASE_DIR does not exist!");
                }

                return dbPath;
            }

            // In Microsoft.NET.Test.Sdk v15.0.0, the current working directory
            // is not set to project's root but instead the output directory.
            // see: https://github.com/Microsoft/vstest/issues/435.
            //
            // Let's change the strategry of finding the parent directory of
            // TestData directory by walking from cwd backwards upto the
            // volume's root.
            var currentDirectory = Directory.GetCurrentDirectory();
            var currentDirectoryInfo = new DirectoryInfo(currentDirectory);

            do
            {
                if (currentDirectoryInfo.EnumerateDirectories("TestData*", SearchOption.AllDirectories).Any())
                {
                    return currentDirectoryInfo.FullName;
                }
                currentDirectoryInfo = currentDirectoryInfo.Parent;
            } while (currentDirectoryInfo?.Parent != null);

            return currentDirectory;
        }
    }
}