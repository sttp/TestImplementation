namespace Sttp
{
    public enum SttpValueTypeCode : byte
    {
        Null = 0,               // 0-bytes
        Int64 = 1,              // 8-bytes
        UInt64 = 2,             // 8-bytes
        Single = 3,             // 4-bytes
        Double = 4,             // 8-bytes
        Decimal = 5,            // 16-bytes
        SttpTime = 6,           // Local/Universal Time with Leap Seconds
        Boolean = 7,            // 1-byte
        Guid = 9,               // 16-bytes
        String = 10,            // 1MB Limit, however, if the value is too large, additional overhead will occur to send the value out of band.
        SttpBuffer = 11,        // 1MB Limit, however, if the value is too large, additional overhead will occur to send the value out of band.
        SttpMarkup = 12,        // Yet another markup language
        SttpBulkTransport = 13, // A special type of value that indicates it is transmitted out of band. These are reserved for very large objects.
    }

    // sizeof(uint8), 1-byte
}
