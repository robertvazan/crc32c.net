using System;
using System.Diagnostics;

using NUnit.Framework;

namespace Crc32C.Tests
{
    [TestFixture]
    public class PerformanceTest
    {
	    private object _defaultCoreAlg;

		[SetUp]
		public void SetUp()
		{
			_defaultCoreAlg = CoreSwitchHelper.GetCoreAlg();
		}

		[TearDown]
		public void TearDown()
		{
			CoreSwitchHelper.SetCoreAlg(_defaultCoreAlg);
		}

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

		[Test]
		public void ThroughputSafe()
		{
			CoreSwitchHelper.SetCoreAlgType("Crc32C.SafeProxy");
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
			Console.WriteLine("Throughput: {0:0.0} MB/s", total / stopwatch.Elapsed.TotalSeconds / 1024 / 1024);
		}
    }
}
