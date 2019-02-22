using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private CtpTypeCode m_lastValueType = CtpTypeCode.Null;
        private CustomBitEncoding m_encoder;
        private bool m_isStart;

        public BasicEncoder()
        {
            m_stream1 = new CtpObjectWriter();
            m_stream2 = new BitStreamWriter();
            m_channelMap = new MetadataChannelMapEncoder();
            m_encoder = new CustomBitEncoding(m_stream2);
        }

        public override int Length => m_stream1.Length + m_stream2.Length;

        public override void Clear()
        {
            m_isStart = true;
            m_lastValueType = CtpTypeCode.Null;
            m_lastChannelID = 0;
            m_lastTimestamp = default(CtpTime);
            m_lastQuality = 0;
            m_stream1.Clear();
            m_stream2.Clear();
            m_encoder.Clear();
        }

        public override CtpCommand ToArray()
        {
            return new CommandDataStreamBasic(m_stream1.ToArray(), m_stream2.ToArray());
        }

        public override void AddDataPoint(SttpDataPoint point)
        {
            int channelID = m_channelMap.GetChannelID(point.Metadata, out var isNew);

            if (m_isStart)
            {
                m_encoder.WriteInt32(channelID);
                m_encoder.WriteInt64(point.Quality);
                m_encoder.WriteTime(point.Time);
                m_stream1.Write(point.Value);
                if (isNew)
                    m_stream1.Write(point.Metadata.DataPointID);

                m_lastChannelID = channelID;
                m_lastQuality = point.Quality;
                m_lastTimestamp = point.Time;
                m_lastValueType = point.Value.ValueTypeCode;
                m_isStart = false;
                return;
            }


            bool qualityChanged = point.Quality != m_lastQuality;
            bool timeChanged = point.Time != m_lastTimestamp;
            bool valueTypeChanged = point.Value.ValueTypeCode != m_lastValueType;
            bool channelIDChanged = m_lastChannelID + 1 != channelID;

            if (!qualityChanged && !timeChanged && !valueTypeChanged && !channelIDChanged)
            {
                m_stream2.WriteBits1(true); //Is the common header.
            }
            else
            {
                m_stream2.WriteBits1(false); //Is not the common header.
                m_stream2.WriteBits1(qualityChanged);
                m_stream2.WriteBits1(timeChanged);
                m_stream2.WriteBits1(valueTypeChanged);
                m_stream2.WriteBits1(channelIDChanged);
            }

            if (channelIDChanged)
            {
                m_encoder.WriteInt32(channelID);
            }
            m_lastChannelID = channelID;

            if (qualityChanged)
            {
                m_encoder.WriteInt64(point.Quality);
                m_lastQuality = point.Quality;
            }

            if (timeChanged)
            {
                m_encoder.WriteBitsChanged64((ulong)(point.Time.Ticks ^ m_lastTimestamp.Ticks));
                m_lastTimestamp = point.Time;
            }

            if (valueTypeChanged)
            {
                m_stream2.WriteBits4((byte)point.Value.ValueTypeCode);
                m_lastValueType = point.Value.ValueTypeCode;
            }

            switch (point.Value.ValueTypeCode)
            {
                case CtpTypeCode.Null:
                    break;
                case CtpTypeCode.Int64:
                    m_encoder.WriteInt64(point.Value.IsInt64);
                    break;
                case CtpTypeCode.Single:
                    m_encoder.WriteSingle(point.Value.IsSingle);
                    break;
                case CtpTypeCode.Double:
                    m_encoder.WriteDouble(point.Value.IsDouble);
                    break;
                case CtpTypeCode.Numeric:
                    break;
                case CtpTypeCode.CtpTime:
                    m_encoder.WriteTime(point.Value.IsCtpTime);
                    break;
                case CtpTypeCode.Boolean:
                    m_encoder.WriteBoolean(point.Value.IsBoolean);
                    break;
                case CtpTypeCode.Guid:
                case CtpTypeCode.String:
                case CtpTypeCode.CtpBuffer:
                case CtpTypeCode.CtpCommand:
                default:
                    m_stream1.Write(point.Value);
                    break;
            }
            if (isNew)
                m_stream1.Write(point.Metadata.DataPointID);
        }


    }
}
