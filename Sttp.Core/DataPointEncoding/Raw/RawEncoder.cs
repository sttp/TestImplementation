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
        private CtpObjectWriter m_stream1;
        private BitStreamWriter m_stream2;
        private CtpTime m_lastTimestamp;
        private long m_lastQuality = 0;

        public RawEncoder()
        {
            m_stream1 = new CtpObjectWriter();
            m_stream2 = new BitStreamWriter();
        }

        public override int Length => m_stream1.Length + m_stream2.Length;

        public override void Clear()
        {
            m_lastTimestamp = default(CtpTime);
            m_lastQuality = 0;
            m_stream1.Clear();
            m_stream2.Clear();
        }

        public override CtpCommand ToArray()
        {
            return new CommandDataStreamRaw(m_stream1.ToArray(), m_stream2.ToArray());
        }

        public override void AddDataPoint(SttpDataPoint point)
        {
            bool qualityChanged = point.Quality != m_lastQuality;
            bool timeChanged = point.Time != m_lastTimestamp;

            m_stream2.WriteBits1(qualityChanged);
            if (qualityChanged)
            {
                m_stream1.Write(point.Quality);
                m_lastQuality = point.Quality;
            }

            m_stream2.WriteBits1(timeChanged);
            if (timeChanged)
            {
                m_stream1.Write(point.Time);
                m_lastTimestamp = point.Time;
            }
            m_stream1.Write(point.Metadata.DataPointID);
            m_stream1.Write(point.Value);
        }


    }
}
