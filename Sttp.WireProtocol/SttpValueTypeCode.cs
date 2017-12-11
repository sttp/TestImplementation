namespace Sttp
{
    public enum SttpValueTypeCode : byte
    {
        Null = 0,               // 0-bytes
        Int64 = 1,              // 8-bytes Signed Value (An unsigned value exceeding Int64.MaxValue must be converted into a Decimal type)
        Single = 2,             // 4-bytes
        Double = 3,             // 8-bytes
        Decimal = 4,            // 16-bytes
        SttpTime = 5,           // 12-bytes Sends time in a number of different formats, adds flags for leap seconds.
        Boolean = 6,            // 1-bit True or False
        Guid = 7,               // 16-bytes
        String = 8,             // A UTF-8 encoded string value. There is no practical size limit, however large strings might benefit by being sent over SttpBulkTransport.
        SttpBuffer = 9,         // A raw byte block. There is no practical size limit, however large blocks might benefit by being sent over SttpBulkTransport.
        SttpMarkup = 10,        // A special markup language for fulfill the complex object mapping need of some data types. This also allows commands to be less structured.
                                // Can easily be converted to/from JSON or XML or YAML.
        SttpBulkTransport = 11, // A special "pointer" type that must be requested out of band. These are ideal for very large buffers that should not be sent inline with other types.
    }

    // sizeof(uint8), 1-byte
}
