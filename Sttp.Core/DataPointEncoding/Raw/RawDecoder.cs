using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;
using Sttp.Codec;

namespace Sttp.DataPointEncoding
{
    public class RawDecoder : DecoderBase
    {
        private CtpObjectReader m_stream1;
        private BitStreamReader m_stream2;
        private CtpTime m_lastTimestamp;
        private long m_lastQuality;

        public RawDecoder(LookupMetadata lookup)
            : base(lookup)
        {
            m_stream1 = new CtpObjectReader();
            m_stream2 = new BitStreamReader();
        }

        public void Load(CommandDataStreamRaw data)
        {
            m_lastTimestamp = default(CtpTime);
            m_lastQuality = 0;
            m_stream1.SetBuffer(data.ObjectStream);
            m_stream2.SetBuffer(data.BitStream);
        }

        public override bool Read(SttpDataPoint dataPoint)
        {
            if (m_stream2.IsEmpty)
                return false;

            if (m_stream2.ReadBits1() == 0)
                m_lastQuality = (long)m_stream1.Read();
            dataPoint.Quality = m_lastQuality;

            if (m_stream2.ReadBits1() == 0)
                m_lastTimestamp = (CtpTime)m_stream1.Read();
            dataPoint.Time = m_lastTimestamp;

            dataPoint.Metadata = LookupMetadata(m_stream1.Read());
            dataPoint.Value = m_stream1.Read();

            return true;
        }



    }
}
