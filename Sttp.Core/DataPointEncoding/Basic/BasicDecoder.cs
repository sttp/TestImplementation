using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;
using Sttp.Codec;

namespace Sttp.DataPointEncoding
{
    public class BasicDecoder : DecoderBase
    {
        private MetadataChannelMapDecoder m_channelMap;
        private CtpObjectReader m_stream1;
        private BitStreamReader m_stream2;
        private int m_lastChannelID = 0;
        private CtpTime m_lastTimestamp;
        private long m_lastQuality = 0;
        private CtpTypeCode m_lastValueType = CtpTypeCode.Null;

        public BasicDecoder(LookupMetadata lookup)
            : base(lookup)
        {
            m_stream1 = new CtpObjectReader();
            m_stream2 = new BitStreamReader();
            m_channelMap = new MetadataChannelMapDecoder();
        }

        public void Load(CommandDataStreamBasic data)
        {
            m_lastValueType = CtpTypeCode.Single;
            m_lastChannelID = 0;
            m_lastTimestamp = new CtpTime(new DateTime(2020, 1, 1));
            m_lastQuality = 0;
            m_stream1.SetBuffer(data.ObjectStream);
            m_stream2.SetBuffer(data.BitStream);
        }

        public override bool Read(SttpDataPoint dataPoint)
        {
            TryAgain:
            if (m_stream2.IsEmpty)
                return false;

            bool qualityChanged = false;
            bool timeChanged = false;
            bool valueTypeChanged = false;
            bool channelIDChanged = false;
            bool hasMetadata = false;

            if (m_stream2.ReadBits1() == 1)
            {
                qualityChanged = m_stream2.ReadBits1() == 1;
                timeChanged = m_stream2.ReadBits1() == 1;
                valueTypeChanged = m_stream2.ReadBits1() == 1;
                channelIDChanged = m_stream2.ReadBits1() == 1;
                hasMetadata = m_stream2.ReadBits1() == 1;
            }

            if (channelIDChanged)
            {
                m_lastChannelID = CustomBitEncoding.ReadPointID(m_stream2);
            }
            else
            {
                m_lastChannelID++;
            }

            if (hasMetadata)
            {
                CtpObject dataPointID = m_stream1.Read();
                m_channelMap.Assign(LookupMetadata(dataPointID), m_lastChannelID);
            }
            dataPoint.Metadata = m_channelMap.GetMetadata(m_lastChannelID);

            if (qualityChanged)
            {
                m_lastQuality = CustomBitEncoding.ReadInt64(m_stream2);
            }
            dataPoint.Quality = m_lastQuality;

            if (timeChanged)
            {
                m_lastTimestamp = CustomBitEncoding.ReadTimeChanged(m_stream2, m_lastTimestamp);
            }
            dataPoint.Time = m_lastTimestamp;

            if (valueTypeChanged)
            {
                m_lastValueType = (CtpTypeCode)m_stream2.ReadBits4();
            }

            switch (m_lastValueType)
            {
                case CtpTypeCode.Null:
                    dataPoint.Value = CtpObject.Null;
                    break;
                //case CtpTypeCode.Int8:
                //    dataPoint.Value = CustomBitEncoding.ReadInt8(m_stream2);
                //    break;
                //case CtpTypeCode.Int16:
                //    dataPoint.Value = CustomBitEncoding.ReadInt16(m_stream2);
                //    break;
                //case CtpTypeCode.Int32:
                //    dataPoint.Value = CustomBitEncoding.ReadInt32(m_stream2);
                //    break;
                //case CtpTypeCode.Int64:
                //    dataPoint.Value = CustomBitEncoding.ReadInt64(m_stream2);
                //    break;
                case CtpTypeCode.Single:
                    dataPoint.Value = CustomBitEncoding.ReadSingle(m_stream2);
                    break;
                case CtpTypeCode.Double:
                    dataPoint.Value = CustomBitEncoding.ReadDouble(m_stream2);
                    break;
                case CtpTypeCode.Boolean:
                    dataPoint.Value = CustomBitEncoding.ReadBoolean(m_stream2);
                    break;
                case CtpTypeCode.CtpTime:
                case CtpTypeCode.Numeric:
                case CtpTypeCode.Guid:
                case CtpTypeCode.String:
                case CtpTypeCode.CtpBuffer:
                case CtpTypeCode.CtpCommand:
                default:
                    dataPoint.Value = m_stream1.Read();
                    break;
            }

            if (dataPoint.Metadata == null)
                goto TryAgain;

            return true;
        }



    }
}
