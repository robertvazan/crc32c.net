# CRC-32C (Castagnoli) for .NET #

This is a hardware-accelerated implementation of CRC-32C (Castagnoli) for .NET.
Intel's CRC32 instruction is used if available. Otherwise this library uses fast software fallback.
Actual CRC-32C algorithm is implemented in C++. .NET wrapper transparently routes calls to native code.

* Documentation: [Home](https://crc32c.machinezoo.com/), [Tutorial for .NET](https://crc32c.machinezoo.com/#net)
* Download: see [Tutorial for .NET](https://crc32c.machinezoo.com/#net)
* Sources: [GitHub](https://github.com/robertvazan/crc32c.net), [Bitbucket](https://bitbucket.org/robertvazan/crc32c.net)
* Issues: [GitHub](https://github.com/robertvazan/crc32c.net/issues), [Bitbucket](https://bitbucket.org/robertvazan/crc32c.net/issues)
* License: [BSD license](https://opensource.org/licenses/BSD-3-Clause)

***

```csharp
uint crc = Crc32CAlgorithm.Compute(array);
```

