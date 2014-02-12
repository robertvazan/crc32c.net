using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Crc32C
{
    public class Crc32CAlgorithm : HashAlgorithm
    {
        uint CurrentCrc;

        public static uint Append(uint initial, byte[] input, int offset, int length)
        {
            if (input == null)
                throw new ArgumentNullException();
            if (offset < 0 || length < 0 || offset + length > input.Length)
                throw new ArgumentOutOfRangeException("Selected range is outside the bounds of the input array");
            return AppendInternal(initial, input, offset, length);
        }

        public static uint Append(uint initial, byte[] input)
        {
            if (input == null)
                throw new ArgumentNullException();
            return AppendInternal(initial, input, 0, input.Length);
        }

        public static uint Compute(byte[] input, int offset, int length)
        {
            return Append(0, input, offset, length);
        }

        public static uint Compute(byte[] input)
        {
            return Append(0, input);
        }

        public override void Initialize()
        {
            CurrentCrc = 0;
        }

        protected override unsafe void HashCore(byte[] input, int offset, int length)
        {
            CurrentCrc = AppendInternal(CurrentCrc, input, offset, length);
        }

        protected override byte[] HashFinal()
        {
            return BitConverter.GetBytes(CurrentCrc);
        }

        public static unsafe uint AppendInternal(uint initial, byte[] input, int offset, int length)
        {
            if (length > 0)
            {
                fixed (byte* ptr = &input[offset])
                    return NativeProxy.Instance.Append(initial, ptr, length);
            }
            else
                return initial;
        }
    }
}
