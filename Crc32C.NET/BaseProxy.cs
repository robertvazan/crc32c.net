// Part of CRC-32C library: https://crc32c.machinezoo.com/
﻿using System;
using System.Runtime.InteropServices;

namespace Crc32C
{
	internal abstract class BaseProxy
	{
		public static readonly BaseProxy Instance;

		static BaseProxy()
		{
			try
			{
				// fast check for permissons and environment
				new PInvokeAbilityCheck().Check();
				// permissions ok, using native implementation
				Instance = IntPtr.Size == 4 ? (BaseProxy)new Native32() : new Native64();
			}
			catch (Exception)
			{
				// some errors, switching to safe mode
				Instance = new SafeProxy();
			}
		}

		// special class, to hide p/invoke binding from BaseProxy static constructor
		private class PInvokeAbilityCheck
		{
			public void Check()
			{
				GetLastError();
			}

			[DllImport("kernel32")]
			private static extern uint GetLastError();
		}

		public abstract uint Append(uint crc, byte[] input, int offset, int length);
	}
}
