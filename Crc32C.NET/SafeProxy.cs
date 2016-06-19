/* This is .NET safe implementation of Crc32C algorithm.
 * This implementation was found fastest from some variants, based on Robert Vazan native implementations
 * Also, it is good for x64 and for x86, so, it seems, there is no sense to do 2 different realizations.
 * Reference speed: Hardware: 20GB/s, Software Native: 2GB/s, this: 850MB/s
 * 
 * Max Vysokikh, 2016
 */

namespace Crc32C
{
	internal class SafeProxy : BaseProxy
	{
		private const uint Poly = 0x82f63b78;

		private static readonly uint[] _table = new uint[16 * 256];

		static SafeProxy()
		{
			for (uint i = 0; i < 256; i++)
			{
				uint res = i;
				for (int t = 0; t < 16; t++)
				{
					for (int k = 0; k < 8; k++) res = (res & 1) == 1 ? Poly ^ (res >> 1) : (res >> 1);
					_table[(t * 256) + i] = res;
				}
			}
		}

		public override uint Append(uint crc, byte[] input, int offset, int length)
		{
			uint crcLocal = uint.MaxValue ^ crc;

			while (length >= 16)
			{
				crcLocal = _table[(15 * 256) + ((crcLocal ^ input[offset]) & 0xff)]
					^ _table[(14 * 256) + (((crcLocal >> 8) ^ input[offset + 1]) & 0xff)]
					^ _table[(13 * 256) + (((crcLocal >> 16) ^ input[offset + 2]) & 0xff)]
					^ _table[(12 * 256) + (((crcLocal >> 24) ^ input[offset + 3]) & 0xff)]
					^ _table[(11 * 256) + input[offset + 4]]
					^ _table[(10 * 256) + input[offset + 5]]
					^ _table[(9 * 256) + input[offset + 6]]
					^ _table[(8 * 256) + input[offset + 7]]
					^ _table[(7 * 256) + input[offset + 8]]
					^ _table[(6 * 256) + input[offset + 9]]
					^ _table[(5 * 256) + input[offset + 10]]
					^ _table[(4 * 256) + input[offset + 11]]
					^ _table[(3 * 256) + input[offset + 12]]
					^ _table[(2 * 256) + input[offset + 13]]
					^ _table[(1 * 256) + input[offset + 14]]
					^ _table[(0 * 256) + input[offset + 15]];
				offset += 16;
				length -= 16;
			}

			while (--length >= 0)
				crcLocal = _table[(crcLocal ^ input[offset++]) & 0xff] ^ crcLocal >> 8;
			return crcLocal ^ uint.MaxValue;
		}
	}
}
