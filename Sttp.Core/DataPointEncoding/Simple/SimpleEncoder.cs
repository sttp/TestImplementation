using System;
using System.Collections.Generic;
using CTP;

namespace Sttp.DataPointEncoding
{
    public sealed class SimpleEncoder : EncoderBase
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
        private SimpleMetadata m_prevPoint;
        private SimpleMetadata m_currentPoint;
        private Dictionary<int, int> m_runtimeIDToChannelIDMapping = new Dictionary<int, int>();
        private Dictionary<CtpObject, int> m_pointIDToChannelIDMapping = new Dictionary<CtpObject, int>();
        private List<SimpleMetadata> m_metadata = new List<SimpleMetadata>();

        private int m_count;

        public SimpleEncoder()
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
                    m_writer.WriteBits3((byte)SimpleSymbols.DefineChannel);
                    m_prevPoint.NeighborChannelId = m_currentPoint.ChannelID;
                }
                CtpValueEncodingNative.Save(m_writer, point.Metadata.DataPointID);
                m_writer.Write8BitSegments((ulong)point.Time.Ticks ^ (ulong)m_prevTimestamp);
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
                m_writer.WriteBits3((byte)SimpleSymbols.ChannelID);
                m_writer.WriteBits(CompareUInt32.RequiredBits((uint)m_metadata.Count), (uint)m_currentPoint.ChannelID);
                m_prevPoint.NeighborChannelId = m_currentPoint.ChannelID;
            }

            if (m_prevTimestamp != point.Time.Ticks)
            {
                m_writer.WriteBits3((byte)SimpleSymbols.Timestamp);
                m_writer.Write8BitSegments((ulong)point.Time.Ticks ^ (ulong)m_currentPoint.PrevTime);
                m_prevTimestamp = point.Time.Ticks;
            }

            if (m_currentPoint.PrevQuality != point.Quality)
            {
                m_writer.WriteBits3((byte)SimpleSymbols.Quality);
                m_writer.Write8BitSegments((ulong)point.Quality);
                m_currentPoint.PrevQuality = point.Quality;
            }

            if (point.Value.ValueTypeCode != m_currentPoint.PrevValue.ValueTypeCode)
            {
                m_writer.WriteBits3((byte)SimpleSymbols.Type);
                m_writer.WriteBits4((uint)point.Value.ValueTypeCode & 15);
                m_currentPoint.PrevValue = CtpObject.CreateDefault(point.Value.ValueTypeCode);
            }

            if (point.Value.IsDefault)
            {
                m_writer.WriteBits3((byte)SimpleSymbols.ValueDefault);
                m_currentPoint.PrevValue = point.Value;
            }
            else if (m_currentPoint.PrevValue == point.Value)
            {
                m_writer.WriteBits3((byte)SimpleSymbols.ValueLast);
            }
            else
            {
                m_writer.WriteBits3((byte)SimpleSymbols.ValueOther);

                switch (point.Value.ValueTypeCode)
                {
                    case CtpTypeCode.Single:
                        Write32((uint)(point.Value.AsRaw64 ^ m_currentPoint.PrevValue.AsRaw64));
                        break;
                    case CtpTypeCode.Int64:
                    case CtpTypeCode.Double:
                    case CtpTypeCode.CtpTime:
                        Write64(point.Value.AsRaw64 ^ m_currentPoint.PrevValue.AsRaw64);
                        break;
                    case CtpTypeCode.Boolean:
                        //Write nothing, this can only be true;
                        break;
                    case CtpTypeCode.Guid:
                        CtpValueEncodingNative.Save(m_writer, point.Value);
                        break;
                    case CtpTypeCode.String:
                    case CtpTypeCode.CtpCommand:
                    case CtpTypeCode.CtpBuffer:
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

        private void Write32(uint bitsChanged)
        {
            int bits = CompareUInt32.RequiredBits(bitsChanged);
            if (bits == 0)
                throw new NotSupportedException();
            bits = ((bits + 3) >> 2) - 1; //Round up to the nearest check;
            m_writer.WriteBits3((uint)bits);
            m_writer.WriteBits((bits + 1) * 4, bitsChanged);
        }

        private void Write64(ulong bitsChanged)
        {
            int bits = CompareUInt64.RequiredBits(bitsChanged);
            if (bits == 0)
                throw new NotSupportedException();
            bits = ((bits + 3) >> 2) - 1; //Round up to the nearest check;
            m_writer.WriteBits4((uint)bits);
            m_writer.WriteBits((bits + 1) * 4, bitsChanged);
        }

        private SimpleMetadata FindMetadata(SttpDataPointMetadata metadata, out bool isNew)
        {
            isNew = false;
            int channelID;

            //Since most of the time, sequencing will be preserved, this check avoids a dictionary lookup.
            if (m_prevPoint != null)
            {
                channelID = m_prevPoint.NeighborChannelId;
                if (m_metadata[channelID].Metadata.DataPointID == metadata.DataPointID)
                    return m_metadata[channelID];
            }

            if (metadata.RuntimeID.HasValue)
            {
                if (!m_runtimeIDToChannelIDMapping.TryGetValue(metadata.RuntimeID.Value, out channelID))
                {
                    channelID = m_metadata.Count;
                    var point = new SimpleMetadata(channelID, metadata);
                    m_metadata.Add(point);
                    m_runtimeIDToChannelIDMapping.Add(metadata.RuntimeID.Value, channelID);
                    isNew = true;
                }
                return m_metadata[channelID];
            }

            if (!m_pointIDToChannelIDMapping.TryGetValue(metadata.DataPointID, out channelID))
            {
                channelID = m_metadata.Count;
                var point = new SimpleMetadata(channelID, metadata);
                m_metadata.Add(point);
                m_pointIDToChannelIDMapping.Add(metadata.DataPointID, channelID);
                isNew = true;
            }
            return m_metadata[channelID];
        }






    }
}
