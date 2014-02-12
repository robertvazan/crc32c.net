using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Crc32C
{
    class Native32 : NativeProxy
    {
        public Native32() : base("crc32c32.dll") { }

        public unsafe override uint Append(uint crc, byte* input, int length)
        {
            return crc32c_append(crc, input, checked((uint)length));
        }

        [DllImport("crc32c32.dll")]
        static unsafe extern uint crc32c_append(uint crc, byte* input, uint length);
    }
}
