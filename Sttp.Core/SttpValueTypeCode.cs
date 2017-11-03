namespace Sttp
{
    public enum SttpValueTypeCode : byte
    {
        // SEC: I'm having a hard time understanding why a point that can only contain null values would be useful.
        // JRC: Time and state are valid quantities even without a measured value.
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
        // SEC: Would this be for Timespan, or Date. Do we need separate ones. Should we standardize the time format?
        // JRC: Good questions - also, should we drop this altogether and let people pass a time "value" through one of the numerics to simplify available types?
        DateTime = 12,   // 8-bytes
        TimeSpan = 13,
        Char = 14,    // 1-byte
        Bool = 15,    // 1-byte
        Guid = 16,    // 16-bytes
        // SEC: 16 bytes is too small.
        // JRC: It is small - goal is something simple to encode and especially compress - however, I don't think the size matters much since publisher API can
        //      take a variable length string or byte array then fragment it into chunks, sequence them and have the subscriber API re-collate them.
        String = 17,  // 16-bytes, max
        Buffer = 18,   // 16-bytes, max
    }

    // sizeof(uint8), 1-byte
}
