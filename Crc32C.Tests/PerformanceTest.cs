using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crc32C.Tests
{
    [TestFixture]
    public class PerformanceTest
    {
        [Test]
        public void Throughput()
        {
            var data = new byte[65536];
            var random = new Random();
            random.NextBytes(data);
            long total = 0;
            var stopwatch = new Stopwatch();
            uint crc = 0;
            stopwatch.Start();
            while (stopwatch.Elapsed < TimeSpan.FromSeconds(3))
            {
                var length = random.Next(data.Length + 1);
                var offset = random.Next(data.Length - length);
                crc = Crc32CAlgorithm.Append(crc, data, offset, length);
                total += length;
            }
            stopwatch.Stop();
            Console.WriteLine("Throughput: {0:0.0} GB/s", total / stopwatch.Elapsed.TotalSeconds / 1024 / 1024 / 1024);
        }
    }
}
