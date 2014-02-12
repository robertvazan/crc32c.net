using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Crc32C
{
    class Native64 : NativeProxy
    {
        public Native64() : base("crc32c64.dll") { }

        public unsafe override uint Append(uint crc, byte* input, int length)
        {
            return crc32c_append(crc, input, checked((ulong)length));
        }

        [DllImport("crc32c64.dll")]
        static unsafe extern uint crc32c_append(uint crc, byte* input, ulong length);
    }
}
