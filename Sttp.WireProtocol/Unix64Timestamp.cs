namespace Sttp.WireProtocol
{
    public class Unix64Timestamp
    {
        public long Value; // Seconds since 1/1/1970, +/-292 billion years
        public TimestampFlags Flags;
    }
    // 9-bytes
}
