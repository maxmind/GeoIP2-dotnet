using MaxMind.Db;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Exceptions;
using System;
using System.Diagnostics;
using System.Net;

namespace GeoIP2.Benchmark
{
    internal class Program
    {
        private static readonly int COUNT = 200000;

        private static void Main(string[] args)
        {
            using (var reader = new DatabaseReader("GeoLite2-City.mmdb", FileAccessMode.Memory))
            {
                Bench("Warm-up for Memory mode", reader);
                Bench("Memory mode", reader);
            }

            using (var reader = new DatabaseReader("GeoLite2-City.mmdb", FileAccessMode.MemoryMapped))
            {
                Bench("Warm-up for MMAP mode", reader);
                Bench("MMAP mode", reader);
            }
        }

        private static void Bench(string name, DatabaseReader reader)
        {
            Console.Write($"\n{name}: ");
            var rand = new Random(1);
            var s = Stopwatch.StartNew();
            for (var i = 0; i < COUNT; i++)
            {
                var ip = new IPAddress(rand.Next(int.MaxValue));
                try
                {
                    reader.City(ip);
                }
                catch (AddressNotFoundException) { }
            }
            s.Stop();
            Console.WriteLine("{0:N0} queries per second", COUNT / s.Elapsed.TotalSeconds);
        }
    }
}