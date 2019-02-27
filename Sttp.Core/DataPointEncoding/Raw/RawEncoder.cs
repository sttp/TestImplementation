using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CTP;
using Sttp.Codec;

namespace Sttp.DataPointEncoding
{
    public class RawEncoder : EncoderBase
    {
        private CtpObjectWriter m_stream;
        private int m_lastChannelID = 0;
        private CtpTime m_lastTimestamp;
        private long m_lastQuality = 0;
        private MetadataChannelMapEncoder m_channelMap;

        public RawEncoder()
        {
            m_stream = new CtpObjectWriter();
            m_channelMap = new MetadataChannelMapEncoder();
        }

        public override int Length => m_stream.Length;

        public override void Clear()
        {
            m_lastChannelID = 0;
            m_lastTimestamp = new CtpTime(new DateTime(2020, 1, 1));
            m_lastQuality = 0;
            m_stream.Clear();
        }

        public override CtpCommand ToArray()
        {
            return new CommandDataStreamRaw(m_stream.ToArray());
        }

        public override void AddDataPoint(SttpDataPoint point)
        {
            int channelID = m_channelMap.GetChannelID(point.Metadata, out var includeMetadata);
            bool qualityChanged = point.Quality != m_lastQuality;
            bool timeChanged = point.Time != m_lastTimestamp;
            bool channelIDChanged = m_lastChannelID + 1 != channelID;

            if (includeMetadata || qualityChanged || timeChanged || channelIDChanged || point.Value.ValueTypeCode == CtpTypeCode.Integer)
            {
                byte code = 0;
                if (qualityChanged)
                    code |= 1;
                if (timeChanged)
                    code |= 2;
                if (channelIDChanged)
                    code |= 4;
                if (includeMetadata)
                    code |= 8;

                m_stream.Write(code);
                if (channelIDChanged)
                    m_stream.Write(channelID);
                if (includeMetadata)
                    m_stream.Write(point.Metadata.DataPointID);
                if (qualityChanged)
                    m_stream.Write(point.Quality);
                if (timeChanged)
                    m_stream.Write(CompareUInt64.Compare((ulong)point.Time.Ticks, (ulong)m_lastTimestamp.Ticks));
            }
            m_stream.Write(point.Value);
            m_lastChannelID = channelID;
            m_lastQuality = point.Quality;
            m_lastTimestamp = point.Time;
        }

    }
}
