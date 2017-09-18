namespace Sttp.WireProtocol
{
    /// <summary>
    /// SEC: Please provide a use case for this class. If there is not a need, why are we complicating the problem.
    /// </summary>
    public class NTP128Timestamp : IEncode
    {
        public long Seconds;    // Seconds since 1/1/1900, +/-292 billion years
        public ulong Fraction;  // 0.05 attosecond resolution (i.e., 0.5e-18 second)
        public TimestampFlags Flags;

        public byte[] Encode()
        {
            return null;
        }

        public static NTP128Timestamp Decode(byte[] buffer, int startIndex, int length)
        {
            return null;
        }
    }
    // 17-bytes
}
