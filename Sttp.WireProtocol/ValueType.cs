namespace Sttp.WireProtocol
{
    public enum ValueType
    {
        Null = 0,     // 0-bytes //SEC: I'm having a hard time understanding why a point that can only contain null values would be useful. 
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
        Ticks = 12,   // 8-bytes //SEC: would this be for Timespan, or Date. Do we need separate ones. Should we standardize the time format?
        Bool = 13,    // 1-byte
        Guid = 14,    // 16-bytes
        String = 15,  // 16-bytes, max //SEC: 16 bytes is too small.
        Buffer = 16   // 16-bytes, max
    }
    // sizeof(uint8), 1-byte
}
