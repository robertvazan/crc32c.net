// Part of CRC-32C library: https://crc32c.machinezoo.com/
﻿using System;
using System.Reflection;

namespace Crc32C.Tests
{
	internal static class CoreSwitchHelper
	{
		private static FieldInfo GetFieldInfo()
		{
			var type = Assembly.GetAssembly(typeof(Crc32CAlgorithm)).GetType("Crc32C.BaseProxy");
			return type.GetField("Instance", BindingFlags.Static | BindingFlags.Public);
		}

		internal static object GetCoreAlg()
		{
			return GetFieldInfo().GetValue(null);
		}

		internal static void SetCoreAlg(object coreAlg)
		{
			GetFieldInfo().SetValue(null, coreAlg);
		}

		internal static void SetCoreAlgType(string typeName)
		{
			Type type = Assembly.GetAssembly(typeof(Crc32CAlgorithm)).GetType(typeName);
			if (type == null)
				throw new InvalidOperationException("Invalid type name");
			GetFieldInfo().SetValue(null, Activator.CreateInstance(type));
		}
	}
}
