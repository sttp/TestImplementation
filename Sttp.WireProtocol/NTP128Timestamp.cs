namespace Sttp.WireProtocol
{
    public class NTP128Timestamp
    {
        public long Seconds;    // Seconds since 1/1/1900, +/-292 billion years
        public ulong Fraction;  // 0.05 attosecond resolution (i.e., 0.5e-18 second)
        public TimestampFlags Flags;
    }
    // 17-bytes
}
