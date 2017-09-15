namespace Sttp.WireProtocol
{
    public enum ValueType
    {
        Null = 0,     // 0-bytes
        SByte = 1,    // 1-byte
        Int16 = 2,    // 2-bytes
        Int32 = 3,    // 4-bytes
        Int64 = 4,    // 8-bytes
        Byte = 5,     // 1-byte
        UInt16 = 6,   // 2-bytes
        UInt32 = 7,   // 4-bytes
        UInt64 = 8,   // 8-bytes
        Decimal = 9,  // 16-bytes
        Double = 10,  // 8-bytes
        Single = 11,  // 4-bytes
        Ticks = 12,   // 8-bytes
        Bool = 13,    // 1-byte
        Guid = 14,    // 16-bytes
        String = 15,  // 16-bytes, max
        Buffer = 16   // 16-bytes, max
    }
    // sizeof(uint8), 1-byte
}
