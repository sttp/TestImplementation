namespace Sttp
{
    public enum SttpValueTypeCode : byte
    {
        // SEC: I'm having a hard time understanding why a point that can only contain null values would be useful.
        // JRC: Time and state are valid quantities even without a measured value.
        Null = 0,             // 0-bytes
        SByte = 1,            // 1-byte
        Int16 = 2,            // 2-bytes
        Int32 = 3,            // 4-bytes
        Int64 = 4,            // 8-bytes
        Byte = 5,             // 1-byte
        UInt16 = 6,           // 2-bytes
        UInt32 = 7,           // 4-bytes
        UInt64 = 8,           // 8-bytes
        Single = 9,           // 4-bytes
        Double = 10,          // 8-bytes
        Decimal = 11,         // 16-bytes
        SttpTime = 12,        // 8-bytes
        SttpTimeOffset = 13,  // 12-bytes
        TimeSpan = 14,        // 8 bytes
        Bool = 15,            // 1-byte
        Char = 16,            // 2-bytes
        Guid = 17,            // 16-bytes
        String = 18,          // 1MB Limit, however, if the value is too large, additional overhead will occur to send the value out of band.
        Buffer = 19,          // 1MB Limit, however, if the value is too large, additional overhead will occur to send the value out of band.
        ValueSet = 20,        // An array of SttpValue. Up to 255 elements.
        NamedSet = 21,        // An array of [string,SttpValue]. Up to 255 elements. Like a connection string.
    }

    // sizeof(uint8), 1-byte
}
