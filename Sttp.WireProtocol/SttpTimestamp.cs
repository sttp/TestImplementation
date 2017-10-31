using System;

namespace Sttp.WireProtocol.SendDataPoints
{
    /// <summary>
    /// The wireline type for transmitting time.
    /// </summary>
    public class SttpTimestamp
    {
        private const ulong LeapsecondFlag = 0x1000000000000000;

        public readonly long Seconds;    // Seconds since 1/1/0001 +/-292 billion years
                                         // Stored normally.
        public readonly ulong Fraction;  // 10 bits for milliseconds, 10 bits for microseconds, 10 bits for nanoseconds, 10 bits for picoseconds,
                                         // 10 bits for femtoseconds, 10 bits for attoseconds, 1 bit for leapsecond, 3 bits reserved.
        public int Milliseconds => (int)((Fraction >> 50) & 1023);
        public int Microseconds => (int)((Fraction >> 40) & 1023);
        public int Nanoseconds => (int)((Fraction >> 30) & 1023);
        public int Picoseconds => (int)((Fraction >> 20) & 1023);
        public int Femtsoeconds => (int)((Fraction >> 10) & 1023);
        public int Attoseconds => (int)(Fraction & 1023);
        public bool LeapsecondInProgress => (Fraction & LeapsecondFlag) > 0;

        public SttpTimestamp(DateTime time, bool leapSecondInProgress = false)
        {
            Seconds = time.Ticks / TimeSpan.TicksPerSecond;
            ulong milliseconds = (uint)(time.Ticks / TimeSpan.TicksPerMillisecond % 1000);
            ulong microseconds = (uint)(time.Ticks / 10 % 1000);
            ulong nanoseconds = (uint)(time.Ticks % 10 * 1000);
            Fraction = (milliseconds << 50) | (microseconds << 40) | (nanoseconds << 30);

            if (leapSecondInProgress)
                Fraction |= LeapsecondFlag;
        }
        public SttpTimestamp(long seconds, ulong fraction)
        {
            Seconds = seconds;
            Fraction = fraction;
        }        
    }
    // 16-bytes
}
