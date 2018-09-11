// Part of CRC-32C library: https://crc32c.machinezoo.com/
﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crc32C.Tests
{
    [TestFixture]
    public class Crc32CAlgorithmTest
    {
        [TestCase("Hello", 3, 2, 1)]
        [TestCase("Nazdar", 0, 0, 6)]
        [TestCase("Ahoj", 10, 0, 0)]
        [TestCase("", 0, 0, 0)]
        [TestCase("", 7, 4, 0)]
        [TestCase("", 0, 5, 0)]
        [TestCase("", 8, 0, 0)]
        public void MethodConsistency(string text, int offset, int tail, int split)
        {
            var checksums = new List<uint>();
            var array = Encoding.ASCII.GetBytes(text);
            var padded = Enumerable.Repeat<byte>(17, offset).Concat(array).Concat(Enumerable.Repeat<byte>(93, tail)).ToArray();
            var half1 = array.Take(split).ToArray();
            var half2 = array.Skip(split).ToArray();
            checksums.Add(Crc32CAlgorithm.Compute(array));
            checksums.Add(Crc32CAlgorithm.Compute(padded, offset, array.Length));
            checksums.Add(Crc32CAlgorithm.Append(0, array));
            checksums.Add(Crc32CAlgorithm.Append(0, padded, offset, array.Length));
            checksums.Add(Crc32CAlgorithm.Append(Crc32CAlgorithm.Append(0, half1), half2));
            checksums.Add(Crc32CAlgorithm.Append(Crc32CAlgorithm.Append(0, padded, offset, split), padded, offset + split, array.Length - split));
            using (var hash = new Crc32CAlgorithm())
                checksums.Add(BitConverter.ToUInt32(hash.ComputeHash(array), 0));
            using (var hash = new Crc32CAlgorithm())
                checksums.Add(BitConverter.ToUInt32(hash.ComputeHash(padded, offset, array.Length), 0));
            using (var stream = new MemoryStream(array))
            using (var hash = new Crc32CAlgorithm())
                checksums.Add(BitConverter.ToUInt32(hash.ComputeHash(stream), 0));
            if (text.Length == 0)
                Assert.AreEqual(0, checksums[0]);
            foreach (var checksum in checksums)
                Assert.AreEqual(checksums[0], checksum);
        }

        [Test]
        public void MethodConsistencyLong()
        {
            var random = new Random();
            var text = new String(Enumerable.Range(0, 1000000).Select(i => (char)((int)'A' + random.Next(26))).ToArray());
            MethodConsistency(text, 123, 456, 123456);
        }

        [Test]
        public void Exceptions()
        {
            Assert.Throws<ArgumentNullException>(() => Crc32CAlgorithm.Compute(null));
            Assert.Throws<ArgumentNullException>(() => Crc32CAlgorithm.Compute(null, 0, 0));
            Assert.Throws<ArgumentNullException>(() => Crc32CAlgorithm.Append(0, null));
            Assert.Throws<ArgumentNullException>(() => Crc32CAlgorithm.Append(0, null, 0, 0));
            var buffer = new byte[10];
            Assert.Throws<ArgumentOutOfRangeException>(() => Crc32CAlgorithm.Compute(buffer, -1, 5));
            Assert.Throws<ArgumentOutOfRangeException>(() => Crc32CAlgorithm.Compute(buffer, 0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => Crc32CAlgorithm.Compute(buffer, 5, 6));
            Assert.Throws<ArgumentOutOfRangeException>(() => Crc32CAlgorithm.Append(0, buffer, -1, 5));
            Assert.Throws<ArgumentOutOfRangeException>(() => Crc32CAlgorithm.Append(0, buffer, 0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => Crc32CAlgorithm.Append(0, buffer, 5, 6));
        }
    }
}
