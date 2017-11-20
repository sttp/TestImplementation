namespace Sttp
{
    public enum SttpValueTypeCode : byte
    {
        Null = 0,               // 0-bytes
        SByte = 1,              // 1-byte
        Int16 = 2,              // 2-bytes
        Int32 = 3,              // 4-bytes
        Int64 = 4,              // 8-bytes
        Byte = 5,               // 1-byte
        UInt16 = 6,             // 2-bytes
        UInt32 = 7,             // 4-bytes
        UInt64 = 8,             // 8-bytes
        Single = 9,             // 4-bytes
        Double = 10,            // 8-bytes
        Decimal = 11,           // 16-bytes
        DateTime = 12,          // Local/Universal/Unspecified/Unambiguous Date Time
        DateTimeOffset = 13,    // Local/Universal/Unspecified/Unambiguous Date Time with an offset.
        SttpTime = 14,          // Local/Universal Time with Leap Seconds
        SttpTimeOffset = 15,    // Local/Universal Time with Leap Seconds and timezone offset.
        TimeSpan = 16,          // 8 bytes
        Bool = 17,              // 1-byte
        Char = 18,              // 2-bytes
        Guid = 19,              // 16-bytes
        String = 20,            // 1MB Limit, however, if the value is too large, additional overhead will occur to send the value out of band.
        Buffer = 21,            // 1MB Limit, however, if the value is too large, additional overhead will occur to send the value out of band.
        ValueSet = 22,          // An array of SttpValue. Up to 255 elements.
        NamedSet = 23,          // An array of [string,SttpValue]. Up to 255 elements. Like a connection string.
        ConnectionString = 24,  // An array of [string,Restriction,SttpValue]. Up to 255 elements. Like a connection string.
        BulkTransportGuid = 25, // A special type of GUID that indicates it is transmitted out of band.
    }

    // sizeof(uint8), 1-byte
}
