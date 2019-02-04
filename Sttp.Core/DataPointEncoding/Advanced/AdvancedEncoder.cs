using System;
using System.Collections.Generic;
using CTP;

namespace Sttp.DataPointEncoding
{
    public sealed class AdvancedEncoder : EncoderBase
    {
        const ulong Bits64 = 0xFFFFFFFFFFFFFFFFu;
        const ulong Bits60 = 0xFFFFFFFFFFFFFFFu;
        const ulong Bits56 = 0xFFFFFFFFFFFFFFu;
        const ulong Bits52 = 0xFFFFFFFFFFFFFu;
        const ulong Bits48 = 0xFFFFFFFFFFFFu;
        const ulong Bits44 = 0xFFFFFFFFFFFu;
        const ulong Bits40 = 0xFFFFFFFFFFu;
        const ulong Bits36 = 0xFFFFFFFFFu;
        const uint Bits32 = 0xFFFFFFFFu;
        const uint Bits28 = 0xFFFFFFFu;
        const uint Bits24 = 0xFFFFFFu;
        const uint Bits20 = 0xFFFFFu;
        const uint Bits16 = 0xFFFFu;
        const uint Bits12 = 0xFFFu;
        const uint Bits8 = 0xFFu;
        const uint Bits4 = 0xFu;

        private ByteWriter m_writer;
        /// <summary>
        /// </summary>
        private long m_prevTimestamp;
        private AdvancedMetadata m_prevPoint;
        private AdvancedMetadata m_currentPoint;
        private Dictionary<int, int> m_runtimeIDToChannelIDMapping = new Dictionary<int, int>();
        private Dictionary<CtpObject, int> m_pointIDToChannelIDMapping = new Dictionary<CtpObject, int>();
        private List<AdvancedMetadata> m_metadata = new List<AdvancedMetadata>();

        private int m_count;

        public AdvancedEncoder()
        {
            m_writer = new ByteWriter();
        }

        public override int Length => m_writer.Length;

        public override void Clear()
        {
            m_writer.Clear();
        }

