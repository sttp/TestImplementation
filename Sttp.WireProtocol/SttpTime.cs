using System;

namespace Sttp
{
    /// <summary>
    /// The default timestamp field for STTP.
    /// </summary>
    public struct SttpTime
    {
        //This is DateTime.MaxValue + 1
        private const long LeapSecondRange = 3155378976000000000;
        private const long TicksMask = (1L << 62) - 1;
        private readonly long m_ticks;

        public SttpTime(DateTime value)
        {
            m_ticks = (long)value.Ticks;
        }

        public SttpTime(ByteReader rd)
        {
            m_ticks = rd.ReadInt64();
        }

        private long InternalTicks => (m_ticks & TicksMask);

        public DateTime Ticks
        {
            get
            {
                if (LeapsecondInProgress)
                {
                    long leapSecondRange = (InternalTicks - LeapSecondRange);
                    long minutesPastEpoc = leapSecondRange / TimeSpan.TicksPerSecond;
                    long ticksIn60 = leapSecondRange % TimeSpan.TicksPerSecond;
                    return new DateTime(minutesPastEpoc * TimeSpan.TicksPerMinute + TimeSpan.TicksPerSecond * 59 + ticksIn60); //A leap second will be wrapped into the 59 second mark.
                }
                return new DateTime(InternalTicks);
            }
        }

        public bool LeapsecondInProgress => InternalTicks >= LeapSecondRange;

        public DateTime AsDateTime => Ticks;

        public static SttpTime Parse(string value)
        {
            return new SttpTime(DateTime.Parse(value));
        }

        public override string ToString()
        {
            return Ticks.ToString();
        }

        public void Save(ByteWriter wr)
        {
            wr.Write(m_ticks);
        }

        public static bool operator ==(SttpTime a, SttpTime b)
        {
            return a.m_ticks == b.m_ticks;
        }

        public static bool operator !=(SttpTime a, SttpTime b)
        {
            return !(a == b);
        }
    } // 8-bytes
}
