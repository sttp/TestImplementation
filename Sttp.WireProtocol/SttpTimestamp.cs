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
        public SttpTimestamp(long seconds, long fraction)
        {
            Seconds = seconds;
            Fraction = fraction;
        }

        
    }
    // 17-bytes
}
