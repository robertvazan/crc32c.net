// Part of CRC-32C library: https://crc32c.machinezoo.com/
﻿using System;
using System.Security.Cryptography;

namespace Crc32C
{
    /// <summary>
    /// Implementation of CRC-32C (Castagnoli) with polynomial 0x1EDC6F41.
    /// It can detect errors more reliably than the older CRC-32-IEEE.
    /// This class will use CRC32 instruction on recent Intel processors if it is available.
    /// Otherwise it will transparently fall back to a very fast software implementation.
    /// If it impossible to use native implementation, calculation will fallback to fast .NET safe implementation.
    /// Besides standard HashAlgorithm methods,
    /// this class supports several convenient static methods returning the CRC as UInt32.
    /// </summary>
    public class Crc32CAlgorithm : HashAlgorithm
    {
        uint CurrentCrc;

        /// <summary>
        /// Creates new instance of Crc32CAlgorithm.
        /// </summary>
        public Crc32CAlgorithm()
        {
            HashSizeValue = 32;
        }

        /// <summary>
        /// Computes CRC-32C from multiple buffers.
        /// Call this method multiple times to chain multiple buffers.
        /// </summary>
        /// <param name="initial">
        /// Initial CRC value for the algorithm. It is zero for the first buffer.
        /// Subsequent buffers should have their initial value set to CRC value returned by previous call to this method.
        /// </param>
        /// <param name="input">Input buffer with data to be checksummed.</param>
        /// <param name="offset">Offset of the input data within the buffer.</param>
        /// <param name="length">Length of the input data in the buffer.</param>
        /// <returns>Accumulated CRC-32C of all buffers processed so far.</returns>
        public static uint Append(uint initial, byte[] input, int offset, int length)
        {
            if (input == null)
                throw new ArgumentNullException();
            if (offset < 0 || length < 0 || offset + length > input.Length)
                throw new ArgumentOutOfRangeException("Selected range is outside the bounds of the input array");
            return AppendInternal(initial, input, offset, length);
        }

        /// <summary>
        /// Computes CRC-32C from multiple buffers.
        /// Call this method multiple times to chain multiple buffers.
        /// </summary>
        /// <param name="initial">
        /// Initial CRC value for the algorithm. It is zero for the first buffer.
        /// Subsequent buffers should have their initial value set to CRC value returned by previous call to this method.
        /// </param>
        /// <param name="input">Input buffer containing data to be checksummed.</param>
        /// <returns>Accumulated CRC-32C of all buffers processed so far.</returns>
        public static uint Append(uint initial, byte[] input)
        {
            if (input == null)
                throw new ArgumentNullException();
            return AppendInternal(initial, input, 0, input.Length);
        }

        /// <summary>
        /// Computes CRC-32C from input buffer.
        /// </summary>
        /// <param name="input">Input buffer with data to be checksummed.</param>
        /// <param name="offset">Offset of the input data within the buffer.</param>
        /// <param name="length">Length of the input data in the buffer.</param>
        /// <returns>CRC-32C of the data in the buffer.</returns>
        public static uint Compute(byte[] input, int offset, int length)
        {
            return Append(0, input, offset, length);
        }

        /// <summary>
        /// Computes CRC-32C from input buffer.
        /// </summary>
        /// <param name="input">Input buffer containing data to be checksummed.</param>
        /// <returns>CRC-32C of the buffer.</returns>
        public static uint Compute(byte[] input)
        {
            return Append(0, input);
        }

        /// <summary>
        /// Resets internal state of the algorithm. Used internally.
        /// </summary>
        public override void Initialize()
        {
            CurrentCrc = 0;
        }

        protected override void HashCore(byte[] input, int offset, int length)
        {
            CurrentCrc = AppendInternal(CurrentCrc, input, offset, length);
        }

        protected override byte[] HashFinal()
        {
            return BitConverter.GetBytes(CurrentCrc);
        }

        private static uint AppendInternal(uint initial, byte[] input, int offset, int length)
        {
            if (length > 0)
            {
                  return BaseProxy.Instance.Append(initial, input, offset, length);
            }
            else
                return initial;
        }
    }
}
