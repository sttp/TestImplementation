using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using CTP;

namespace Sttp.DataPointEncoding
{
    public class NormalEncoder : EncoderBase
    {
        private CtpObjectWriter m_stream;
        private int m_lastChannelID = 0;
        private CtpTime m_lastTimestamp;
        private long m_lastQuality = 0;
        private Dictionary<CtpObject, int> m_pointIDToChannelIDMapping = new Dictionary<CtpObject, int>();
        private List<SttpDataPointID> m_metadata = new List<SttpDataPointID>();

        public NormalEncoder()
        {
            m_stream = new CtpObjectWriter();
            Clear();
        }

        public override int Length => m_stream.Length;

        public sealed override void Clear()
        {
            m_lastChannelID = 0;
            m_lastTimestamp = default(CtpTime);
            m_lastQuality = 0;
            m_stream.Clear();
        }

        public override CommandObject ToArray()
        {
            return new CommandDataStreamNormal(m_stream.ToArray());
        }

        public override void AddDataPoint(SttpDataPoint point)
        {
            int channelID = m_lastChannelID += 1;
            bool includeMetadata = false;

            //Most of the time, measurements will be sequential, this limits a dictionary lookup
            if (channelID >= m_metadata.Count || !m_metadata[channelID].ID.Equals(point.DataPoint.ID))
            {
                channelID = GetChannelID(point, ref includeMetadata);
            }

            CtpObject value = point.Value;
            bool qualityChanged = point.Quality != m_lastQuality;
            bool timeChanged = point.Time != m_lastTimestamp;
            bool channelIDChanged = m_lastChannelID != channelID;
            bool isSpecialExclusion = (value.ValueTypeCode == CtpTypeCode.Integer && value.IsInteger > long.MaxValue - 16);

            if (includeMetadata || qualityChanged || timeChanged || channelIDChanged || isSpecialExclusion)
            {
                byte code = 0;
                if (channelIDChanged)
                    code |= 1;
                if (includeMetadata)
                    code |= 2;
                if (qualityChanged)
                    code |= 4;
                if (timeChanged)
                    code |= 8;

                m_stream.WriteExact(code);
                if (channelIDChanged)
                {
                    m_stream.WriteExact(channelID);
                    m_lastChannelID = channelID;
                }

                if (includeMetadata)
                {
                    m_stream.Write(point.DataPoint.ID);
                }
                if (qualityChanged)
                {
                    m_stream.WriteExact(point.Quality);
                    m_lastQuality = point.Quality;
                }
                if (timeChanged)
                {
                    m_stream.Write(point.Time);
                    m_lastTimestamp = point.Time;
                }
            }
            else if (value.ValueTypeCode == CtpTypeCode.Integer)
            {
                if (value.IsInteger >= 0)
                    value = value.IsInteger + 16;
            }

            m_stream.Write(value);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private int GetChannelID(SttpDataPoint point, ref bool includeMetadata)
        {
            if (!m_pointIDToChannelIDMapping.TryGetValue(point.DataPoint.ID, out var channelID))
            {
                includeMetadata = true;
                channelID = m_metadata.Count;
                m_pointIDToChannelIDMapping.Add(point.DataPoint.ID, channelID);
                m_metadata.Add(point.DataPoint);
            }
            return channelID;
        }
    }
}
