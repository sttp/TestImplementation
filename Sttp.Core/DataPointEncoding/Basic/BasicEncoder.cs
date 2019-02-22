using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CTP;
using Sttp.Codec;

namespace Sttp.DataPointEncoding
{
    public class BasicEncoder : EncoderBase
    {
        private CtpObjectWriter m_stream1;
        private BitStreamWriter m_stream2;
        private int m_lastChannelID = 0;
        private CtpTime m_lastTimestamp;
        private long m_lastQuality = 0;
        private MetadataChannelMapEncoder m_channelMap;

        public BasicEncoder()
        {
            m_stream1 = new CtpObjectWriter();
            m_stream2 = new BitStreamWriter();
            m_channelMap = new MetadataChannelMapEncoder();
        }

        public override int Length => m_stream1.Length + m_stream2.Length;

        public override void Clear()
        {
            m_lastChannelID = 0;
            m_lastTimestamp = default(CtpTime);
            m_lastQuality = 0;
            m_stream1.Clear();
            m_stream2.Clear();
        }

        public override CtpCommand ToArray()
        {
            return new CommandDataStreamBasic(m_stream1.ToArray(), m_stream2.ToArray());
        }

        public override void AddDataPoint(SttpDataPoint point)
        {
            bool qualityChanged = point.Quality != m_lastQuality;
            bool timeChanged = point.Time != m_lastTimestamp;

            if (!qualityChanged && !timeChanged)
            {
                m_stream2.WriteBits1(true); //Is the common header.
            }
            else
            {
                m_stream2.WriteBits1(false); //Is not the common header.
                m_stream2.WriteBits1(qualityChanged);
                m_stream2.WriteBits1(timeChanged);
            }

            int channelID = m_channelMap.GetChannelID(point.Metadata, out var isNew);
            int pointIDDelta = channelID ^ m_lastChannelID;
            m_stream2.Write4BitSegments((uint)pointIDDelta);
            m_lastChannelID = channelID;

            if (qualityChanged)
            {
                m_stream1.Write(point.Quality);
            }

            if (timeChanged)
            {
                m_stream1.Write(point.Time);
                m_lastTimestamp = point.Time;
            }

            m_stream1.Write(point.Value);

            if (isNew)
            {
                m_stream1.Write(point.Metadata.DataPointID);
            }
        }

       
    }
}
