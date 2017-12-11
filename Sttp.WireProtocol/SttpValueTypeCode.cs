namespace Sttp
{
    public enum SttpValueTypeCode : byte
    {
        Null = 0,               // 0-bytes
        Int32 = 3,              // 4-bytes
        Int64 = 4,              // 8-bytes
        UInt32 = 7,             // 4-bytes
        UInt64 = 8,             // 8-bytes
        Single = 9,             // 4-bytes
        Double = 10,            // 8-bytes
        Decimal = 11,           // 16-bytes
        SttpTime = 14,          // Local/Universal Time with Leap Seconds
        Boolean = 17,           // 1-byte
        Char = 18,              // 2-bytes
        Guid = 19,              // 16-bytes
        String = 20,            // 1MB Limit, however, if the value is too large, additional overhead will occur to send the value out of band.
        SttpBuffer = 21,        // 1MB Limit, however, if the value is too large, additional overhead will occur to send the value out of band.
        SttpValueSet = 22,      // An array of SttpValue. Up to 255 elements.
        SttpNamedSet = 23,      // An array of [string,SttpValue]. Up to 255 elements. Like a connection string.
        SttpMarkup = 24,        // Yet another markup language
        BulkTransportGuid = 25, // A special type of GUID that indicates it is transmitted out of band.
    }

    // sizeof(uint8), 1-byte
}
