using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;
using Sttp.Codec;

namespace Sttp.DataPointEncoding
{
    public class BasicDecoder : DecoderBase
    {
        private MetadataChannelMapDecoder m_channelMap;
        private CtpObjectReader m_stream1;
        private BitStreamReader m_stream2;
        private int m_lastChannelID = 0;
        private CtpTime m_lastTimestamp;
        private long m_lastQuality = 0;
        
        public BasicDecoder(LookupMetadata lookup)
            : base(lookup)
        {
            m_stream1 = new CtpObjectReader();
            m_stream2 = new BitStreamReader();
            m_channelMap = new MetadataChannelMapDecoder();
        }

        public void Load(CommandDataStreamBasic data)
        {
            m_lastChannelID = 0;
            m_lastTimestamp = default(CtpTime);
            m_lastQuality = 0;
            m_stream1.SetBuffer(data.ObjectStream);
            m_stream2.SetBuffer(data.BitStream);
        }

        public override bool Read(SttpDataPoint dataPoint)
        {
            if (m_stream2.IsEmpty)
                return false;

            bool qualityChanged = false;
            bool timeChanged = false;
            
            if (m_stream2.ReadBits1() == 0)
            {
                qualityChanged = m_stream2.ReadBits1() == 1;
                timeChanged = m_stream2.ReadBits1() == 1;
            }

            m_lastChannelID ^= (int)(uint)m_stream2.Read4BitSegments();
            dataPoint.Metadata = m_channelMap.GetMetadata(m_lastChannelID);

            if (qualityChanged)
            {
                m_lastQuality = (long)m_stream1.Read();
            }
            dataPoint.Quality = m_lastQuality;

            if (timeChanged)
            {
                m_lastTimestamp = (CtpTime)m_stream1.Read();
            }

            dataPoint.Time = m_lastTimestamp;
            dataPoint.Value = m_stream1.Read();

            if (dataPoint.Metadata == null)
            {
                var obj = m_stream1.Read();
                dataPoint.Metadata = LookupMetadata(obj);
                m_channelMap.Assign(dataPoint.Metadata, m_lastChannelID);
            }
            return true;
        }



    }
}
