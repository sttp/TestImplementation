﻿using System;
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
        private ByteWriter m_stream;
        private CtpTime m_lastTimestamp;
        private long m_lastQuality = 0;
        private CtpTypeCode m_lastValueCode;

        public RawEncoder()
        {
            m_stream = new ByteWriter();
        }

        public override int Length => m_stream.Length;

        public override void Clear(bool clearMapping)
        {
            m_lastTimestamp = default(CtpTime);
            m_lastQuality = 0;
            m_lastValueCode = CtpTypeCode.Null;
            m_stream.Clear();
        }

        public override void AddDataPoint(SttpDataPoint point)
        {
            bool qualityChanged = point.Quality != m_lastQuality;
            bool timeChanged = point.Time != m_lastTimestamp;
            bool typeChanged = point.Value.ValueTypeCode != m_lastValueCode;

            if (!qualityChanged && !timeChanged && !typeChanged)
            {
                m_stream.WriteBits1(true); //Is the common header.
            }
            else
            {
                m_stream.WriteBits1(false); //Is not the common header.
                m_stream.WriteBits1(qualityChanged);
                m_stream.WriteBits1(timeChanged);
                m_stream.WriteBits1(typeChanged);
            }

            CtpValueEncodingNative.Save(m_stream, point.Metadata.DataPointID);

            if (qualityChanged)
            {
                m_stream.Write(point.Quality);
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

            CtpValueEncodingWithoutType.Save(m_stream, point.Value);
        }

        public override byte[] ToArray()
        {
            return m_stream.ToArray();
        }
    }
}