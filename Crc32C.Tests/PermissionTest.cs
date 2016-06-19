using System;
using System.Security;
using System.Security.Permissions;

using NUnit.Framework;

namespace Crc32C.Tests
{
	[TestFixture]
	public class PermissionTest
	{
		[Test]
		public void CanRunInLimitedEnvironment()
		{
			var setup = new AppDomainSetup
			{
				ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
				ApplicationName = "sandbox",
			};

			var permissions = new PermissionSet(PermissionState.None);
			// assembly load
			permissions.AddPermission(new FileIOPermission(PermissionState.Unrestricted));
			// assembly execute
			permissions.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));

			var test = AppDomain.CreateDomain("sandbox", null, setup, permissions);

			var instance = (Executor)test.CreateInstanceFromAndUnwrap(this.GetType().Assembly.Location, typeof(Executor).FullName);
			// lets ensure, without permissions we can calculate something
			Assert.That(instance.Compute(new byte[1]), Is.EqualTo(1383945041));
		}

		public class Executor : MarshalByRefObject
		{
			public uint Compute(byte[] input)
			{
				return Crc32CAlgorithm.Compute(input);
			}
		}
	}
}
