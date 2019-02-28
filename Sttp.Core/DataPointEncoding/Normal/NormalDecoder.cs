using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;
using Sttp.Codec;

namespace Sttp.DataPointEncoding
{
    public class NormalDecoder : DecoderBase
    {
        private MetadataChannelMapDecoder m_channelMap;
        private CtpObjectReader m_stream;
        private int m_lastChannelID = 0;
        private CtpTime m_lastTimestamp;
        private long m_lastQuality = 0;

        public NormalDecoder(LookupMetadata lookup)
            : base(lookup)
        {
            m_stream = new CtpObjectReader();
            m_channelMap = new MetadataChannelMapDecoder();
        }

        public void Load(CommandDataStreamNormal data)
        {
            m_lastChannelID = 0;
            m_lastTimestamp = default(CtpTime);
            m_lastQuality = 0;
            m_stream.SetBuffer(data.ObjectStream);
        }

        public override bool Read(SttpDataPoint dataPoint)
        {
            TryAgain:
            if (m_stream.IsEmpty)
                return false;

            m_lastChannelID++;
            CtpObject value = m_stream.Read();

            if (value.ValueTypeCode == CtpTypeCode.Integer)
            {
                long code = value.IsInteger;
                if (0 <= code && code <= 15)
                {
                    bool channelIDChanged = (code & 1) != 0;
                    bool hasMetadata = (code & 2) != 0;
                    bool qualityChanged = (code & 4) != 0;
                    bool timeChanged = (code & 8) != 0;

                    if (channelIDChanged)
                        m_lastChannelID = m_stream.Read().AsInt32;

                    if (hasMetadata)
                        m_channelMap.Assign(LookupMetadata(m_stream.Read()), m_lastChannelID);

                    if (qualityChanged)
                        m_lastQuality = m_stream.Read().AsInt64;

                    if (timeChanged)
                        m_lastTimestamp = m_stream.Read().AsCtpTime;

                    value = m_stream.Read();
                }
                else if (code >= 16)
                {
                    value = code - 16;
                }
            }

            dataPoint.Metadata = m_channelMap.GetMetadata(m_lastChannelID);
            dataPoint.Quality = m_lastQuality;
            dataPoint.Time = m_lastTimestamp;
            dataPoint.Value = value;

            if (dataPoint.Metadata == null)
                goto TryAgain;

            return true;
        }



    }
}
