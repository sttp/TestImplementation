using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CTP;
using Sttp.Codec;
using Sttp.Collection;

namespace Sttp.DataPointEncoding
{
    public class AdvancedEncoder : EncoderBase
    {
        private MetadataChannelMapEncoder m_channelMap;

        const ulong Bits64 = 0xFFFFFFFFFFFFFFFFu;
        const ulong Bits60 = 0xFFFFFFFFFFFFFFFu;
        const ulong Bits56 = 0xFFFFFFFFFFFFFFu;
        const ulong Bits52 = 0xFFFFFFFFFFFFFu;
        const ulong Bits48 = 0xFFFFFFFFFFFFu;
        const ulong Bits44 = 0xFFFFFFFFFFFu;
        const ulong Bits40 = 0xFFFFFFFFFFu;
        const ulong Bits36 = 0xFFFFFFFFFu;
        const uint Bits32 = 0xFFFFFFFFu;
        const uint Bits30 = 0x3FFFFFFFu;
        const uint Bits28 = 0xFFFFFFFu;
        const uint Bits26 = 0x3FFFFFFu;
        const uint Bits24 = 0xFFFFFFu;
        const uint Bits22 = 0x3FFFFFu;
        const uint Bits20 = 0xFFFFFu;
        const uint Bits18 = 0x3FFFFu;
        const uint Bits16 = 0xFFFFu;
        const uint Bits14 = 0x3FFFu;
        const uint Bits12 = 0xFFFu;
        const uint Bits10 = 0x3FFu;
        const uint Bits8 = 0xFFu;
        const uint Bits6 = 0x3Fu;
        const uint Bits4 = 0xFu;
        const uint Bits2 = 0x3u;
        const uint Bits0 = 0x0u;

        public ByteWriter Writer;

        private long m_prevTimestamp;
        private AdvancedPointMetadata m_prevPoint;
        private AdvancedPointMetadata m_nextPoint;
        private AdvancedWordEncoding m_encoder;

        private AdvancedTimestampEncoding m_timeEncoding;

        private IndexedArray<AdvancedPointMetadata> m_points;

        public AdvancedEncoder()
        {
            m_channelMap = new MetadataChannelMapEncoder();
            Writer = new ByteWriter();
            Reset(true);
        }

        public override int Length => Writer.Length;

        public override void Clear(bool clearMapping)
        {
            Reset(clearMapping);
            if (clearMapping)
            {
                m_channelMap.Clear();
            }
        }