        public override void AddDataPoint(SttpDataPoint point)
        {
            m_count++;
            if (m_count == 211)
                m_count = m_count;

            m_currentPoint = FindMetadata(point.Metadata, out var isNew);

            if (isNew)
            {
                m_currentPoint.Assign(point);
                if (m_prevPoint != null) //If there is no previous point, the first symbol is unspecified and assumed to be DefineChannel.
                {
                    m_prevPoint.NextSymbolEncoding.WriteCode(AdvancedSymbols.DefineChannel);
                    m_prevPoint.NeighborChannelId = m_currentPoint.ChannelID;
                }
                CtpValueEncodingNative.Save(m_writer, point.Metadata.DataPointID);
                m_writer.Write8BitSegments(CompareUInt64.Compare((ulong)point.Time.Ticks, (ulong)m_prevTimestamp));
                m_writer.Write8BitSegments((ulong)point.Quality);
                CtpValueEncodingNative.Save(m_writer, point.Value);
                m_prevTimestamp = point.Time.Ticks;
                m_prevPoint = m_currentPoint;
                return;
            }

            if (m_prevPoint == null)
                throw new Exception("This should never occur");

            if (m_prevPoint.NeighborChannelId != m_currentPoint.ChannelID)
            {
                m_prevPoint.NextSymbolEncoding.WriteCode(AdvancedSymbols.ChannelID);
                m_writer.WriteBits(CompareUInt32.RequiredBits((uint)m_metadata.Count), (uint)m_currentPoint.ChannelID);
                m_prevPoint.NeighborChannelId = m_currentPoint.ChannelID;
            }

            if (m_prevTimestamp != point.Time.Ticks)
            {
                long timestamp = point.Time.Ticks;
                var delta = timestamp - m_currentPoint.PrevTime;
                if (m_currentPoint.TimeDelta1 == delta)
                {
                    m_prevPoint.NextSymbolEncoding.WriteCode(AdvancedSymbols.TimestampDelta1);
                }
                else if (m_currentPoint.TimeDelta2 == delta)
                {
                    m_prevPoint.NextSymbolEncoding.WriteCode(AdvancedSymbols.TimestampDelta2);
                }
                else if (m_currentPoint.TimeDelta3 == delta)
                {
                    m_prevPoint.NextSymbolEncoding.WriteCode(AdvancedSymbols.TimestampDelta3);
                    m_currentPoint.TimeDelta3 = m_currentPoint.TimeDelta2;
                    m_currentPoint.TimeDelta2 = m_currentPoint.TimeDelta1;
                    m_currentPoint.TimeDelta1 = delta;
                }
                else
                {
                    m_prevPoint.NextSymbolEncoding.WriteCode(AdvancedSymbols.TimestampElse);
                    m_currentPoint.TimeDelta3 = m_currentPoint.TimeDelta2;
                    m_currentPoint.TimeDelta2 = m_currentPoint.TimeDelta1;
                    m_currentPoint.TimeDelta1 = delta;
                    m_writer.Write8BitSegments(CompareUInt64.Compare((ulong)point.Time.Ticks, (ulong)m_currentPoint.PrevTime));
                }
                m_prevTimestamp = timestamp;
            }

            if (m_currentPoint.PrevQuality != point.Quality)
            {
                m_prevPoint.NextSymbolEncoding.WriteCode(AdvancedSymbols.Quality);
                m_writer.Write8BitSegments((ulong)point.Quality);
                m_currentPoint.PrevQuality = point.Quality;
            }

            if (point.Value.ValueTypeCode != m_currentPoint.PrevValue.ValueTypeCode)
            {
                m_prevPoint.NextSymbolEncoding.WriteCode(AdvancedSymbols.Type);
                m_writer.WriteBits4((uint)point.Value.ValueTypeCode & 15);
                m_currentPoint.PrevValue = CtpObject.CreateDefault(point.Value.ValueTypeCode);
            }

            if (point.Value.IsDefault)
            {
                m_prevPoint.NextSymbolEncoding.WriteCode(AdvancedSymbols.ValueDefault);
                m_currentPoint.PrevValue = point.Value;
            }
            else if (m_currentPoint.PrevValue.Equals(point.Value))
            {
                m_prevPoint.NextSymbolEncoding.WriteCode(AdvancedSymbols.ValueLast);
            }
            else
            {
                switch (point.Value.ValueTypeCode)
                {
                    case CtpTypeCode.Single:
                    case CtpTypeCode.Int64:
                    case CtpTypeCode.Double:
                    case CtpTypeCode.CtpTime:
                        ulong bitsChanged = CompareUInt64.Compare(point.Value.UnsafeRawInt64, m_currentPoint.PrevValue.UnsafeRawInt64);
                        Write64(bitsChanged);
                        break;
                    case CtpTypeCode.Boolean:
                        //Cannot be false, that would be Default.
                        m_prevPoint.NextSymbolEncoding.WriteCode(AdvancedSymbols.ValueTrue);
                        break;
                    case CtpTypeCode.Guid:
                        m_prevPoint.NextSymbolEncoding.WriteCode(AdvancedSymbols.ValueBits128);
                        CtpValueEncodingNative.Save(m_writer, point.Value);
                        break;
                    case CtpTypeCode.String:
                    case CtpTypeCode.CtpCommand:
                    case CtpTypeCode.CtpBuffer:
                        m_prevPoint.NextSymbolEncoding.WriteCode(AdvancedSymbols.ValueBuffer);
                        CtpValueEncodingNative.Save(m_writer, point.Value);
                        break;
                    case CtpTypeCode.Null:
                        //Will always be caught by Default
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                m_currentPoint.PrevValue = point.Value;
            }

            m_prevPoint = m_currentPoint;
            m_currentPoint.Assign(point);
        }

        public override byte[] ToArray()
        {
            return m_writer.ToArray();
        }

        private void Write64(ulong bitsChanged)
        {
            if (bitsChanged <= Bits4)
            {
                m_prevPoint.NextSymbolEncoding.WriteCode(AdvancedSymbols.ValueBits4);
                m_writer.WriteBits(4, bitsChanged);
            }
            else if (bitsChanged <= Bits8)
            {
                m_prevPoint.NextSymbolEncoding.WriteCode(AdvancedSymbols.ValueBits8);
                m_writer.WriteBits(8, bitsChanged);
            }
            else if (bitsChanged <= Bits12)
            {
                m_prevPoint.NextSymbolEncoding.WriteCode(AdvancedSymbols.ValueBits12);
                m_writer.WriteBits(12, bitsChanged);
            }
            else if (bitsChanged <= Bits16)
            {
                m_prevPoint.NextSymbolEncoding.WriteCode(AdvancedSymbols.ValueBits16);
                m_writer.WriteBits(16, bitsChanged);
            }
            else if (bitsChanged <= Bits20)
            {
                m_prevPoint.NextSymbolEncoding.WriteCode(AdvancedSymbols.ValueBits20);
                m_writer.WriteBits(20, bitsChanged);
            }
            else if (bitsChanged <= Bits24)
            {
                m_prevPoint.NextSymbolEncoding.WriteCode(AdvancedSymbols.ValueBits24);
                m_writer.WriteBits(24, bitsChanged);
            }
            else if (bitsChanged <= Bits28)
            {
                m_prevPoint.NextSymbolEncoding.WriteCode(AdvancedSymbols.ValueBits28);
                m_writer.WriteBits(28, bitsChanged);
            }
            else if (bitsChanged <= Bits32)
            {
                m_prevPoint.NextSymbolEncoding.WriteCode(AdvancedSymbols.ValueBits32);
                m_writer.WriteBits(32, bitsChanged);
            }
            else if (bitsChanged <= Bits36)
            {
                m_prevPoint.NextSymbolEncoding.WriteCode(AdvancedSymbols.ValueBits36);
                m_writer.WriteBits(36, bitsChanged);
            }
            else if (bitsChanged <= Bits40)
            {
                m_prevPoint.NextSymbolEncoding.WriteCode(AdvancedSymbols.ValueBits40);
                m_writer.WriteBits(40, bitsChanged);
            }
            else if (bitsChanged <= Bits44)
            {
                m_prevPoint.NextSymbolEncoding.WriteCode(AdvancedSymbols.ValueBits44);
                m_writer.WriteBits(44, bitsChanged);
            }
            else if (bitsChanged <= Bits48)
            {
                m_prevPoint.NextSymbolEncoding.WriteCode(AdvancedSymbols.ValueBits48);
                m_writer.WriteBits(48, bitsChanged);
            }
            else if (bitsChanged <= Bits52)
            {
                m_prevPoint.NextSymbolEncoding.WriteCode(AdvancedSymbols.ValueBits52);
                m_writer.WriteBits(52, bitsChanged);
            }
            else if (bitsChanged <= Bits56)
            {
                m_prevPoint.NextSymbolEncoding.WriteCode(AdvancedSymbols.ValueBits56);
                m_writer.WriteBits(56, bitsChanged);
            }
            else if (bitsChanged <= Bits60)
            {
                m_prevPoint.NextSymbolEncoding.WriteCode(AdvancedSymbols.ValueBits60);
                m_writer.WriteBits(60, bitsChanged);
            }
            else
            {
                m_prevPoint.NextSymbolEncoding.WriteCode(AdvancedSymbols.ValueBits64);
                m_writer.WriteBits(64, bitsChanged);
            }
        }

        private AdvancedMetadata FindMetadata(SttpDataPointMetadata metadata, out bool isNew)
        {
            isNew = false;
            int channelID;

            //Since most of the time, sequencing will be preserved, this check avoids a dictionary lookup.
            if (m_prevPoint != null)
            {
                channelID = m_prevPoint.NeighborChannelId;
                if (m_metadata[channelID].Metadata.DataPointID.Equals(metadata.DataPointID))
                    return m_metadata[channelID];
            }

            if (metadata.RuntimeID.HasValue)
            {
                if (!m_runtimeIDToChannelIDMapping.TryGetValue(metadata.RuntimeID.Value, out channelID))
                {
                    channelID = m_metadata.Count;
                    var point = new AdvancedMetadata(channelID, metadata, m_writer, null);
                    m_metadata.Add(point);
                    m_runtimeIDToChannelIDMapping.Add(metadata.RuntimeID.Value, channelID);
                    isNew = true;
                }
                return m_metadata[channelID];
            }

            if (!m_pointIDToChannelIDMapping.TryGetValue(metadata.DataPointID, out channelID))
            {
                channelID = m_metadata.Count;
                var point = new AdvancedMetadata(channelID, metadata, m_writer, null);
                m_metadata.Add(point);
                m_pointIDToChannelIDMapping.Add(metadata.DataPointID, channelID);
                isNew = true;
            }
            return m_metadata[channelID];
        }






    }
}
