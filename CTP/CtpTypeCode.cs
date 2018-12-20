namespace CTP
{
    public enum CtpTypeCode : byte
    {
        Null = 0,               // 0-bytes
        Int64 = 1,              // 8-bytes Signed Value (An unsigned value exceeding Int64.MaxValue will throw an overflow exception)
        Single = 2,             // 4-bytes
        Double = 3,             // 8-bytes
        CtpTime = 4,            // 8-bytes Sends time in a number of different formats, adds flags for leap seconds.
        Boolean = 5,            // 1-bit True or False
        Guid = 6,               // 16-bytes
        String = 7,             // A UTF-8 encoded string value. The size limit is not expressly stated, but is bound by the maximum fragmented packet size.
        CtpBuffer = 8,          // A raw byte block. The size limit is not expressly stated, but is bound by the maximum fragmented packet size.
        CtpCommand = 9,        // A special markup language for fulfill the complex object mapping need of some data types. This also allows commands to be less structured.
                                // Can easily be converted to/from JSON or XML or YAML.
    }

    // sizeof(uint8), 1-byte
}
