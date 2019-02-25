namespace CTP
{
    public enum CtpTypeCode : byte
    {
        Null = 0,               // 0-byte null value
        Int8 = 1,               // 1-byte signed integer
        Int16 = 2,              // 2-byte signed integer
        Int32 = 3,              // 4-byte signed integer
        Int64 = 4,              // 8-byte signed integer
        Numeric = 5,            // A 96-bit scaled integer value. Formatted as Decimal.
                                // Note: For all integer types, they SHOULD be represented as their smallest integer type.
                                //       This includes Numeric when Scale = 0. Otherwise, values may be unnecessarily padded.
        Single = 6,             // 4-byte IEEE Float
        Double = 7,             // 8-byte IEEE Float
        CtpTime = 8,            // 8-bytes Sends time in a number of different formats, adds flags for leap seconds.
        Boolean = 9,            // 1-bit True or False
        Guid = 10,              // 16-bytes
        String = 11,            // A UTF-8 encoded string value. The size limit is not expressly stated, but is bound by the maximum fragmented packet size.
        CtpBuffer = 12,         // A raw byte block. The size limit is not expressly stated, but is bound by the maximum fragmented packet size.
        CtpCommand = 13,        // A special markup language for fulfill the complex object mapping need of some data types. This also allows commands to be less structured.
                                // Can easily be converted to/from JSON or XML or YAML.
    }
}