        public override void AddDataPoint(SttpDataPoint point)
        {
            int channelID = m_channelMap.GetChannelID(point.Metadata, out var isNew);

            if (isNew)
            {
                m_nextPoint = new AdvancedPointMetadata(Writer, null, point);
                m_points[channelID] = m_nextPoint;
                m_encoder = m_prevPoint.NextValueEncoding;
                m_encoder.WriteCode(AdvancedCodeWords.NewPoint);
                CtpValueEncodingNative.Save(Writer, point.Metadata.DataPointID);
                Writer.Write8BitSegments(CompareUInt64.Compare((ulong)point.Time.Ticks, (ulong)m_prevTimestamp));
                m_prevTimestamp = point.Time.Ticks;
                Writer.Write8BitSegments((ulong)point.Quality);
                CtpValueEncodingNative.Save(Writer, point.Value);
                m_prevPoint.NeighborChannelId = channelID;
            }
            else
            {
                m_nextPoint = m_points[channelID];
                m_encoder = m_prevPoint.NextValueEncoding;

                if (m_prevPoint.NeighborChannelId != channelID)
                {
                    m_encoder.WriteCode(AdvancedCodeWords.PointID);
                    Writer.Write8BitSegments((uint)channelID);
                    m_prevPoint.NeighborChannelId = channelID;
                }
                if (m_prevTimestamp != point.Time.Ticks)
                {
                    long timestamp = point.Time.Ticks;
                    m_timeEncoding.WriteCode(m_encoder, timestamp - m_prevTimestamp);
                    m_prevTimestamp = timestamp;
                }
                if (m_nextPoint.PrevQuality != point.Quality)
                {
                    m_encoder.WriteCode(AdvancedCodeWords.Quality);
                    Writer.Write8BitSegments((ulong)point.Quality);
                    m_nextPoint.PrevQuality = point.Quality;
                }
                if (point.Value.ValueTypeCode != m_nextPoint.PrevTypeCode)
                {
                    m_encoder.WriteCode(AdvancedCodeWords.Type);
                    Writer.WriteBits4((uint)point.Value.ValueTypeCode & 15);
                    m_nextPoint.PrevTypeCode = point.Value.ValueTypeCode;
                    m_nextPoint.PrevValue = CtpObject.Null;
                }

                if (point.Value.IsDefault)
                {
                    m_encoder.WriteCode(AdvancedCodeWords.ValueDefault);
                    m_nextPoint.PrevValue = point.Value;
                }
                else if (m_nextPoint.PrevValue == point.Value)
                {
                    m_encoder.WriteCode(AdvancedCodeWords.ValuePrev);
                }
                else
                {
                    switch (point.Value.ValueTypeCode)
                    {
                        case CtpTypeCode.Int64:
                        case CtpTypeCode.Single:
                        case CtpTypeCode.Double:
                        case CtpTypeCode.CtpTime:
                            Write64(point.Value.AsRaw64);
                            break;
                        case CtpTypeCode.Boolean:
                            //Cannot be false, that would be Default.
                            m_encoder.WriteCode(AdvancedCodeWords.ValueTrue);
                            break;
                        case CtpTypeCode.Guid:
                        case CtpTypeCode.String:
                        case CtpTypeCode.CtpCommand:
                        case CtpTypeCode.CtpBuffer:
                            m_encoder.WriteCode(AdvancedCodeWords.ValueRaw);
                            CtpValueEncodingNative.Save(Writer, point.Value);
                            break;
                        case CtpTypeCode.Null:
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    m_nextPoint.PrevValue = point.Value;
                }
            }



            m_prevPoint = m_nextPoint;
        }

        public override byte[] ToArray()
        {
            return Writer.ToArray();
        }

        /// <summary>
        /// Resets the TSSC Encoder to the initial state. 
        /// </summary>
        /// <remarks>
        /// TSSC is a stateful encoder that requires a state
        /// of the previous data to be maintained. Therefore, if 
        /// the state ever becomes corrupt (out of order, dropped, corrupted, or duplicated)
        /// the state must be reset on both ends.
        /// </remarks>
        public void Reset(bool clearMapping)
        {
            Writer.Clear();
            if (clearMapping)
            {
                m_points = new IndexedArray<AdvancedPointMetadata>();
                m_prevPoint = new AdvancedPointMetadata(Writer, null, null);
                m_timeEncoding = new AdvancedTimestampEncoding(Writer, null);
                m_prevTimestamp = 0;
            }
        }

        private void Write64(ulong valueRaw)
        {
            ulong bitsChanged = CompareUInt64.Compare(valueRaw, m_nextPoint.PrevValue.AsRaw64);
            if (bitsChanged <= Bits4)
            {
                m_encoder.WriteCode(AdvancedCodeWords.ValueXOR4);
                Writer.WriteBits(4, bitsChanged);
            }
            else if (bitsChanged <= Bits8)
            {
                m_encoder.WriteCode(AdvancedCodeWords.ValueXOR8);
                Writer.WriteBits(8, bitsChanged);
            }
            else if (bitsChanged <= Bits10)
            {
                m_encoder.WriteCode(AdvancedCodeWords.ValueXOR10);
                Writer.WriteBits(10, bitsChanged);
            }
            else if (bitsChanged <= Bits12)
            {
                m_encoder.WriteCode(AdvancedCodeWords.ValueXOR12);
                Writer.WriteBits(12, bitsChanged);
            }
            else if (bitsChanged <= Bits14)
            {
                m_encoder.WriteCode(AdvancedCodeWords.ValueXOR14);
                Writer.WriteBits(14, bitsChanged);
            }
            else if (bitsChanged <= Bits16)
            {
                m_encoder.WriteCode(AdvancedCodeWords.ValueXOR16);
                Writer.WriteBits(16, bitsChanged);
            }
            else if (bitsChanged <= Bits18)
            {
                m_encoder.WriteCode(AdvancedCodeWords.ValueXOR18);
                Writer.WriteBits(18, bitsChanged);
            }
            else if (bitsChanged <= Bits20)
            {
                m_encoder.WriteCode(AdvancedCodeWords.ValueXOR20);
                Writer.WriteBits(20, bitsChanged);
            }
            else if (bitsChanged <= Bits24)
            {
                m_encoder.WriteCode(AdvancedCodeWords.ValueXOR24);
                Writer.WriteBits(24, bitsChanged);
            }
            else if (bitsChanged <= Bits28)
            {
                m_encoder.WriteCode(AdvancedCodeWords.ValueXOR28);
                Writer.WriteBits(28, bitsChanged);
            }
            else if (bitsChanged <= Bits32)
            {
                m_encoder.WriteCode(AdvancedCodeWords.ValueXOR32);
                Writer.WriteBits(32, bitsChanged);
            }
            else if (bitsChanged <= Bits36)
            {
                m_encoder.WriteCode(AdvancedCodeWords.ValueXOR36);
                Writer.WriteBits(36, bitsChanged);
            }
            else if (bitsChanged <= Bits40)
            {
                m_encoder.WriteCode(AdvancedCodeWords.ValueXOR40);
                Writer.WriteBits(40, bitsChanged);
            }
            else if (bitsChanged <= Bits44)
            {
                m_encoder.WriteCode(AdvancedCodeWords.ValueXOR44);
                Writer.WriteBits(44, bitsChanged);
            }
            else if (bitsChanged <= Bits48)
            {
                m_encoder.WriteCode(AdvancedCodeWords.ValueXOR48);
                Writer.WriteBits(48, bitsChanged);
            }
            else if (bitsChanged <= Bits52)
            {
                m_encoder.WriteCode(AdvancedCodeWords.ValueXOR52);
                Writer.WriteBits(52, bitsChanged);
            }
            else if (bitsChanged <= Bits56)
            {
                m_encoder.WriteCode(AdvancedCodeWords.ValueXOR56);
                Writer.WriteBits(56, bitsChanged);
            }
            else if (bitsChanged <= Bits60)
            {
                m_encoder.WriteCode(AdvancedCodeWords.ValueXOR60);
                Writer.WriteBits(60, bitsChanged);
            }
            else
            {
                m_encoder.WriteCode(AdvancedCodeWords.ValueRaw);
                Writer.WriteBits(64, bitsChanged);
            }
        }

    }
}
