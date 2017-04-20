using MaxMind.Db;
using MaxMind.GeoIP2.Exceptions;
using System;
using System.Diagnostics;
using System.Net;
using System.IO;

namespace MaxMind.GeoIP2.Benchmark
{
    public class Program
    {
        private const int Count = 200000;

        public static void Main(string[] args)
        {
            // first we check if the command-line argument is provided
            var dbPath = args.Length > 0 ? args[0] : null;
            if (dbPath != null)
            {
                if (!File.Exists(dbPath))
                {
                    throw new Exception("Path provided by command-line argument does not exist!");
                }
            }
            else
            {
                // check if environment variable MAXMIND_BENCHMARK_DB is set
                dbPath = Environment.GetEnvironmentVariable("MAXMIND_BENCHMARK_DB");

                if (!string.IsNullOrEmpty(dbPath))
                {
                    if (!File.Exists(dbPath))
                    {
                        throw new Exception("Path set as environment variable MAXMIND_BENCHMARK_DB does not exist!");
                    }
                }
                else
                {
                    // check if GeoLite2-City.mmdb exists in CWD
                    dbPath = "GeoLite2-City.mmdb";

                    if (!File.Exists(dbPath))
                    {
                        throw new Exception($"{dbPath} does not exist in current directory!");
                    }
                }
            }

            using (var reader = new DatabaseReader(dbPath, FileAccessMode.Memory))
            {
                Bench("Warm-up for Memory mode", reader);
                Bench("Memory mode", reader);
            }

            //            using (var reader = new DatabaseReader("GeoLite2-City.mmdb", FileAccessMode.MemoryMapped))
            //            {
            //                Bench("Warm-up for MMAP mode", reader);
            //                Bench("MMAP mode", reader);
            //            }
        }

        private static void Bench(string name, DatabaseReader reader)
        {
            Console.Write($"\n{name}: ");
            var rand = new Random(1);
            var s = Stopwatch.StartNew();
            for (var i = 0; i < Count; i++)
            {
                var ip = new IPAddress(rand.Next(int.MaxValue));
                try
                {
                    reader.City(ip);
                }
                catch (AddressNotFoundException) { }
            }
            s.Stop();
            Console.WriteLine("{0:N0} queries per second", Count / s.Elapsed.TotalSeconds);
        }
    }
}
