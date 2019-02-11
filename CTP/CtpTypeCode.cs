namespace CTP
{
    public enum CtpTypeCode : byte
    {
        Null = 0,               // 0-bytes
        Int64 = 1,              // 8-bytes Signed Value (An unsigned value exceeding Int64.MaxValue will throw an overflow exception)
        Single = 2,             // 4-bytes
        Double = 3,             // 8-bytes
        Numeric = 4,            // A 128-bit scaled integer value. Formatted as Decimal.
        CtpTime = 5,            // 8-bytes Sends time in a number of different formats, adds flags for leap seconds.
        Boolean = 6,            // 1-bit True or False
        Guid = 7,               // 16-bytes
        String = 8,             // A UTF-8 encoded string value. The size limit is not expressly stated, but is bound by the maximum fragmented packet size.
        CtpBuffer = 9,          // A raw byte block. The size limit is not expressly stated, but is bound by the maximum fragmented packet size.
        CtpCommand = 10,        // A special markup language for fulfill the complex object mapping need of some data types. This also allows commands to be less structured.
                                // Can easily be converted to/from JSON or XML or YAML.
    }

    // sizeof(uint8), 1-byte
}
