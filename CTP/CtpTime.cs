using System;
using System.Runtime.Serialization;

namespace CTP
{
    public readonly struct CtpTime : IComparable<CtpTime>, IEquatable<CtpTime>, IComparable, IFormattable, ISerializable
    {
        //((DateTime.MaxValue.Ticks / TimeSpan.TicksPerMinute) << 30) + TimeSpan.TicksPerSecond * 61;
        private const long MaxTicks = 5646770628038745215;
        private const int LeapSecondCheckRange = 59 * (int)TimeSpan.TicksPerSecond;
        private const int LeapSecondLowerRange = 60 * (int)TimeSpan.TicksPerSecond;
        private const int LeapSecondUpperRange = 61 * (int)TimeSpan.TicksPerSecond;
        private const int TicksPerMinuteMask = (1 << 30) - 1;
        private const int MinutesPastEpocShift = 30;
        public readonly long Ticks;

        public static readonly CtpTime MinValue = new CtpTime(0);
        public static readonly CtpTime MaxValue = new CtpTime(MaxTicks);

        public CtpTime(long ticks)
        {
            Ticks = ticks;
            Validate();
        }

        public CtpTime(DateTime value)
        {
            Ticks = value.Ticks / TimeSpan.TicksPerMinute;
            Ticks = (Ticks << MinutesPastEpocShift) + (value.Ticks - (Ticks * TimeSpan.TicksPerMinute));
            Validate();
        }

        public CtpTime(DateTime value, bool leapSecondInProgress)
        {
            Ticks = value.Ticks / TimeSpan.TicksPerMinute;
            Ticks = (Ticks << MinutesPastEpocShift) + (value.Ticks - (Ticks * TimeSpan.TicksPerMinute));

            if (leapSecondInProgress)
            {
                Ticks += TimeSpan.TicksPerSecond;
            }
            Validate();
        }

        private void Validate()
        {
            if (Ticks < 0 || Ticks > MaxTicks || TicksPastMinute > LeapSecondUpperRange)
            {
                throw new Exception("Encoding Error");
            }
        }

        public int TicksPastMinute => (int)(Ticks & TicksPerMinuteMask);

        public long MinutesPastEpoc => (Ticks >> MinutesPastEpocShift);

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


        public static CtpTime Parse(string value)
        {
            return new CtpTime(DateTime.Parse(value));
        }

        public int CompareTo(CtpTime other)
        {
            return Ticks.CompareTo(other.Ticks);
        }

        public bool Equals(CtpTime other)
        {
            return Ticks == other.Ticks;
        }

        public static bool operator ==(CtpTime a, CtpTime b)
        {
            return a.Ticks == b.Ticks;
        }

        public static bool operator !=(CtpTime a, CtpTime b)
        {
            return a.Ticks != b.Ticks;
        }

        public static bool operator <(CtpTime a, CtpTime b)
        {
            return a.Ticks < b.Ticks;
        }

        public static bool operator >(CtpTime a, CtpTime b)
        {
            return a.Ticks > b.Ticks;
        }

        public static bool operator <=(CtpTime a, CtpTime b)
        {
            return a.Ticks <= b.Ticks;
        }

        public static bool operator >=(CtpTime a, CtpTime b)
        {
            return a.Ticks >= b.Ticks;
        }

        public static explicit operator DateTime(CtpTime a)
        {
            return a.AsDateTime;
        }

        public static explicit operator CtpTime(DateTime a)
        {
            return new CtpTime(a);
        }


        public override bool Equals(object value)
        {
            if (value is CtpTime)
            {
                return Ticks == ((CtpTime)value).Ticks;
            }
            return false;
        }

        public override string ToString()
        {
            return AsDateTime.ToString("O");
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            info.AddValue("ticks", Ticks);
        }

        public override int GetHashCode()
        {
            return (int)Ticks ^ (int)(Ticks >> 32);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return AsDateTime.ToString(format, formatProvider);
        }

        public int CompareTo(object value)
        {
            if (value == null)
            {
                return 1;
            }
            if (!(value is CtpTime))
            {
                throw new ArgumentException("Type must be SttpTime");
            }
            long internalTicks = ((CtpTime)value).Ticks;
            long internalTicks2 = Ticks;
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
