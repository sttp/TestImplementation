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

        public BasicEncoder()
        {
            m_stream1 = new CtpObjectWriter();
            m_stream2 = new BitStreamWriter();
            m_channelMap = new MetadataChannelMapEncoder();
        }

        public override int Length => m_stream1.Length + m_stream2.Length;

        public override void Clear()
        {
            m_lastValueType = CtpTypeCode.Single;
            m_lastChannelID = 0;
            m_lastTimestamp = new CtpTime(new DateTime(2020, 1, 1));
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
            int channelID = m_channelMap.GetChannelID(point.Metadata, out var sendMetadata);
            bool qualityChanged = point.Quality != m_lastQuality;
            bool timeChanged = point.Time != m_lastTimestamp;
            bool valueTypeChanged = point.Value.ValueTypeCode != m_lastValueType;
            bool channelIDChanged = m_lastChannelID + 1 != channelID;

            if (!sendMetadata && !qualityChanged && !timeChanged && !valueTypeChanged && !channelIDChanged)
            {
                m_stream2.WriteBits1(0); //Is the common header. (~75% of the time, this is true)
            }
            else
            {
                m_stream2.WriteBits2(1); //Is not the common header.
                m_stream2.WriteBits1(qualityChanged); //Rare
                m_stream2.WriteBits1(timeChanged); //Nearly Rare
                m_stream2.WriteBits1(valueTypeChanged);
                m_stream2.WriteBits1(channelIDChanged);
                m_stream2.WriteBits1(sendMetadata); //Rare
            }

            if (channelIDChanged)
            {
                CustomBitEncoding.WritePointID(m_stream2, channelID);
            }
            m_lastChannelID = channelID;

            if (sendMetadata)
            {
                m_stream1.Write(point.Metadata.DataPointID);
            }

            if (qualityChanged)
            {
                CustomBitEncoding.WriteInt64(m_stream2, point.Quality);
                m_lastQuality = point.Quality;
            }

            if (timeChanged)
            {
                CustomBitEncoding.WriteTimeChanged(m_stream2, m_lastTimestamp, point.Time);
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
                    CustomBitEncoding.WriteInt64(m_stream2, point.Value.IsInt64);
                    break;
                case CtpTypeCode.Single:
                    CustomBitEncoding.WriteSingle(m_stream2, point.Value.IsSingle);
                    break;
                case CtpTypeCode.Double:
                    CustomBitEncoding.WriteDouble(m_stream2, point.Value.IsDouble);
                    break;
                case CtpTypeCode.Boolean:
                    CustomBitEncoding.WriteBoolean(m_stream2, point.Value.IsBoolean);
                    break;
                case CtpTypeCode.CtpTime:
                case CtpTypeCode.Numeric:
                case CtpTypeCode.Guid:
                case CtpTypeCode.String:
                case CtpTypeCode.CtpBuffer:
                case CtpTypeCode.CtpCommand:
                default:
                    m_stream1.Write(point.Value);
                    break;
            }
        }


    }
}
