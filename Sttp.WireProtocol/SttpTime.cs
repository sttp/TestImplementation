using System;

namespace Sttp
{
    [Flags]
    public enum SttpTimeFlags
    {
        None = 0,
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
        IsLocalTime = 1 << 2,
        /// <summary>
        /// Gets if the time specified is in local time or universal time.
        /// </summary>
        IsUniversalTime = 1 << 2,
        /// <summary>
        /// Indicates that the kind of the datetime is not specified.
        /// </summary>
        IsUnspecifiedTime = 1 << 3,

        IsOffsetValid = 1 << 4,
    }
    /// <summary>
    /// The default timestamp field for STTP.
    /// </summary>
    public struct SttpTime
    {
        private readonly uint m_upperTicks;
        private readonly uint m_lowerTicks;
        private readonly short m_offset;         //The minute offset of the time from UTC. 0 for UTC time. 
        private readonly SttpTimeFlags m_flags;

        public SttpTime(DateTime value)
        {
            ulong v = (ulong)value.Ticks;
            m_upperTicks = (uint)(v >> 32);
            m_lowerTicks = (uint)v;
            m_offset = 0;
            m_flags = SttpTimeFlags.None;

            switch (value.Kind)
            {
                case DateTimeKind.Unspecified:
                    m_flags |= SttpTimeFlags.IsUnspecifiedTime;
                    break;
                case DateTimeKind.Utc:
                    m_flags |= SttpTimeFlags.IsUniversalTime;
                    break;
                case DateTimeKind.Local:
                    m_flags |= SttpTimeFlags.IsLocalTime;

                    //ToDo: Extract the repeated hour out of the DateTime value.
                    //byte kind = (byte)(*(ulong*)&value >> 62);
                    //if (kind == )
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public SttpTime(DateTimeOffset value)
        {
            ulong v = (ulong)value.UtcTicks;
            m_upperTicks = (uint)(v >> 32);
            m_lowerTicks = (uint)v;
            m_offset = (short)(value.Offset.Ticks / TimeSpan.TicksPerMinute);
            m_flags = SttpTimeFlags.IsOffsetValid;
        }

        public SttpTime(ByteReader rd)
        {
            m_upperTicks = rd.ReadUInt32();
            m_lowerTicks = rd.ReadUInt32();
            m_offset = rd.ReadInt16();
            m_flags = (SttpTimeFlags)rd.ReadByte();
        }

        public DateTime UtcTicks => new DateTime(((long)m_upperTicks << 32) | m_lowerTicks, DateTimeKind.Utc);
        public DateTime Ticks => new DateTime((((long)m_upperTicks << 32) | m_lowerTicks) + TimeSpan.TicksPerMinute * m_offset, DateTimeKind.Unspecified);
        public TimeSpan Offset => new TimeSpan(TimeSpan.TicksPerMinute * m_offset);

        public bool LeapsecondInProgress => (m_flags & SttpTimeFlags.IsLeapSecondOccurring) != 0;

        public DateTime AsDateTime => Ticks;

        public DateTimeOffset AsDateTimeOffset => new DateTimeOffset(UtcTicks, new TimeSpan(m_offset));

        public static SttpTime Parse(string isString)
        {
            throw new NotImplementedException();
        }

        public void Save(ByteWriter wr)
        {
            wr.Write(m_upperTicks);
            wr.Write(m_lowerTicks);
            wr.Write(m_offset);
            wr.Write((byte)m_flags);
        }

        public static bool operator ==(SttpTime a, SttpTime b)
        {
            return a.m_upperTicks == b.m_upperTicks && a.m_lowerTicks == b.m_lowerTicks && a.m_offset == b.m_offset && a.m_flags == b.m_flags;
        }

        public static bool operator !=(SttpTime a, SttpTime b)
        {
            return !(a == b);
        }
    } // 8-bytes
}
