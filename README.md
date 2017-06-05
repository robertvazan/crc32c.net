This is hardware-accelerated implementation of CRC-32C (Castagnoli) for .NET.
Intel's CRC32 instruction is used if available. Otherwise this library uses super fast software fallback.
Actual CRC-32C algorithm is implemented in C++. .NET wrapper transparently routes calls to native code.

Website: [CRC-32C (Castagnoli) for C++ and .NET](https://crc32c.machinezoo.com/)
