using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec.DataPoint
{
    public class BasicDecoder
    {
        private ByteReader m_stream;
        private int m_lastRuntimeID = 0;
        private SttpValueMutable m_lastTimestamp = new SttpValueMutable();
        private long m_lastQuality = 0;
        private SttpValueTypeCode m_lastValueCode;

        public BasicDecoder()
        {
            m_stream = new ByteReader();
        }

        public void Load(byte[] data)
        {
            m_lastRuntimeID = 0;
            m_lastTimestamp.SetNull();
            m_lastQuality = 0;
            m_lastValueCode = SttpValueTypeCode.Null;
            m_stream.SetBuffer(data, 0, data.Length);
        }

        public bool Read(SttpDataPoint dataPoint)
        {
            if (m_stream.IsEmpty)
            {
                //It's possible that this is not enough since items might eventually be stored with a few bits, so I need some kind of extra escape sequence.
                return false;
            }

            bool canUseRuntimeID = true;
            bool hasExtendedData = false;
            bool qualityChanged = false;
            bool timeChanged = false;
            bool typeChanged = false;

            if (m_stream.ReadBits1() == 0)
            {
                canUseRuntimeID = m_stream.ReadBits1() == 1;
                hasExtendedData = m_stream.ReadBits1() == 1;
                qualityChanged = m_stream.ReadBits1() == 1;
                timeChanged = m_stream.ReadBits1() == 1;
                typeChanged = m_stream.ReadBits1() == 1;
            }

            if (canUseRuntimeID)
            {
                m_lastRuntimeID ^= (int)(uint)m_stream.Read4BitSegments();
                dataPoint.DataPointRuntimeID = m_lastRuntimeID;
                dataPoint.DataPointID.SetNull();
            }
            else
            {
                dataPoint.DataPointRuntimeID = -1;
                SttpValueEncodingNative.Load(m_stream, dataPoint.DataPointID);
            }

            if (hasExtendedData)
            {
                SttpValueEncodingNative.Load(m_stream, dataPoint.ExtendedData);
            }
            else
            {
                dataPoint.ExtendedData.SetNull();
            }

            if (qualityChanged)
            {
                m_lastQuality = m_stream.ReadInt64();
            }
            dataPoint.Quality = m_lastQuality;

            if (timeChanged)
            {
                SttpValueEncodingNative.Load(m_stream, m_lastTimestamp);
            }

            dataPoint.Time.SetValue(m_lastTimestamp);

            if (typeChanged)
            {
                m_lastValueCode = (SttpValueTypeCode)m_stream.ReadBits4();
            }

            SttpValueEncodingWithoutType.Load(m_stream, m_lastValueCode, dataPoint.Value);
            return true;
        }



    }
}
