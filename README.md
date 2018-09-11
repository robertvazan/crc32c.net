# CRC-32C (Castagnoli) for .NET #

This is a hardware-accelerated implementation of CRC-32C (Castagnoli) for .NET.
Intel's CRC32 instruction is used if available. Otherwise this library uses fast software fallback.
Actual CRC-32C algorithm is implemented in C++. .NET wrapper transparently routes calls to native code.

* [Website](https://crc32c.machinezoo.com/)
* [Tutorial for .NET](https://crc32c.machinezoo.com/#net)
* [Source code (main repository)](https://bitbucket.org/robertvazan/crc32c.net/src/default/)
* [BSD license](https://opensource.org/licenses/BSD-3-Clause)

***

```csharp
uint crc = Crc32CAlgorithm.Compute(array);
```

