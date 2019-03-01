namespace CTP
{
    public enum CtpTypeCode : byte
    {
        Null = 0,        // 0-byte null value
        Integer = 1,     // 8-byte signed integer
        Single = 2,      // 4-byte IEEE Float
        Double = 3,      // 8-byte IEEE Float
        Numeric = 4,     // A 96-bit scaled integer value. Formatted as Decimal.
        CtpTime = 5,     // 8-byte 100ns time with support for leap seconds.
        Boolean = 6,     // True or False
        Guid = 7,        // 16-bytes
        String = 8,      // A UTF-8 encoded string value.
        CtpBuffer = 9,   // A raw byte block. 
        CtpCommand = 10, // A special complex data type that includes schema and data, similar to JSON.
    }
}
