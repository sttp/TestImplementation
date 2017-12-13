using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.WireProtocol.SendDataPoints
{
    public class Encoder
    {
        private int m_maxRuntimeIDCache;
        private ByteWriter m_stream;

        int m_lastRuntimeID = 0;
        SttpTime m_lastTimestamp = default(SttpTime);
        byte m_lastTimeQuality = 0;
        byte m_lastValueQuality = 0;
        private SttpValueTypeCode m_lastValueCode;

        public Encoder(int maxRuntimeIDCache)
        {
            m_maxRuntimeIDCache = maxRuntimeIDCache;
            m_stream = new ByteWriter();
        }

        public void Clear()
        {
            m_lastRuntimeID = 0;
            m_lastTimestamp = default(SttpTime);
            m_lastTimeQuality = 0;
            m_lastValueQuality = 0;
            m_lastValueCode = SttpValueTypeCode.Null;
            m_stream = null;
        }

        public void AddDataPoint(SttpDataPoint point)
        {
            bool canUseRuntimeID = point.DataPointID.RuntimeID >= 0 && point.DataPointID.RuntimeID < m_maxRuntimeIDCache;
            bool hasExtraFields = point.ExtraFields != null && point.ExtraFields.Length > 0;
            bool timeQualityChanged = (byte)point.TimestampQuality != m_lastTimeQuality;
            bool valueQualityChanged = (byte)point.ValueQuality != m_lastValueQuality;
            bool timeChanged = point.Time != m_lastTimestamp;
            bool typeChanged = point.Value.ValueTypeCode != m_lastValueCode;

            if (canUseRuntimeID && !hasExtraFields && !timeQualityChanged && !valueQualityChanged && !timeChanged && !typeChanged)
            {
                m_stream.WriteBits1(true); //Is the common header.
            }
            else
            {
                m_stream.WriteBits1(false); //Is not the common header.
                m_stream.WriteBits1(canUseRuntimeID);
                m_stream.WriteBits1(hasExtraFields);
                m_stream.WriteBits1(timeQualityChanged);
                m_stream.WriteBits1(valueQualityChanged);
                m_stream.WriteBits1(timeChanged);
                m_stream.WriteBits1(typeChanged);
            }

            if (canUseRuntimeID)
            {
                int pointIDDelta = point.DataPointID.RuntimeID ^ m_lastRuntimeID;
                m_stream.Write4BitSegments((uint)pointIDDelta);
                m_lastRuntimeID = point.DataPointID.RuntimeID;
            }
            else
            {
                m_stream.WriteBits2((byte)point.DataPointID.ValueTypeCode);
                switch (point.DataPointID.ValueTypeCode)
                {
                    case SttpDataPointIDTypeCode.Null:
                        break;
                    case SttpDataPointIDTypeCode.Guid:
                        m_stream.Write(point.DataPointID.AsGuid);
                        break;
                    case SttpDataPointIDTypeCode.String:
                        m_stream.Write(point.DataPointID.AsString);
                        break;
                    case SttpDataPointIDTypeCode.SttpMarkup:
                        m_stream.Write(point.DataPointID.AsSttpMarkup);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (hasExtraFields)
            {
                m_stream.Write4BitSegments((uint)point.ExtraFields.Length);
                foreach (var field in point.ExtraFields)
                {
                    m_stream.Write(field);
                }
            }

            if (timeQualityChanged)
            {
                m_stream.Write((byte)point.TimestampQuality);
                m_lastTimeQuality = (byte)point.TimestampQuality;
            }

            if (valueQualityChanged)
            {
                m_stream.Write((byte)point.ValueQuality);
                m_lastValueQuality = (byte)point.ValueQuality;
            }

            if (timeChanged)
            {
                m_stream.Write(point.Time);
                m_lastTimestamp = point.Time;
            }

            if (typeChanged)
            {
                m_stream.WriteBits4((byte)point.Value.ValueTypeCode);
                m_lastValueCode = point.Value.ValueTypeCode;
            }

            point.Value.SaveWithoutTypeCode(m_stream);
        }
    }
}
