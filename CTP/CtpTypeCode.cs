namespace CTP
{
    public enum CtpTypeCode : byte
    {
        Null = 0,               // 0-byte null value
        Integer = 1,            // 8-byte signed integer
        Single = 2,             // 4-byte IEEE Float
        Double = 3,             // 8-byte IEEE Float
        Numeric = 4,            // A 96-bit scaled integer value. Formatted as Decimal.
                                // Note: Decimal types that can be represented as Integers will be converted to an integer type.
        CtpTime = 5,            // 8-bytes Sends time in a number of different formats, adds flags for leap seconds.
        Boolean = 6,            // 1-bit True or False
        Guid = 7,               // 16-bytes
        String = 8,             // A UTF-8 encoded string value. The size limit is not expressly stated, but is bound by the maximum fragmented packet size.
        CtpBuffer = 9,          // A raw byte block. The size limit is not expressly stated, but is bound by the maximum fragmented packet size.
        CtpCommand = 10,        // A special markup language for fulfill the complex object mapping need of some data types. This also allows commands to be less structured.
                                // Can easily be converted to/from JSON or XML or YAML.
    }
}
