using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;

namespace Sttp.DataPointEncoding
{
    public class RawDecoder : DecoderBase
    {
        private ByteReader m_stream;
        private CtpTime m_lastTimestamp;
        private long m_lastQuality = 0;
        private CtpTypeCode m_lastValueCode;

        public RawDecoder(LookupMetadata lookup)
            : base(lookup)
        {
            m_stream = new ByteReader();
        }

        public override void Load(byte[] data, bool clearMapping)
        {
            m_lastTimestamp = default(CtpTime);
            m_lastQuality = 0;
            m_lastValueCode = CtpTypeCode.Null;
            m_stream.SetBuffer(data, 0, data.Length);
        }

        public override bool Read(SttpDataPoint dataPoint)
        {
            if (m_stream.IsEmpty)
            {
                //It's possible that this is not enough since items might eventually be stored with a few bits, so I need some kind of extra escape sequence.
                return false;
            }

            bool hasExtendedData = false;
            bool qualityChanged = false;
            bool timeChanged = false;
            bool typeChanged = false;

            if (m_stream.ReadBits1() == 0)
            {
                hasExtendedData = m_stream.ReadBits1() == 1;
                qualityChanged = m_stream.ReadBits1() == 1;
                timeChanged = m_stream.ReadBits1() == 1;
                typeChanged = m_stream.ReadBits1() == 1;
            }

            var obj = CtpValueEncodingNative.Load(m_stream);
            dataPoint.Metadata = LookupMetadata(obj);

            if (hasExtendedData)
            {
                dataPoint.ExtendedData = CtpValueEncodingNative.Load(m_stream);
            }
            else
            {
                dataPoint.ExtendedData = CtpObject.Null;
            }

            if (qualityChanged)
            {
                m_lastQuality = m_stream.ReadInt64();
            }
            dataPoint.Quality = m_lastQuality;

            if (timeChanged)
            {
                m_lastTimestamp = m_stream.ReadCtpTime();
            }

            dataPoint.Time = m_lastTimestamp;

            if (typeChanged)
            {
                m_lastValueCode = (CtpTypeCode)m_stream.ReadBits4();
            }

            dataPoint.Value = CtpValueEncodingWithoutType.Load(m_stream, m_lastValueCode);

            return true;
        }



    }
}
