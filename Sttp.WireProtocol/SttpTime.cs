using System;
using System.Runtime.Serialization;

namespace Sttp
{
    public struct SttpTime : IComparable<SttpTime>, IEquatable<SttpTime>, IComparable, IFormattable, ISerializable
    {
        //((DateTime.MaxValue.Ticks / TimeSpan.TicksPerMinute) << 30) + TimeSpan.TicksPerSecond * 61;
        private const long MaxTicks = 5646770628038745215;
        private const int LeapSecondCheckRange = 59 * (int)TimeSpan.TicksPerSecond;
        private const int LeapSecondLowerRange = 60 * (int)TimeSpan.TicksPerSecond;
        private const int LeapSecondUpperRange = 61 * (int)TimeSpan.TicksPerSecond;
        private const int TicksPerMinuteMask = (1 << 30) - 1;
        private const int MinutesPastEpocShift = 30;
        private readonly long m_ticks;

        public static readonly SttpTime MinValue = new SttpTime(0);
        public static readonly SttpTime MaxValue = new SttpTime(MaxTicks);

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

        private void Validate()
        {
            if (m_ticks < 0 || m_ticks > MaxTicks || TicksPastMinute > LeapSecondUpperRange)
            {
                throw new Exception("Encoding Error");
            }
        }

        public int TicksPastMinute => (int)(m_ticks & TicksPerMinuteMask);

        public long MinutesPastEpoc => (m_ticks >> MinutesPastEpocShift);

        public long Ticks => m_ticks;

        public DateTime AsDateTime
        {
            get
            {
                long ticks = (MinutesPastEpoc * TimeSpan.TicksPerMinute + TicksPastMinute);
                if (LeapSecondInProgress)
                {
                    ticks -= TimeSpan.TicksPerSecond;
                }
                return new DateTime(ticks);
            }
        }

        public bool LeapSecondInProgress => TicksPastMinute >= LeapSecondLowerRange;


        public static SttpTime Parse(string value)
        {
            return new SttpTime(DateTime.Parse(value));
        }

        public int CompareTo(SttpTime other)
        {
            return m_ticks.CompareTo(other.m_ticks);
        }

        public bool Equals(SttpTime other)
        {
            return m_ticks == other.m_ticks;
        }

        public static bool operator ==(SttpTime a, SttpTime b)
        {
            return a.m_ticks == b.m_ticks;
        }

        public static bool operator !=(SttpTime a, SttpTime b)
        {
            return a.m_ticks != b.m_ticks;
        }

        public static bool operator <(SttpTime a, SttpTime b)
        {
            return a.m_ticks < b.m_ticks;
        }

        public static bool operator >(SttpTime a, SttpTime b)
        {
            return a.m_ticks > b.m_ticks;
        }

        public static bool operator <=(SttpTime a, SttpTime b)
        {
            return a.m_ticks <= b.m_ticks;
        }

        public static bool operator >=(SttpTime a, SttpTime b)
        {
            return a.m_ticks >= b.m_ticks;
        }

        public static explicit operator DateTime(SttpTime a)
        {
            return a.AsDateTime;
        }

        public static explicit operator SttpTime(DateTime a)
        {
            return new SttpTime(a);
        }


        public override bool Equals(object value)
        {
            if (value is SttpTime)
            {
                return m_ticks == ((SttpTime)value).m_ticks;
            }
            return false;
        }

        public override string ToString()
        {
            return AsDateTime.ToString();
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            info.AddValue("ticks", m_ticks);
        }

        public override int GetHashCode()
        {
            return (int)m_ticks ^ (int)(m_ticks >> 32);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object value)
        {
            if (value == null)
            {
                return 1;
            }
            if (!(value is SttpTime))
            {
                throw new ArgumentException("Type must be SttpTime");
            }
            long internalTicks = ((SttpTime)value).m_ticks;
            long internalTicks2 = m_ticks;
            if (internalTicks2 > internalTicks)
            {
                return 1;
            }
            if (internalTicks2 < internalTicks)
            {
                return -1;
            }
            return 0;
        }


    } // 8-bytes
}
