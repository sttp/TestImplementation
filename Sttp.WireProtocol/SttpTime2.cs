using System;

namespace Sttp
{
    [Flags]
    public enum SttpTimeFlags
    {
        /// <summary>
        /// When local time, this flag will indicate that this is the repeated hour of the local time.
        /// </summary>
        IsRepeatedHour = 1 << 0,
        /// <summary>
        /// Regardless of the Local/Universal Time, this flag indicates that a repeated second is occurring.
        /// </summary>
        IsLeapSecondOccurring = 1 << 1,
        /// <summary>
        /// Gets if the time specified is in local time or universal time.
        /// </summary>
        IsUniversalTime = 1 << 2,
        /// <summary>
        /// Gets if the time specified is a date only without a time.
        /// </summary>
        IsDateOnly = 1 << 3,
        /// <summary>
        /// Gets if the value specified is a time only without a date.
        /// </summary>
        IsTimeOnly = 1 << 4,

        /// <summary>
        /// Indicates that the kind of the datetime is not specified.
        /// </summary>
        IsDateTimeUnspecified,

        /// <summary>
        /// Gets if the tick value held is a timespan.
        /// </summary>
        IsTimeSpan,
    }
    /// <summary>
    /// The default timestamp field for STTP.
    /// </summary>
    public struct SttpTime
    {
        public readonly uint UpperTicks;           // Bits 0-62 Same as DateTime.Ticks Bit 63 LeapSecondPending. Bit64 is reserved for RepeatedHour if time is specified in local time.
        public readonly uint LowerTicks;           // Bits 0-62 Same as DateTime.Ticks Bit 63 LeapSecondPending. Bit64 is reserved for RepeatedHour if time is specified in local time.
                                                   //ToDo: Consider using all 4 values to maintain support with DateTime. 
        public readonly short Offset;              //The minute offset of the time. 0 for UTC time.
        public readonly SttpTimeFlags Flags;

        public SttpTime(DateTime value) 
        {
            throw new NotImplementedException();
        }

        public SttpTime(DateTimeOffset dateTimeOffset)
        {
            throw new NotImplementedException();
        }

        public SttpTime(TimeSpan dateTimeOffset)
        {
            throw new NotImplementedException();
        }

        //public SttpTime2(DateTime time, bool leapSecondInProgress = false)
        //{

        //}

        //public SttpTime2(long rawValue)
        //{
        //    RawValue = rawValue;
        //}

        public DateTime Ticks => new DateTime(0);

        public bool LeapsecondInProgress => (0) > 0;

        public TimeSpan AsTimeSpan { get; set; }
        public DateTime AsDateTime { get; set; }
        public DateTimeOffset AsDateTimeOffset { get; set; }

        public static SttpTime Parse(string isString)
        {
            throw new NotImplementedException();
        }
    } // 8-bytes
}
