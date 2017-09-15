using System;

namespace Sttp.WireProtocol
{
    [Flags]
    public enum StringEncodingFlags
    {
        ASCII = 0,
        ANSI = 1 << 0,
        UTF8 = 1 << 1,
        Unicode = 1 << 2
    }
    // sizeof(uint8), 1-byte
}