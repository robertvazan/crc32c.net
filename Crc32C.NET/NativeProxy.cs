// Part of CRC-32C library: https://crc32c.machinezoo.com/
﻿using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Crc32C
{
    abstract class NativeProxy : BaseProxy
    {
        protected NativeProxy(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var folder = Path.Combine(Path.GetTempPath(), "Crc32C.NET-" + assembly.GetName().Version.ToString());
            Directory.CreateDirectory(folder);
            var path = Path.Combine(folder, name);
            byte[] contents;
            using (var input = assembly.GetManifestResourceStream("Crc32C." + name))
            using (var buffer = new MemoryStream())
            {
                byte[] block = new byte[8192];
                int copied;
                while ((copied = input.Read(block, 0, block.Length)) != 0)
                    buffer.Write(block, 0, copied);
                buffer.Close();
                contents = buffer.ToArray();
            }
            if (!File.Exists(path) || !Utils.BuffersEqual(File.ReadAllBytes(path), contents))
            {
                using (var output = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.None))
                    output.Write(contents, 0, contents.Length);
            }
            var h = LoadLibrary(path);
            if (h == IntPtr.Zero)
                throw new ApplicationException("Cannot load " + name);
        }

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern IntPtr LoadLibrary(string lpFileName);
    }
}
