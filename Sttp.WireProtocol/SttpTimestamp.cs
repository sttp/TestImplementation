using System;
using System.Linq;

namespace Sttp.WireProtocol
{
    /// <summary>
    /// The wireline type for transmitting time.
    /// </summary>
    public class SttpTimestamp
    {
        public readonly long Seconds;    // Seconds since 1/1/0001 +/-292 billion years
                                         // Stored normally.
        public readonly long Fraction;  // 10 bits for milliseconds, 10 bits for microseconds, 10 bits for nanoseconds, 10 bits for picoseconds,
                                        // 10 bits for femto seconds, 10 bits for atto seconds, 2 bits reserved.
        public readonly TimestampFlags Flags;

        public int Milliseconds => (int)((Fraction >> 50) & 1023);
        public int Microseconds => (int)((Fraction >> 40) & 1023);
        public int Nanoseconds => (int)((Fraction >> 30) & 1023);
        public int Picoseconds => (int)((Fraction >> 20) & 1023);
        public int Femtsoeconds => (int)((Fraction >> 10) & 1023);
        public int Attoseconds => (int)(Fraction & 1023);

        public SttpTimestamp(DateTime time)
        {
            Seconds = time.Ticks / TimeSpan.TicksPerSecond;
            long milliseconds = (int)(time.Ticks / TimeSpan.TicksPerMillisecond % 1000);
            long microseconds = (int)(time.Ticks / 10 % 1000);
            long nanoseconds = (int)(time.Ticks % 10 * 1000);
            Fraction = (milliseconds << 50) | (microseconds << 40) | (nanoseconds << 30);
        }
        public SttpTimestamp(long seconds, long fraction, TimestampFlags flags)
        {
            Seconds = seconds;
            Fraction = fraction;
            Flags = flags;
        }

        public byte[] Encode()
        {
            return BigEndian.GetBytes(Seconds).Concat(BigEndian.GetBytes(Fraction)).Concat(new byte[] { (byte)Flags }).ToArray();
        }

        public static SttpTimestamp Decode(byte[] buffer, int startIndex, int length)
        {
            if (length != 17)
                throw new Exception();
            buffer.ValidateParameters(startIndex, length);
            return new SttpTimestamp(BigEndian.ToInt64(buffer, startIndex), BigEndian.ToInt64(buffer, startIndex + 8), (TimestampFlags)buffer[startIndex + 16]);
        }
    }
    // 17-bytes
}
