using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using CTP;
using Sttp.Codec;

namespace Sttp.DataPointEncoding
{
    public class NormalEncoder : EncoderBase
    {
        private CtpObjectWriter m_stream;
        private int m_lastChannelID = 0;
        private CtpTime m_lastTimestamp;
        private long m_lastQuality = 0;
        private MetadataChannelMapEncoder m_channelMap;

        public NormalEncoder()
        {
            m_stream = new CtpObjectWriter();
            m_channelMap = new MetadataChannelMapEncoder();
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

        public override CtpCommand ToArray()
        {
            return new CommandDataStreamNormal(m_stream.ToArray());
        }

        public override void AddDataPoint(SttpDataPoint point)
        {
            CtpObject value = point.Value;
            int channelID = m_channelMap.GetChannelID(point.Metadata, out var includeMetadata);
            bool qualityChanged = point.Quality != m_lastQuality;
            bool timeChanged = point.Time != m_lastTimestamp;
            bool channelIDChanged = m_lastChannelID + 1 != channelID;
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

                m_stream.Write(code);
                if (channelIDChanged)
                    m_stream.Write(channelID);
                if (includeMetadata)
                    m_stream.Write(point.Metadata.DataPointID);
                if (qualityChanged)
                    m_stream.Write(point.Quality);
                if (timeChanged)
                    m_stream.Write(point.Time);
            }
            else if (value.ValueTypeCode == CtpTypeCode.Integer)
            {
                if (value.IsInteger >= 0)
                    value = value.IsInteger + 16;
            }

            m_stream.Write(value);
            m_lastChannelID = channelID;
            m_lastQuality = point.Quality;
            m_lastTimestamp = point.Time;
        }


    }
}
