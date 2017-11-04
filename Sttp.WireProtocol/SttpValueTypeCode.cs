namespace Sttp
{
    /// <summary>
    /// Represents the fundamental data types supported by STTP.
    /// Higher level types are derived from this base type, 
    /// and the lower 3 bits of the type code are reserved to match these fundamental types.
    /// 
    /// NOTE: When inheriting from something other than Buffer, the fundamental type of the struct must be a single field
    /// of the specified length. A field whose primitive type is (int,short,short) cannot use Int64 because it will serialize 
    /// differently depending on the endian of the platform. Neither will a union struct work. That's why there isn't a Int128,
    /// Decimal and Guid are structures of smaller primitive types. 
    /// 
    /// </summary>
    public enum SttpBaseValueTypeCode : byte
    {
        Null = 0,           //All values are nullable.
        One8BitValue = 1,   //SByte, Byte, Bool
        One16BitValue = 2,  //Int16, UInt16, Char
        One32BitValue = 3,  //Single, UInt32, Int32
        One64BitValue = 4,  //Double, UInt64, Int64, TimeSpan, DateTime, SttpTimestamp
        Buffer = 5,         //Numeric, Decimal, Guid, Custom User Defined Types
    }

    public enum SttpValueTypeCode : byte
    {
        // SEC: I'm having a hard time understanding why a point that can only contain null values would be useful.
        // JRC: Time and state are valid quantities even without a measured value.
        Null = 0 * 8 + 0,            // 0-bytes
        SByte = 0 * 8 + 1,           // 1-byte
        Byte = 1 * 8 + 1,            // 1-byte
        Bool = 2 * 8 + 1,            // 1-byte
        Int16 = 0 * 8 + 2,           // 2-bytes
        UInt16 = 1 * 8 + 2,          // 2-bytes
        Char = 2 * 8 + 2,            // 2-bytes
        Int32 = 1 * 8 + 3,           // 4-bytes
        UInt32 = 2 * 8 + 3,          // 4-bytes
        Single = 3 * 8 + 3,          // 4-bytes
        Int64 = 0 * 8 + 4,           // 8-bytes
        UInt64 = 1 * 8 + 4,          // 8-bytes
        Double = 2 * 8 + 4,          // 8-bytes
        SttpTime = 3 * 8 + 4,        // 8-bytes
        TimeSpan = 4 * 8 + 4,        // 8 bytes
        Buffer = 0 * 8 + 5,          // 16-bytes, max
        SttpTimeOffset = 1 * 8 + 5,  // 12-bytes
        Decimal = 2 * 8 + 5,         // 16-bytes
        Guid = 3 * 8 + 5,            // 16-bytes
        String = 4 * 8 + 5,          // 16-bytes, max
        Set = 5 * 8 + 5,             // An array of SttpValueTypeCode.
    }

    // sizeof(uint8), 1-byte
}
