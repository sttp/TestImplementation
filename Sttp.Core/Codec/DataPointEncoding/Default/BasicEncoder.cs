using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CTP;
using Sttp.Codec;

namespace Sttp.Codec.DataPoint
{
    public class BasicEncoder
    {
        private int m_maxRuntimeIDCache;
        private ByteWriter m_stream;

        private int m_lastRuntimeID = 0;
        private readonly CtpValueMutable m_lastTimestamp = new CtpValueMutable();
        private long m_lastQuality = 0;
        private CtpTypeCode m_lastValueCode;

        public BasicEncoder(int maxRuntimeIDCache)
        {
            m_maxRuntimeIDCache = maxRuntimeIDCache;
            m_stream = new ByteWriter();
        }

        public void Clear()
        {
            m_lastRuntimeID = 0;
            m_lastTimestamp.SetNull();
            m_lastQuality = 0;
            m_lastValueCode = CtpTypeCode.Null;
            m_stream.Clear();
        }

        public void AddDataPoint(SttpDataPoint point)
        {
            bool canUseRuntimeID = point.DataPointRuntimeID >= 0 && point.DataPointRuntimeID < m_maxRuntimeIDCache;
            bool hasExtendedData = !point.ExtendedData.IsNull;
            bool qualityChanged = point.Quality != m_lastQuality;
            bool timeChanged = point.Time != m_lastTimestamp;
            bool typeChanged = point.Value.ValueTypeCode != m_lastValueCode;

            if (canUseRuntimeID && !hasExtendedData && qualityChanged && !timeChanged && !typeChanged)
            {
                m_stream.WriteBits1(true); //Is the common header.
            }
            else
            {
                m_stream.WriteBits1(false); //Is not the common header.
                m_stream.WriteBits1(canUseRuntimeID);
                m_stream.WriteBits1(hasExtendedData);
                m_stream.WriteBits1(qualityChanged);
                m_stream.WriteBits1(timeChanged);
                m_stream.WriteBits1(typeChanged);
            }

            if (canUseRuntimeID)
            {
                int pointIDDelta = point.DataPointRuntimeID ^ m_lastRuntimeID;
                m_stream.Write4BitSegments((uint)pointIDDelta);
                m_lastRuntimeID = point.DataPointRuntimeID;
            }
            else
            {
                CtpValueEncodingNative.Save(m_stream, point.DataPointID);
            }

            if (hasExtendedData)
            {
                CtpValueEncodingNative.Save(m_stream, point.ExtendedData);
            }

            if (qualityChanged)
            {
                m_stream.Write(point.Quality);
            }

            if (timeChanged)
            {
                CtpValueEncodingNative.Save(m_stream, point.Time);
                m_lastTimestamp.SetValue(point.Time);
            }

            if (typeChanged)
            {
                m_stream.WriteBits4((byte)point.Value.ValueTypeCode);
                m_lastValueCode = point.Value.ValueTypeCode;
            }

            CtpValueEncodingWithoutType.Save(m_stream, point.Value);
        }

        public byte[] ToArray()
        {
            return m_stream.ToArray();
        }
    }
}
