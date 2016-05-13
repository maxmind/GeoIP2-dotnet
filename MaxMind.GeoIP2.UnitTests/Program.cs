using System;
using System.Reflection;
using NUnit.Common;
using NUnitLite;

#if NETCOREAPP1_0
using System.IO;
#endif

namespace MaxMind.GeoIP2.UnitTests
{
    public static class Program
    {
        public static string CurrentDirectory =>
#if !NETCOREAPP1_0
            Environment.CurrentDirectory;
#else
            Directory.GetCurrentDirectory();
#endif

        public static int Main(string[] args)
        {
            return new AutoRun(typeof(Program).GetTypeInfo().Assembly)
                .Execute(args, new ExtendedTextWrapper(Console.Out), Console.In);
        }
    }
}
