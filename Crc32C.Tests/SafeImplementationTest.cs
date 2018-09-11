// Part of CRC-32C library: https://crc32c.machinezoo.com/
﻿using System;
using System.Text;

using NUnit.Framework;

namespace Crc32C.Tests
{
	[TestFixture]
	public class SafeImplementationTest
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

		[TestCase("Hello", 3)]
		[TestCase("Nazdar", 0)]
		[TestCase("Ahoj", 1)]
		[TestCase("Very long text.Very long text.Very long text.Very long text.Very long text.Very long text.Very long text", 0)]
		[TestCase("Very long text.Very long text.Very long text.Very long text.Very long text.Very long text.Very long text", 3)]
		public void ResultConsistency(string text, int offset)
		{
			var bytes = Encoding.ASCII.GetBytes(text);
			var crc1 = Crc32CAlgorithm.Append(123456789, bytes, offset, bytes.Length - offset);

			CoreSwitchHelper.SetCoreAlgType("Crc32C.SafeProxy");
			var crc2 = Crc32CAlgorithm.Append(123456789, bytes, offset, bytes.Length - offset);
			Assert.That(crc2, Is.EqualTo(crc1));
		}

		[Test]
		public void ResultConsistencyLong()
		{
			var bytes = new byte[30000];
			new Random().NextBytes(bytes);
			var crc1 = Crc32CAlgorithm.Append(123456789, bytes, 0, bytes.Length);
			CoreSwitchHelper.SetCoreAlgType("Crc32C.SafeProxy");
			var crc2 = Crc32CAlgorithm.Append(123456789, bytes, 0, bytes.Length);
			Assert.That(crc2, Is.EqualTo(crc1));
		}
	}
}
