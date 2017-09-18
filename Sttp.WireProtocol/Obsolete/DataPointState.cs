using System;
using System.IO;

namespace Sttp.WireProtocol
{
    public class DataPointState : IEncode
    {
        [Flags]
        public enum Includes : byte
        {
            TimeHi = 1 << 0,
            TimeLo = 1 << 1,
            Quality = 1 << 2,
            Sequence = 1 << 3
        }

        private long m_timeHi;
        private ulong m_timeLo;
        private TimestampFlags m_timestampFlags;
        private QualityFlags m_qualityFlags;
        private ushort m_sequence;
        private Includes m_includes;

        private static readonly long UnixBaseTicks = new DateTime(1970, 1, 1, 0, 0, 0).Ticks;
        private static readonly long NtpBaseTicks = new DateTime(1900, 1, 1, 0, 0, 0).Ticks;

        public long TimeHi => m_timeHi;
        public ulong TimeLo => m_timeLo;
        public TimestampFlags TimestampFlags => m_timestampFlags;
        public QualityFlags QualityFlags => m_qualityFlags;
        public ushort Sequence => m_sequence;
        public Includes IncludedValues => m_includes;

        public void SetTimestamp(DateTime timestamp, TimestampType type, TimestampFlags flags)
        {
            long value = timestamp.Ticks;
                     
            switch (type)
            {
                case TimestampType.Ticks:
                    SetTimestamp(new TicksTimestamp { Value = value, Flags = flags});
                    break;
                case TimestampType.Unix64:
                    value = UnixBaseTicks - value;
                    SetTimestamp(new Unix64Timestamp { Value = value / TimeSpan.TicksPerSecond, Flags = flags });
                    break;
                case TimestampType.NTP128:
                    value = NtpBaseTicks - value;
                    long seconds = value / TimeSpan.TicksPerSecond;
                    ulong fraction = (ulong)(value - (value - value % TimeSpan.TicksPerSecond)) / ulong.MaxValue;
                    SetTimestamp(new NTP128Timestamp { Seconds =  seconds, Fraction = fraction, Flags = flags });
                    break;
            }
        }

        public void SetTimestamp(TicksTimestamp timestamp)
        {
            m_timeHi = timestamp.Value;
            m_timestampFlags = timestamp.Flags;
            m_includes |= Includes.TimeHi;
        }

        public void SetTimestamp(Unix64Timestamp timestamp)
        {
            m_timeHi = timestamp.Value;
            m_timestampFlags = timestamp.Flags;
            m_includes |= Includes.TimeHi;
        }

        public void SetTimestamp(NTP128Timestamp timestamp)
        {
            m_timeHi = timestamp.Seconds;
            m_timeLo = timestamp.Fraction;
            m_timestampFlags = timestamp.Flags;
            m_includes |= Includes.TimeHi | Includes.TimeLo;
        }

        public void SetQualityFlags(QualityFlags flags)
        {
            m_qualityFlags = flags;
            m_includes |= Includes.Quality;
        }

        public void SetSequence(ushort sequence)
        {
            m_sequence = sequence;
            m_includes |= Includes.Sequence;
        }

        public byte[] Encode()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                if ((m_includes & Includes.TimeHi) > 0)
                    stream.Write(BigEndian.GetBytes(m_timeHi), 0, 8);

                if ((m_includes & Includes.TimeLo) > 0)
                    stream.Write(BigEndian.GetBytes(m_timeLo), 0, 8);

                if ((m_includes & Includes.TimeHi) > 0)
                    stream.WriteByte((byte)m_timestampFlags);

                if ((m_includes & Includes.Quality) > 0)
                    stream.WriteByte((byte)m_qualityFlags);

                if ((m_includes & Includes.Sequence) > 0)
                    stream.Write(BigEndian.GetBytes(m_sequence), 0, 2);

                return stream.ToArray();
            }
        }

        public static DataPointState Decode(byte[] buffer, int startIndex, int length, Includes includes)
        {
            return null;
        }
    }
}
