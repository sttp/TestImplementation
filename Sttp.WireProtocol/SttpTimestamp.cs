using System;

namespace Sttp.WireProtocol.SendDataPoints
{
    /// <summary>
    /// The default timestamp field for STTP.
    /// </summary>
    public struct SttpTimestamp
    {
        private const long TicksMask = LeapsecondFlag - 1;
        private const long LeapsecondFlag = 1L << 62;

        public readonly long RawValue;           // Bits 0-62 Same as DateTime.Ticks  Bit 63 LeapSecondPending. Bit64 Sign for time BC.

        public SttpTimestamp(DateTime time, bool leapSecondInProgress = false)
        {
            RawValue = time.Ticks;
            if (leapSecondInProgress)
                RawValue |= LeapsecondFlag;
        }

        public SttpTimestamp(long rawValue)
        {
            RawValue = rawValue;
        }

        public DateTime Ticks => new DateTime(RawValue & TicksMask);

        public bool LeapsecondInProgress => (RawValue & LeapsecondFlag) > 0;


    } // 8-bytes
}
