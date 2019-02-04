using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;

namespace Sttp.DataPointEncoding
{
    public class SimpleDecoder : DecoderBase
    {
        private long m_prevTimestamp;
        private SimpleMetadata m_prevPoint;
        private SimpleMetadata m_currentPoint;
        private List<SimpleMetadata> m_metadata = new List<SimpleMetadata>();
        private ByteReader m_reader;
        private int m_count;

        public SimpleDecoder(LookupMetadata lookup)
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
            SimpleSymbols code;
            if (m_reader.IsEmpty)
            {
                return false;
            }

            m_count++;
            if (m_count == 211)
                m_count = m_count;

            if (m_prevPoint != null)
            {
                code = ReadCode();
            }
            else
            {
                code = SimpleSymbols.DefineChannel;
            }

            if (code == SimpleSymbols.DefineChannel)
            {
                channelID = m_metadata.Count;
                CtpObject dataPointID = CtpValueEncodingNative.Load(m_reader);
                m_prevTimestamp = (long)m_reader.Read8BitSegments() ^ m_prevTimestamp;
                dataPoint.Time = new CtpTime(m_prevTimestamp);
                dataPoint.Quality = (long)m_reader.Read8BitSegments();
                dataPoint.Value = CtpValueEncodingNative.Load(m_reader);
                dataPoint.Metadata = LookupMetadata(dataPointID);
                if (m_prevPoint != null)
                {
                    m_prevPoint.NeighborChannelId = channelID;
                }
                m_prevPoint = new SimpleMetadata(channelID, dataPoint.Metadata);
                m_prevPoint.Assign(dataPoint);
                m_metadata.Add(m_prevPoint);
                return true;
            }

            if (m_prevPoint == null)
                throw new Exception("This should never occur");

            channelID = m_prevPoint.NeighborChannelId;
            if (code == SimpleSymbols.ChannelID)
            {
                channelID = (int)m_reader.ReadBits(CompareUInt32.RequiredBits((uint)m_metadata.Count));
                m_prevPoint.NeighborChannelId = channelID;
                code = ReadCode();
                if (code <= SimpleSymbols.ChannelID)
                    throw new Exception("Parsing Error");
            }

            m_currentPoint = m_metadata[channelID];
            dataPoint.Metadata = m_currentPoint.Metadata;

            if (code <= SimpleSymbols.Timestamp)
            {
                m_prevTimestamp = (long)m_reader.Read8BitSegments() ^ m_currentPoint.PrevTime;
                code = ReadCode();
                if (code <= SimpleSymbols.Timestamp)
                    throw new Exception("Parsing Error");
            }

            dataPoint.Time = new CtpTime(m_prevTimestamp);

            if (code <= SimpleSymbols.Quality)
            {
                m_currentPoint.PrevQuality = (long)m_reader.Read8BitSegments();

                code = ReadCode();
                if (code <= SimpleSymbols.Quality)
                    throw new Exception("Parsing Error");
            }

            dataPoint.Quality = m_currentPoint.PrevQuality;

            if (code <= SimpleSymbols.Type)
            {
                m_currentPoint.PrevValue = CtpObject.CreateDefault((CtpTypeCode)m_reader.ReadBits4());

                code = ReadCode();
                if (code <= SimpleSymbols.Type)
                    throw new Exception("Parsing Error");
            }

            if (code == SimpleSymbols.ValueDefault)
            {
                m_currentPoint.PrevValue = CtpObject.CreateDefault(m_currentPoint.PrevValue.ValueTypeCode);
            }
            else if (code == SimpleSymbols.ValueLast)
            {

            }
            else
            {
                ulong value;
                switch (m_currentPoint.PrevValue.ValueTypeCode)
                {
                    case CtpTypeCode.Int64:
                        value = ReadInt64();
                        m_currentPoint.PrevValue = (long)value;
                        break;
                    case CtpTypeCode.Single:
                        value = ReadInt32();
                        uint v2 = (uint)value;
                        m_currentPoint.PrevValue = *(float*)&v2;
                        break;
                    case CtpTypeCode.Double:
                        value = ReadInt64();
                        m_currentPoint.PrevValue = *(double*)&value;
                        break;
                    case CtpTypeCode.CtpTime:
                        value = ReadInt64();
                        m_currentPoint.PrevValue = new CtpTime((long)value);
                        break;
                    case CtpTypeCode.Boolean:
                        if (code != SimpleSymbols.ValueOther)
                            throw new Exception("Only true values are supported here.");
                        m_currentPoint.PrevValue = true;
                        break;
                    case CtpTypeCode.Guid:
                        if (code != SimpleSymbols.ValueOther)
                            throw new Exception("Expected a GUID.");
                        m_currentPoint.PrevValue = CtpValueEncodingNative.Load(m_reader);
                        break;
                    case CtpTypeCode.String:
                    case CtpTypeCode.CtpCommand:
                    case CtpTypeCode.CtpBuffer:
                        if (code != SimpleSymbols.ValueOther)
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

        private SimpleSymbols ReadCode()
        {
            if (m_reader.ReadBits1() == 0)
            {
                return (SimpleSymbols)m_reader.ReadBits3();
            }
            return SimpleSymbols.ValueOther;
        }

        private ulong ReadInt32()
        {
            return (m_reader.ReadBits((int)(m_reader.ReadBits3() + 1) * 4) ^ m_currentPoint.PrevValue.UnsafeRawInt64);
        }

        private ulong ReadInt64()
        {
            return (m_reader.ReadBits((int)(m_reader.ReadBits4() + 1) * 4) ^ m_currentPoint.PrevValue.UnsafeRawInt64);
        }
    }
}
