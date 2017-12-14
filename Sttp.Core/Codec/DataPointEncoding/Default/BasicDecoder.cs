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
        private SttpTime m_lastTimestamp = default(SttpTime);
        private byte m_lastTimeQuality = 0;
        private byte m_lastValueQuality = 0;
        private SttpValueTypeCode m_lastValueCode;

        public BasicDecoder()
        {
            m_stream = new ByteReader();
        }

        public void Load(byte[] data)
        {
            m_lastRuntimeID = 0;
            m_lastTimestamp = default(SttpTime);
            m_lastTimeQuality = 0;
            m_lastValueQuality = 0;
            m_lastValueCode = SttpValueTypeCode.Null;
            m_stream.SetBuffer(data, 0, data.Length);
        }

        public bool Read(SttpDataPoint dataPoint)
        {
            if (m_stream.Position == m_stream.Length)
            {
                //It's possible that this is not enough since items might eventually be stored with a few bits, so I need some kind of extra escape sequence.
                return false;
            }

            bool canUseRuntimeID = true;
            bool hasExtraFields = false;
            bool timeQualityChanged = false;
            bool valueQualityChanged = false;
            bool timeChanged = false;
            bool typeChanged = false;

            if (m_stream.ReadBits1() == 0)
            {
                canUseRuntimeID = m_stream.ReadBits1() == 1;
                hasExtraFields = m_stream.ReadBits1() == 1;
                timeQualityChanged = m_stream.ReadBits1() == 1;
                valueQualityChanged = m_stream.ReadBits1() == 1;
                timeChanged = m_stream.ReadBits1() == 1;
                typeChanged = m_stream.ReadBits1() == 1;
            }

            if (canUseRuntimeID)
            {
                m_lastRuntimeID ^= (int)(uint)m_stream.Read4BitSegments();
                if (dataPoint.DataPointID == null)
                    dataPoint.DataPointID = new SttpDataPointID();
                dataPoint.DataPointID.RuntimeID = m_lastRuntimeID;
                dataPoint.DataPointID.IsNull = true;
            }
            else
            {
                dataPoint.DataPointID.RuntimeID = -1;
                switch ((SttpDataPointIDTypeCode)m_stream.ReadBits2())
                {
                    case SttpDataPointIDTypeCode.Null:
                        dataPoint.DataPointID.IsNull = true;
                        break;
                    case SttpDataPointIDTypeCode.Guid:
                        dataPoint.DataPointID.AsGuid = m_stream.ReadGuid();
                        break;
                    case SttpDataPointIDTypeCode.String:
                        dataPoint.DataPointID.AsString = m_stream.ReadString();
                        break;
                    case SttpDataPointIDTypeCode.SttpMarkup:
                        dataPoint.DataPointID.AsSttpMarkup = m_stream.ReadSttpMarkup();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (hasExtraFields)
            {
                dataPoint.ExtraFields = new SttpValue[m_stream.Read4BitSegments()];
                for (int x = 0; x < dataPoint.ExtraFields.Length; x++)
                {
                    dataPoint.ExtraFields[x] = SttpValueEncodingNative.Load(m_stream);
                }
            }
            else
            {
                dataPoint.ExtraFields = null;
            }

            if (timeQualityChanged)
            {
                m_lastTimeQuality = m_stream.ReadByte();
            }
            dataPoint.TimestampQuality = (TimeQualityFlags)m_lastTimeQuality;

            if (valueQualityChanged)
            {
                m_lastValueQuality = m_stream.ReadByte();
            }
            dataPoint.ValueQuality = (ValueQualityFlags)m_lastValueQuality;

            if (timeChanged)
            {
                m_lastTimestamp = m_stream.ReadSttpTime();
            }
            dataPoint.Time = m_lastTimestamp;

            if (typeChanged)
            {
                m_lastValueCode = (SttpValueTypeCode)m_stream.ReadBits4();
            }

            dataPoint.Value = SttpValueEncodingWithoutType.Load(m_stream, m_lastValueCode);
            return true;
        }



    }
}
