using System;

namespace Sttp.WireProtocol.SendDataPoints
{
    

    ///// <summary>
    ///// The highest precision timestamp.
    ///// </summary>
    //public struct SttpLongTimestamp
    //{
    //    //Consider this 


    //    private const long LeapsecondFlag = 1L << 62;

    //    public readonly long Ticks;           // Bits 0-62 Same as DateTime.Ticks  Bit 63 LeapSecondPending. Bit64 Sign for time BC.
    //    public readonly uint ExtraPrecision;  // Normally 0, but for scientific data, 
    //                                          // Bit 31 - When 1, Bits 0-30 are used for extending +/- billions of years.
    //                                          //          When 0, Bits 24-30 brings the precision to Nanoseconds that are missing from Ticks. Ticks are 100ns. This provides 2 bits.
    //                                          //                  Bits 14-23 are picoseconds
    //                                          //                  Bits 4-23 are femtoseconds
    //                                          //                  Bits 0-3 are 100 attosecond.

    //    public bool LeapsecondInProgress => (Ticks & LeapsecondFlag) > 0;
    //    public bool IsHighPrecision => ExtraPrecision != 0;

    //    public SttpLongTimestamp(DateTime time, bool leapSecondInProgress = false)
    //    {
    //        Ticks = time.Ticks;
    //        ExtraPrecision = 0;
    //        if (leapSecondInProgress)
    //            Ticks |= LeapsecondFlag;
    //    }

    //    public SttpLongTimestamp(long rawTicks, uint extraPrecision)
    //    {
    //        Ticks = rawTicks;
    //        ExtraPrecision = extraPrecision;
    //    }

    //    public SttpTimestamp ToSttpTimestamp()
    //    {
    //        return new SttpTimestamp(Ticks);
    //    }
    //} // 12-bytes


    /// <summary>
    /// The highest precision timestamp.
    /// </summary>
    public struct SttpTicks
    {
        private const long LeapsecondFlag = 1L << 62;

        public readonly long Ticks;           // Bits 0-62 Same as DateTime.Ticks  Bit 63 LeapSecondPending. Bit64 Sign for time BC.

        public bool LeapsecondInProgress => (Ticks & LeapsecondFlag) > 0;

        public SttpTicks(DateTime time, bool leapSecondInProgress = false)
        {
            Ticks = time.Ticks;
            if (leapSecondInProgress)
                Ticks |= LeapsecondFlag;
        }
        public SttpTicks(long raw)
        {
            Ticks = raw;
        }

    } // 8-bytes
}
