This is hardware-accelerated implementation of CRC-32C (Castagnoli) for .NET.
Intel’s CRC32 instruction is used if available. Otherwise this library uses super fast software fallback.
Actual CRC-32C algorithm is implemented in C++. .NET wrapper transparently routes calls to native code.

Website: [CRC-32C (Castagnoli) for C++ and .NET](http://crc32c.angeloflogic.com/)

NuGet: [Crc32C.NET](https://www.nuget.org/packages/Crc32C.NET/)

Download: [crc32c-hw-1.0.2.2.zip](http://crc32c.angeloflogic.com/download/crc32c-hw-1.0.2.2.zip)

License: [BSD license](http://crc32c.angeloflogic.com/license-net/)
