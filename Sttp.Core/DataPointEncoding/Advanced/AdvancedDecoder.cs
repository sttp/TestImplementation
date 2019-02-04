using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;

namespace Sttp.DataPointEncoding
{
    public class AdvancedDecoder : DecoderBase
    {
        private long m_prevTimestamp;
        private AdvancedMetadata m_prevPoint;
        private AdvancedMetadata m_currentPoint;
        private List<AdvancedMetadata> m_metadata = new List<AdvancedMetadata>();
        private ByteReader m_reader;
        private int m_count;

        public AdvancedDecoder(LookupMetadata lookup)
            : base(lookup)
        {
            m_reader = new ByteReader();
        }

        public override void Load(byte[] data)
        {
            m_reader.SetBuffer(data, 0, data.Length);
        }

        public override unsafe bool Read(SttpDataPoint dataPoint)
        {
            int channelID;
            AdvancedSymbols code;
            if (m_reader.IsEmpty)
            {
                return false;
            }

            m_count++;
            if (m_count == 211)
                m_count = m_count;

            if (m_prevPoint != null)
            {
                code = m_prevPoint.NextSymbolEncoding.ReadCode();
            }
            else
            {
                code = AdvancedSymbols.DefineChannel;
            }

            if (code == AdvancedSymbols.DefineChannel)
            {
                channelID = m_metadata.Count;
                CtpObject dataPointID = CtpValueEncodingNative.Load(m_reader);
                m_prevTimestamp = (long)CompareUInt64.UnCompare(m_reader.Read8BitSegments(), (ulong)m_prevTimestamp);
                dataPoint.Time = new CtpTime(m_prevTimestamp);
                dataPoint.Quality = (long)m_reader.Read8BitSegments();
                dataPoint.Value = CtpValueEncodingNative.Load(m_reader);
                dataPoint.Metadata = LookupMetadata(dataPointID);
                if (m_prevPoint != null)
                {
                    m_prevPoint.NeighborChannelId = channelID;
                }
                m_prevPoint = new AdvancedMetadata(channelID, dataPoint.Metadata, null, m_reader);
                m_prevPoint.Assign(dataPoint);
                m_metadata.Add(m_prevPoint);
                return true;
            }

            if (m_prevPoint == null)
                throw new Exception("This should never occur");

            channelID = m_prevPoint.NeighborChannelId;
            if (code == AdvancedSymbols.ChannelID)
            {
                channelID = (int)m_reader.ReadBits(CompareUInt32.RequiredBits((uint)m_metadata.Count));
                m_prevPoint.NeighborChannelId = channelID;
                code = m_prevPoint.NextSymbolEncoding.ReadCode();
                if (code <= AdvancedSymbols.ChannelID)
                    throw new Exception("Parsing Error");
            }

            m_currentPoint = m_metadata[channelID];
            dataPoint.Metadata = m_currentPoint.Metadata;

            if (code <= AdvancedSymbols.TimestampElse)
            {
                switch (code)
                {
                    case AdvancedSymbols.TimestampDelta1:
                        m_prevTimestamp = m_currentPoint.TimeDelta1 + m_currentPoint.PrevTime;
                        break;
                    case AdvancedSymbols.TimestampDelta2:
                        m_prevTimestamp = m_currentPoint.TimeDelta2 + m_currentPoint.PrevTime;
                        break;
                    case AdvancedSymbols.TimestampDelta3:
                        m_prevTimestamp = m_currentPoint.TimeDelta3 + m_currentPoint.PrevTime;
                        var delta3 = m_currentPoint.TimeDelta3;
                        m_currentPoint.TimeDelta3 = m_currentPoint.TimeDelta2;
                        m_currentPoint.TimeDelta2 = m_currentPoint.TimeDelta1;
                        m_currentPoint.TimeDelta1 = delta3;
                        break;
                    case AdvancedSymbols.TimestampElse:
                        m_prevTimestamp = (long)CompareUInt64.UnCompare(m_reader.Read8BitSegments(), (ulong)m_currentPoint.PrevTime);
                        m_currentPoint.TimeDelta3 = m_currentPoint.TimeDelta2;
                        m_currentPoint.TimeDelta2 = m_currentPoint.TimeDelta1;
                        m_currentPoint.TimeDelta1 = m_prevTimestamp - m_currentPoint.PrevTime;
                        break;
                }
                code = m_prevPoint.NextSymbolEncoding.ReadCode();
                if (code <= AdvancedSymbols.TimestampElse)
                    throw new Exception("Parsing Error");
            }

            dataPoint.Time = new CtpTime(m_prevTimestamp);

            if (code <= AdvancedSymbols.Quality)
            {
                m_currentPoint.PrevQuality = (long)m_reader.Read8BitSegments();

                code = m_prevPoint.NextSymbolEncoding.ReadCode();
                if (code <= AdvancedSymbols.Quality)
                    throw new Exception("Parsing Error");
            }

            dataPoint.Quality = m_currentPoint.PrevQuality;

            if (code <= AdvancedSymbols.Type)
            {
                m_currentPoint.PrevValue = CtpObject.CreateDefault((CtpTypeCode)m_reader.ReadBits4());

                code = m_prevPoint.NextSymbolEncoding.ReadCode();
                if (code <= AdvancedSymbols.Type)
                    throw new Exception("Parsing Error");
            }

            if (code == AdvancedSymbols.ValueDefault)
            {
                m_currentPoint.PrevValue = CtpObject.CreateDefault(m_currentPoint.PrevValue.ValueTypeCode);
            }
            else if (code == AdvancedSymbols.ValueLast)
            {

            }
            else
            {
                ulong value;
                switch (m_currentPoint.PrevValue.ValueTypeCode)
                {
                    case CtpTypeCode.Int64:
                        value = ReadInt64(code);
                        m_currentPoint.PrevValue = (long)value;
                        break;
                    case CtpTypeCode.Single:
                        value = ReadInt64(code);
                        uint v2 = (uint)value;
                        m_currentPoint.PrevValue = *(float*)&v2;
                        break;
                    case CtpTypeCode.Double:
                        value = ReadInt64(code);
                        m_currentPoint.PrevValue = *(double*)&value;
                        break;
                    case CtpTypeCode.CtpTime:
                        value = ReadInt64(code);
                        m_currentPoint.PrevValue = new CtpTime((long)value);
                        break;
                    case CtpTypeCode.Boolean:
                        if (code != AdvancedSymbols.ValueTrue)
                            throw new Exception("Only true values are supported here.");
                        m_currentPoint.PrevValue = true;
                        break;
                    case CtpTypeCode.Guid:
                        if (code != AdvancedSymbols.ValueBits128)
                            throw new Exception("Expected a GUID.");
                        m_currentPoint.PrevValue = CtpValueEncodingNative.Load(m_reader);
                        break;
                    case CtpTypeCode.String:
                    case CtpTypeCode.CtpCommand:
                    case CtpTypeCode.CtpBuffer:
                        if (code != AdvancedSymbols.ValueBuffer)
                            throw new Exception("Only raw types are supported here.");
                        m_currentPoint.PrevValue = CtpValueEncodingNative.Load(m_reader);
                        break;
                    case CtpTypeCode.Null:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            dataPoint.Value = m_currentPoint.PrevValue;
            m_currentPoint.Assign(dataPoint);
            m_prevPoint = m_currentPoint;
            return true;
        }

        private ulong ReadInt64(AdvancedSymbols code)
        {
            switch (code)
            {
                case AdvancedSymbols.ValueBits4:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(4), m_currentPoint.PrevValue.UnsafeRawInt64);
                case AdvancedSymbols.ValueBits8:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(8), m_currentPoint.PrevValue.UnsafeRawInt64);
                case AdvancedSymbols.ValueBits12:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(12), m_currentPoint.PrevValue.UnsafeRawInt64);
                case AdvancedSymbols.ValueBits16:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(16), m_currentPoint.PrevValue.UnsafeRawInt64);
                case AdvancedSymbols.ValueBits20:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(20), m_currentPoint.PrevValue.UnsafeRawInt64);
                case AdvancedSymbols.ValueBits24:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(24), m_currentPoint.PrevValue.UnsafeRawInt64);
                case AdvancedSymbols.ValueBits28:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(28), m_currentPoint.PrevValue.UnsafeRawInt64);
                case AdvancedSymbols.ValueBits32:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(32), m_currentPoint.PrevValue.UnsafeRawInt64);
                case AdvancedSymbols.ValueBits36:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(36), m_currentPoint.PrevValue.UnsafeRawInt64);
                case AdvancedSymbols.ValueBits40:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(40), m_currentPoint.PrevValue.UnsafeRawInt64);
                case AdvancedSymbols.ValueBits44:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(44), m_currentPoint.PrevValue.UnsafeRawInt64);
                case AdvancedSymbols.ValueBits48:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(48), m_currentPoint.PrevValue.UnsafeRawInt64);
                case AdvancedSymbols.ValueBits52:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(52), m_currentPoint.PrevValue.UnsafeRawInt64);
                case AdvancedSymbols.ValueBits56:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(56), m_currentPoint.PrevValue.UnsafeRawInt64);
                case AdvancedSymbols.ValueBits60:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(60), m_currentPoint.PrevValue.UnsafeRawInt64);
                case AdvancedSymbols.ValueBits64:
                    return CompareUInt64.UnCompare(m_reader.ReadBits(64), m_currentPoint.PrevValue.UnsafeRawInt64);
                default:
                    throw new ArgumentOutOfRangeException(nameof(code), code, null);
            }
        }
    }
}
