using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;
using Sttp.Collection;

namespace Sttp.DataPointEncoding
{
    public class AdvancedDecoder : DecoderBase
    {
        private long m_prevTimestamp;
        private AdvancedPointMetadata m_prevPoint;
        private AdvancedPointMetadata m_nextPoint;
        private AdvancedWordEncoding m_encoder => m_prevPoint.NextValueEncoding;
        //private AdvancedTimestampEncoding m_timeEncoding;
        private IndexedArray<AdvancedPointMetadata> m_points;
        private ByteReader m_reader;
        private MetadataChannelMapDecoder m_channelMap;
        private int m_count = 0;

        public AdvancedDecoder(LookupMetadata lookup)
            : base(lookup)
        {
            m_reader = new ByteReader();
            m_channelMap = new MetadataChannelMapDecoder();
            m_points = new IndexedArray<AdvancedPointMetadata>();
            m_prevPoint = new AdvancedPointMetadata(null, m_reader);
            //m_timeEncoding = new AdvancedTimestampEncoding(null, m_reader);
        }

        public override void Load(byte[] data, bool clearMapping)
        {
            m_reader.SetBuffer(data, 0, data.Length);
            if (clearMapping)
            {
                m_points = new IndexedArray<AdvancedPointMetadata>();
                m_prevPoint = new AdvancedPointMetadata(null, m_reader);
                //m_timeEncoding = new AdvancedTimestampEncoding(null, m_reader);
                m_prevTimestamp = 0;
                m_channelMap.Clear();
            }
        }

