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
    public class AdaptiveEncoder : EncoderBase
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
        const uint Bits31 = 0x7FFFFFFFu;
        const uint Bits30 = 0x3FFFFFFFu;
        const uint Bits29 = 0x1FFFFFFFu;
        const uint Bits28 = 0xFFFFFFFu;
        const uint Bits27 = 0x7FFFFFFu;
        const uint Bits26 = 0x3FFFFFFu;
        const uint Bits25 = 0x1FFFFFFu;
        const uint Bits24 = 0xFFFFFFu;
        const uint Bits23 = 0x7FFFFFu;
        const uint Bits22 = 0x3FFFFFu;
        const uint Bits21 = 0x1FFFFFu;
        const uint Bits20 = 0xFFFFFu;
        const uint Bits19 = 0x7FFFFu;
        const uint Bits18 = 0x3FFFFu;
        const uint Bits17 = 0x1FFFFu;
        const uint Bits16 = 0xFFFFu;
        const uint Bits15 = 0x7FFFu;
        const uint Bits14 = 0x3FFFu;
        const uint Bits13 = 0x1FFFu;
        const uint Bits12 = 0xFFFu;
        const uint Bits11 = 0x7FFu;
        const uint Bits10 = 0x3FFu;
        const uint Bits9 = 0x1FFu;
        const uint Bits8 = 0xFFu;
        const uint Bits7 = 0x7Fu;
        const uint Bits6 = 0x3Fu;
        const uint Bits5 = 0x1Fu;
        const uint Bits4 = 0xFu;
        const uint Bits3 = 0x7u;
        const uint Bits2 = 0x3u;
        const uint Bits1 = 0x1u;
        const uint Bits0 = 0x0u;

        public ByteWriter Writer;

        private long m_prevTimestamp1;
        private TsscPointMetadata m_prevPoint;
        private TsscPointMetadata m_nextPoint;
        private TsscWordEncoding m_encoder;

        private IndexedArray<TsscPointMetadata> m_points;

        public AdaptiveEncoder()
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

        public override unsafe void AddDataPoint(SttpDataPoint point)
        {
            int channelID = m_channelMap.GetChannelID(point.Metadata, out bool isNew);
            m_nextPoint = m_points[channelID];
            if (m_nextPoint == null)
            {
                m_nextPoint = new TsscPointMetadata(Writer, null);
                m_nextPoint.PrevNextChannelId1 = (ushort)(channelID + 1);
                m_points[channelID] = m_nextPoint;
            }

            m_encoder = m_prevPoint.NextValueEncoding;

            //99% of the time, this data does not change. Therefore handle it differently.
            if (m_prevPoint.PrevNextChannelId1 != channelID
                || m_prevTimestamp1 != point.Time.Ticks
                || m_nextPoint.PrevQuality1 != (ulong)point.Quality
                || point.Value.ValueTypeCode != m_nextPoint.PrevTypeCode)
            {
                WriteExceptionCase(point, channelID);
            }

            switch (point.Value.ValueTypeCode)
            {
                case CtpTypeCode.Null:
                    m_encoder.WriteCode(TsscCodeWords.ValueRaw);
                    break;
                case CtpTypeCode.Int64:
                    ulong ul = (ulong)point.Value.IsInt64;
                    Write64(ul);
                    break;
                case CtpTypeCode.Single:
                    float f = point.Value.IsSingle;
                    Write64((ulong)(long)*(int*)&f);
                    break;
                case CtpTypeCode.Double:
                    float d = point.Value.IsSingle;
                    Write64(*(ulong*)&d);
                    break;
                case CtpTypeCode.CtpTime:
                    Write64((ulong)point.Value.IsCtpTime.Ticks);
                    break;
                case CtpTypeCode.Boolean:
                    if (point.Value.IsBoolean)
                        m_encoder.WriteCode(TsscCodeWords.ValueTrue);
                    else
                        m_encoder.WriteCode(TsscCodeWords.ValueFalse);
                    break;
                case CtpTypeCode.Guid:
                    if (m_nextPoint.Prev1 == point.Value)
                    {
                        m_encoder.WriteCode(TsscCodeWords.Value1);
                    }
                    else if (point.Value.AsGuid == Guid.Empty)
                    {
                        m_encoder.WriteCode(TsscCodeWords.ValueEmpty);
                        m_nextPoint.Prev1 = point.Value;
                    }
                    else
                    {
                        m_encoder.WriteCode(TsscCodeWords.ValueRaw);
                        CtpValueEncodingNative.Save(Writer, point.Value);
                        m_nextPoint.Prev1 = point.Value;
                    }
                    break;
                case CtpTypeCode.String:
                    if (m_nextPoint.Prev1 == point.Value)
                    {
                        m_encoder.WriteCode(TsscCodeWords.Value1);
                    }
                    else if (point.Value.IsString.Length == 0)
                    {
                        m_encoder.WriteCode(TsscCodeWords.ValueEmpty);
                        m_nextPoint.Prev1 = point.Value;
                    }
                    else
                    {
                        m_encoder.WriteCode(TsscCodeWords.ValueRaw);
                        CtpValueEncodingNative.Save(Writer, point.Value);
                        m_nextPoint.Prev1 = point.Value;
                    }
                    break;
                case CtpTypeCode.CtpBuffer:
                    if (m_nextPoint.Prev1 == point.Value)
                    {
                        m_encoder.WriteCode(TsscCodeWords.Value1);
                    }
                    else if (point.Value.IsCtpBuffer.Length == 0)
                    {
                        m_encoder.WriteCode(TsscCodeWords.ValueRaw);
                        m_nextPoint.Prev1 = point.Value;
                    }
                    else
                    {
                        m_encoder.WriteCode(TsscCodeWords.ValueRaw);
                        CtpValueEncodingNative.Save(Writer, point.Value);
                        m_nextPoint.Prev1 = point.Value;
                    }
                    break;
                case CtpTypeCode.CtpCommand:
                    if (m_nextPoint.Prev1 == point.Value)
                    {
                        m_encoder.WriteCode(TsscCodeWords.Value1);
                    }
                    else
                    {
                        m_encoder.WriteCode(TsscCodeWords.ValueRaw);
                        CtpValueEncodingNative.Save(Writer, point.Value);
                        m_nextPoint.Prev1 = point.Value;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (isNew)
            {
                CtpValueEncodingNative.Save(Writer, point.Metadata.DataPointID);
            }

            m_prevPoint = m_nextPoint;

        }


        private void WriteExceptionCase(SttpDataPoint point, int channelID)
        {
            if (m_prevPoint.PrevNextChannelId1 != channelID)
            {
                m_encoder.WriteCode(0);
                Writer.WriteBits2(0);
                Writer.Write8BitSegments((uint)channelID);
                m_prevPoint.PrevNextChannelId1 = channelID;
            }
            if (m_prevTimestamp1 != point.Time.Ticks)
            {
                long timestamp = point.Time.Ticks;
                m_encoder.WriteCode(0);
                Writer.WriteBits2(1);
                Writer.Write8BitSegments((ulong)(timestamp ^ m_prevTimestamp1));
                m_prevTimestamp1 = timestamp;
            }
            if (m_nextPoint.PrevQuality1 != (ulong)point.Quality)
            {
                ulong quality = (ulong)point.Quality;
                m_encoder.WriteCode(0);
                Writer.WriteBits2(2);
                Writer.Write8BitSegments(quality);
                m_nextPoint.PrevQuality1 = quality;
            }
            if (point.Value.ValueTypeCode != m_nextPoint.PrevTypeCode)
            {
                m_encoder.WriteCode(0);
                Writer.WriteBits2(3);
                Writer.WriteBits4((uint)point.Value.ValueTypeCode & 15);
                m_nextPoint.PrevTypeCode = point.Value.ValueTypeCode;
            }
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
                m_points = new IndexedArray<TsscPointMetadata>();
                m_prevPoint = new TsscPointMetadata(Writer, null);
                m_prevTimestamp1 = 0;
            }
        }

        private void Write64(ulong valueRaw)
        {
            TsscPointMetadata point = m_nextPoint;

            if (valueRaw == 0)
            {
                m_encoder.WriteCode(TsscCodeWords.ValueZero);
            }
            else if (point.PrevValue1 == valueRaw)
            {
                m_encoder.WriteCode(TsscCodeWords.ValueXOR0);
            }
            else
            {
                ulong bitsChanged;
                bitsChanged = CompareUInt64.Compare(valueRaw, point.PrevValue1);
                point.PrevValue1 = valueRaw;
                if (bitsChanged <= Bits4)
                {
                    m_encoder.WriteCode(TsscCodeWords.ValueXOR4);
                    Writer.WriteBits(4, bitsChanged);
                }
                else if (bitsChanged <= Bits8)
                {
                    m_encoder.WriteCode(TsscCodeWords.ValueXOR8);
                    Writer.WriteBits(8, bitsChanged);
                }
                else if (bitsChanged <= Bits10)
                {
                    m_encoder.WriteCode(TsscCodeWords.ValueXOR10);
                    Writer.WriteBits(10, bitsChanged);
                }
                else if (bitsChanged <= Bits12)
                {
                    m_encoder.WriteCode(TsscCodeWords.ValueXOR12);
                    Writer.WriteBits(12, bitsChanged);
                }
                else if (bitsChanged <= Bits14)
                {
                    m_encoder.WriteCode(TsscCodeWords.ValueXOR14);
                    Writer.WriteBits(14, bitsChanged);
                }
                else if (bitsChanged <= Bits16)
                {
                    m_encoder.WriteCode(TsscCodeWords.ValueXOR16);
                    Writer.WriteBits(16, bitsChanged);
                }
                else if (bitsChanged <= Bits18)
                {
                    m_encoder.WriteCode(TsscCodeWords.ValueXOR18);
                    Writer.WriteBits(18, bitsChanged);
                }
                else if (bitsChanged <= Bits20)
                {
                    m_encoder.WriteCode(TsscCodeWords.ValueXOR20);
                    Writer.WriteBits(20, bitsChanged);
                }
                else if (bitsChanged <= Bits24)
                {
                    m_encoder.WriteCode(TsscCodeWords.ValueXOR24);
                    Writer.WriteBits(24, bitsChanged);
                }
                else if (bitsChanged <= Bits28)
                {
                    m_encoder.WriteCode(TsscCodeWords.ValueXOR28);
                    Writer.WriteBits(28, bitsChanged);
                }
                else if (bitsChanged <= Bits32)
                {
                    m_encoder.WriteCode(TsscCodeWords.ValueXOR32);
                    Writer.WriteBits(32, bitsChanged);
                }
                else if (bitsChanged <= Bits36)
                {
                    m_encoder.WriteCode(TsscCodeWords.ValueXOR36);
                    Writer.WriteBits(36, bitsChanged);
                }
                else if (bitsChanged <= Bits40)
                {
                    m_encoder.WriteCode(TsscCodeWords.ValueXOR40);
                    Writer.WriteBits(40, bitsChanged);
                }
                else if (bitsChanged <= Bits44)
                {
                    m_encoder.WriteCode(TsscCodeWords.ValueXOR44);
                    Writer.WriteBits(44, bitsChanged);
                }
                else if (bitsChanged <= Bits48)
                {
                    m_encoder.WriteCode(TsscCodeWords.ValueXOR48);
                    Writer.WriteBits(48, bitsChanged);
                }
                else if (bitsChanged <= Bits52)
                {
                    m_encoder.WriteCode(TsscCodeWords.ValueXOR52);
                    Writer.WriteBits(52, bitsChanged);
                }
                else if (bitsChanged <= Bits56)
                {
                    m_encoder.WriteCode(TsscCodeWords.ValueXOR56);
                    Writer.WriteBits(56, bitsChanged);
                }
                else if (bitsChanged <= Bits60)
                {
                    m_encoder.WriteCode(TsscCodeWords.ValueXOR60);
                    Writer.WriteBits(60, bitsChanged);
                }
                else
                {
                    m_encoder.WriteCode(TsscCodeWords.ValueXOR64);
                    Writer.WriteBits(64, bitsChanged);
                }
            }
        }

    }
}
