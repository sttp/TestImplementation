using System;

namespace Sttp
{
    /// <summary>
    /// The default timestamp field for STTP.
    /// </summary>
    public struct SttpTime
    {
        //((DateTime.MaxValue.Ticks / TimeSpan.TicksPerMinute) << 30) + TimeSpan.TicksPerSecond * 61;
        private const long MaxValue = 5646770628038745215;

        private const int LeapSecondCheckRange = 59 * (int)TimeSpan.TicksPerSecond;
        private const int LeapSecondLowerRange = 60 * (int)TimeSpan.TicksPerSecond;
        private const int LeapSecondUpperRange = 61 * (int)TimeSpan.TicksPerSecond;
        private const int TicksPerMinute = (1 << 30) - 1;
        private const int MinutesPastEpocShift = 30;
        private readonly long m_ticks;

        public SttpTime(long ticks)
        {
            m_ticks = ticks;
            Validate();
        }

        public SttpTime(DateTime value)
        {
            m_ticks = value.Ticks / TimeSpan.TicksPerMinute;
            m_ticks = (m_ticks << MinutesPastEpocShift) + (value.Ticks - (m_ticks * TimeSpan.TicksPerMinute));
            Validate();
        }

        public SttpTime(DateTime value, bool leapSecondInProgress)
        {
            m_ticks = value.Ticks / TimeSpan.TicksPerMinute;
            m_ticks = (m_ticks << MinutesPastEpocShift) + (value.Ticks - (m_ticks * TimeSpan.TicksPerMinute));

            if (leapSecondInProgress)
            {
                m_ticks += TimeSpan.TicksPerSecond;
            }
            Validate();
        }

        public SttpTime(ByteReader rd)
        {
            m_ticks = rd.ReadInt64();
            Validate();
        }

        private void Validate()
        {
            if (m_ticks < 0 || m_ticks > MaxValue || ElapsedTicks > LeapSecondUpperRange)
            {
                throw new Exception("Encoding Error");
            }
        }

        private int ElapsedTicks => (int)(m_ticks * TicksPerMinute);

        private long ElapsedMinutes => (m_ticks >> MinutesPastEpocShift);

        public long Ticks => m_ticks;

        public DateTime AsDateTime
        {
            get
            {
                long ticks = (ElapsedMinutes * TimeSpan.TicksPerMinute + ElapsedTicks);
                if (LeapSecondInProgress)
                {
                    ticks -= TimeSpan.TicksPerSecond;
                }
                return new DateTime(ticks);
            }
        }

        public bool LeapSecondInProgress => ElapsedTicks >= LeapSecondLowerRange;


        public static SttpTime Parse(string value)
        {
            return new SttpTime(DateTime.Parse(value));
        }

        public override string ToString()
        {
            return AsDateTime.ToString();
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