        public override unsafe bool Read(SttpDataPoint dataPoint)
        {
            int channelID;
            if (m_reader.IsEmpty)
            {
                return false;
            }
            m_count++;

            if (m_count == 454789)
                m_count = 454789;

            var code = m_encoder.ReadCode();
            if (code == AdvancedCodeWords.NewPoint)
            {
                channelID = m_channelMap.GetNextChannelID;
                CtpObject dataPointID = CtpValueEncodingNative.Load(m_reader);
                m_prevTimestamp = (long)CompareUInt64.UnCompare(m_reader.Read8BitSegments(), (ulong)m_prevTimestamp);
                dataPoint.Time = new CtpTime(m_prevTimestamp);
                dataPoint.Quality = (long)m_reader.Read8BitSegments();
                dataPoint.Value = CtpValueEncodingNative.Load(m_reader);
                dataPoint.Metadata = LookupMetadata(dataPointID);
                m_channelMap.Assign(dataPoint.Metadata, channelID);
                m_prevPoint.NeighborChannelId = channelID;
                m_prevPoint = new AdvancedPointMetadata(null, m_reader);
                m_prevPoint.Assign(dataPoint);
                m_points[channelID] = m_prevPoint;
                return true;
            }

            channelID = m_prevPoint.NeighborChannelId;
            if (code == AdvancedCodeWords.PointID)
            {
                channelID = (int)m_reader.Read8BitSegments();
                m_prevPoint.NeighborChannelId = channelID;
                code = m_encoder.ReadCode();
                if (code <= AdvancedCodeWords.PointID)
                    throw new Exception("Parsing Error");
            }

            m_nextPoint = m_points[channelID];
            dataPoint.Metadata = m_channelMap.GetMetadata(channelID);

            if (code <= AdvancedCodeWords.Timestamp)
            {
                switch (m_reader.ReadBits2())
                {
                    case 0:
                        m_prevTimestamp = m_nextPoint.TimeDelta1 + m_nextPoint.PrevTime;
                        break;
                    case 1:
                        m_prevTimestamp = m_nextPoint.TimeDelta2 + m_nextPoint.PrevTime;
                        break;
                    case 2:
                        m_prevTimestamp = m_nextPoint.TimeDelta3 + m_nextPoint.PrevTime;
                        var delta3 = m_nextPoint.TimeDelta3;
                        m_nextPoint.TimeDelta3 = m_nextPoint.TimeDelta2;
                        m_nextPoint.TimeDelta2 = m_nextPoint.TimeDelta1;
                        m_nextPoint.TimeDelta1 = delta3;
                        break;
                    case 3:
                        m_prevTimestamp = (long)CompareUInt64.UnCompare(m_reader.Read8BitSegments(), (ulong)m_nextPoint.PrevTime);
                        m_nextPoint.TimeDelta3 = m_nextPoint.TimeDelta2;
                        m_nextPoint.TimeDelta2 = m_nextPoint.TimeDelta1;
                        m_nextPoint.TimeDelta1 = m_prevTimestamp - m_nextPoint.PrevTime;
                        break;
                }
                code = m_encoder.ReadCode();
                if (code <= AdvancedCodeWords.Timestamp)
                    throw new Exception("Parsing Error");
            }

            dataPoint.Time = new CtpTime(m_prevTimestamp);

            if (code <= AdvancedCodeWords.Quality)
            {
                m_nextPoint.PrevQuality = (long)m_reader.Read8BitSegments();

                code = m_encoder.ReadCode();
                if (code <= AdvancedCodeWords.Quality)
                    throw new Exception("Parsing Error");

            }

            dataPoint.Quality = m_nextPoint.PrevQuality;

            if (code <= AdvancedCodeWords.Type)
            {
                m_nextPoint.PrevValue = CtpObject.CreateDefault((CtpTypeCode)m_reader.ReadBits4());

                code = m_encoder.ReadCode();
                if (code <= AdvancedCodeWords.Type)
                    throw new Exception("Parsing Error");
            }

            if (code == AdvancedCodeWords.ValueDefault)
            {
                m_nextPoint.PrevValue = CtpObject.CreateDefault(m_nextPoint.PrevValue.ValueTypeCode);
            }
            else if (code == AdvancedCodeWords.ValuePrev)
            {

            }
            else
            {
                ulong value;
                switch (m_nextPoint.PrevValue.ValueTypeCode)
                {
                    case CtpTypeCode.Int64:
                        value = ReadInt64(code);
                        m_nextPoint.PrevValue = (long)value;
                        break;
                    case CtpTypeCode.Single:
                        value = ReadInt64(code);
                        uint v2 = (uint)value;
                        m_nextPoint.PrevValue = *(float*)&v2;
                        break;
                    case CtpTypeCode.Double:
                        value = ReadInt64(code);
                        m_nextPoint.PrevValue = *(double*)&value;
                        break;
                    case CtpTypeCode.CtpTime:
                        value = ReadInt64(code);
                        m_nextPoint.PrevValue = new CtpTime((long)value);
                        break;
                    case CtpTypeCode.Boolean:
                        if (code != AdvancedCodeWords.ValueTrue)
                            throw new Exception("Only true values are supported here.");
                        m_nextPoint.PrevValue = true;
                        break;
                    case CtpTypeCode.Guid:
                    case CtpTypeCode.String:
                    case CtpTypeCode.CtpCommand:
                    case CtpTypeCode.CtpBuffer:
                        if (code != AdvancedCodeWords.ValueRaw)
                            throw new Exception("Only raw types are supported here.");
                        m_nextPoint.PrevValue = CtpValueEncodingNative.Load(m_reader);
                        break;
                    case CtpTypeCode.Null:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            dataPoint.Value = m_nextPoint.PrevValue;
            m_nextPoint.Assign(dataPoint);
            m_prevPoint = m_nextPoint;
            return true;
        }

        private ulong ReadInt64(AdvancedCodeWords code)
        {
            switch (code)
            {
                case AdvancedCodeWords.ValueXOR4:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(4), m_nextPoint.PrevValue.AsRaw64);
                case AdvancedCodeWords.ValueXOR8:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(8), m_nextPoint.PrevValue.AsRaw64);
                case AdvancedCodeWords.ValueXOR10:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(10), m_nextPoint.PrevValue.AsRaw64);
                case AdvancedCodeWords.ValueXOR12:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(12), m_nextPoint.PrevValue.AsRaw64);
                case AdvancedCodeWords.ValueXOR14:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(14), m_nextPoint.PrevValue.AsRaw64);
                case AdvancedCodeWords.ValueXOR16:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(16), m_nextPoint.PrevValue.AsRaw64);
                case AdvancedCodeWords.ValueXOR18:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(18), m_nextPoint.PrevValue.AsRaw64);
                case AdvancedCodeWords.ValueXOR20:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(20), m_nextPoint.PrevValue.AsRaw64);
                case AdvancedCodeWords.ValueXOR24:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(24), m_nextPoint.PrevValue.AsRaw64);
                case AdvancedCodeWords.ValueXOR28:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(28), m_nextPoint.PrevValue.AsRaw64);
                case AdvancedCodeWords.ValueXOR32:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(32), m_nextPoint.PrevValue.AsRaw64);
                case AdvancedCodeWords.ValueXOR36:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(36), m_nextPoint.PrevValue.AsRaw64);
                case AdvancedCodeWords.ValueXOR40:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(40), m_nextPoint.PrevValue.AsRaw64);
                case AdvancedCodeWords.ValueXOR44:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(44), m_nextPoint.PrevValue.AsRaw64);
                case AdvancedCodeWords.ValueXOR48:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(48), m_nextPoint.PrevValue.AsRaw64);
                case AdvancedCodeWords.ValueXOR52:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(52), m_nextPoint.PrevValue.AsRaw64);
                case AdvancedCodeWords.ValueXOR56:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(56), m_nextPoint.PrevValue.AsRaw64);
                case AdvancedCodeWords.ValueXOR60:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(60), m_nextPoint.PrevValue.AsRaw64);
                case AdvancedCodeWords.ValueRaw:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(64), m_nextPoint.PrevValue.AsRaw64);
                default:
                    throw new ArgumentOutOfRangeException(nameof(code), code, null);
            }
        }

    }
}
