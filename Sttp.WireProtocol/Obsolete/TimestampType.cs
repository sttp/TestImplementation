namespace Sttp.WireProtocol
{
    // SEC: I think we should decide on a single timestamp type that we will support instead of complicate the protocol.
    // JRC: I am not thinking this adds much complexity - when encoding these timestamps you either have one or two 64-bit values.
    //      There can be a "single" timestamp class at the API level for the STTP implementations that hides the details. With the
    //      exception of Ticks, these are industry standard timestamps that are readily understood and the proposed resolutions
    //      mean that you will never have to revisit time again.
    // Publisher determines timestamp type for data point - should be stored in metadata
    public enum TimestampType
    {
        NoTime = 0x0, // No timestamp included
        Ticks = 0x1,  // Using TicksTimestamp - 9-byte 100-nanosecond resolution spanning 32,768 years
        Unix64 = 0x2, // Using Unix64Timestamp - 9-byte second resolution spanning 584 billion years
        NTP128 = 0x3  // Using NTP128Timestamp - 17-byte attosecond resolution spanning 584 billion years
    }
    // 2-bits
}
