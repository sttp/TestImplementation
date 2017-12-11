namespace Sttp
{
    public enum SttpValueTypeCode : byte
    {
        Null = 0,               // 0-bytes
        Int64 = 1,              // 8-bytes
        UInt64 = 2,             // 8-bytes
        Single = 3,             // 4-bytes
        Double = 4,            // 8-bytes
        Decimal = 5,           // 16-bytes
        SttpTime = 6,          // Local/Universal Time with Leap Seconds
        Boolean = 7,           // 1-byte
        Char = 8,              // 2-bytes
        Guid = 9,              // 16-bytes
        String = 10,            // 1MB Limit, however, if the value is too large, additional overhead will occur to send the value out of band.
        SttpBuffer = 11,        // 1MB Limit, however, if the value is too large, additional overhead will occur to send the value out of band.
        SttpValueSet = 12,      // An array of SttpValue. Up to 255 elements.
        SttpNamedSet = 13,      // An array of [string,SttpValue]. Up to 255 elements. Like a connection string.
        SttpMarkup = 14,        // Yet another markup language
        BulkTransportGuid = 15, // A special type of GUID that indicates it is transmitted out of band.
    }

    // sizeof(uint8), 1-byte
}
