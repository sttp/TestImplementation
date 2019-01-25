﻿using System;
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

        const uint Bits31 = 0x7FFFFFFu;
        const uint Bits30 = 0x3FFFFFFu;
        const uint Bits29 = 0x1FFFFFFu;
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
                    m_encoder.WriteCode(TsscCodeWordsElse.ValueRaw);
                    break;
                case CtpTypeCode.Int64:
                    ulong ul = (ulong)point.Value.IsInt64;
                    Write64(ul);
                    break;
                case CtpTypeCode.Single:
                    WriteSingle(point.Value.IsSingle);
                    break;
                case CtpTypeCode.Double:
                    WriteDouble(point.Value.IsDouble);
                    break;
                case CtpTypeCode.CtpTime:
                    Write64((ulong)point.Value.IsCtpTime.Ticks);
                    break;
                case CtpTypeCode.Boolean:
                    if (point.Value.IsBoolean)
                        m_encoder.WriteCode(TsscCodeWordsBool.ValueTrue);
                    else
                        m_encoder.WriteCode(TsscCodeWordsBool.ValueFalse);
                    break;
                case CtpTypeCode.Guid:
                    if (m_nextPoint.Prev1 == point.Value)
                    {
                        m_encoder.WriteCode(TsscCodeWordsElse.Value1);
                    }
                    else if (m_nextPoint.Prev2 == point.Value)
                    {
                        m_encoder.WriteCode(TsscCodeWordsElse.Value2);
                        m_nextPoint.Prev2 = m_nextPoint.Prev1;
                        m_nextPoint.Prev1 = point.Value;
                    }
                    else if (m_nextPoint.Prev3 == point.Value)
                    {
                        m_encoder.WriteCode(TsscCodeWordsElse.Value3);
                        m_nextPoint.Prev3 = m_nextPoint.Prev2;
                        m_nextPoint.Prev2 = m_nextPoint.Prev1;
                        m_nextPoint.Prev1 = point.Value;
                    }
                    else if (point.Value.AsGuid == Guid.Empty)
                    {
                        m_encoder.WriteCode(TsscCodeWordsElse.ValueEmpty);
                        m_nextPoint.Prev3 = m_nextPoint.Prev2;
                        m_nextPoint.Prev2 = m_nextPoint.Prev1;
                        m_nextPoint.Prev1 = point.Value;
                    }
                    else
                    {
                        m_encoder.WriteCode(TsscCodeWordsElse.ValueRaw);
                        CtpValueEncodingNative.Save(Writer, point.Value);
                    }
                    break;
                case CtpTypeCode.String:
                    if (m_nextPoint.Prev1 == point.Value)
                    {
                        m_encoder.WriteCode(TsscCodeWordsElse.Value1);
                    }
                    else if (m_nextPoint.Prev2 == point.Value)
                    {
                        m_encoder.WriteCode(TsscCodeWordsElse.Value2);
                        m_nextPoint.Prev2 = m_nextPoint.Prev1;
                        m_nextPoint.Prev1 = point.Value;
                    }
                    else if (m_nextPoint.Prev3 == point.Value)
                    {
                        m_encoder.WriteCode(TsscCodeWordsElse.Value3);
                        m_nextPoint.Prev3 = m_nextPoint.Prev2;
                        m_nextPoint.Prev2 = m_nextPoint.Prev1;
                        m_nextPoint.Prev1 = point.Value;
                    }
                    else if (point.Value.IsString.Length == 0)
                    {
                        m_encoder.WriteCode(TsscCodeWordsElse.ValueEmpty);
                        m_nextPoint.Prev3 = m_nextPoint.Prev2;
                        m_nextPoint.Prev2 = m_nextPoint.Prev1;
                        m_nextPoint.Prev1 = point.Value;
                    }
                    else
                    {
                        m_encoder.WriteCode(TsscCodeWordsElse.ValueRaw);
                        CtpValueEncodingNative.Save(Writer, point.Value);
                    }
                    break;
                case CtpTypeCode.CtpBuffer:
                    if (m_nextPoint.Prev1 == point.Value)
                    {
                        m_encoder.WriteCode(TsscCodeWordsElse.Value1);
                    }
                    else if (m_nextPoint.Prev2 == point.Value)
                    {
                        m_encoder.WriteCode(TsscCodeWordsElse.Value2);
                        m_nextPoint.Prev2 = m_nextPoint.Prev1;
                        m_nextPoint.Prev1 = point.Value;
                    }
                    else if (m_nextPoint.Prev3 == point.Value)
                    {
                        m_encoder.WriteCode(TsscCodeWordsElse.Value3);
                        m_nextPoint.Prev3 = m_nextPoint.Prev2;
                        m_nextPoint.Prev2 = m_nextPoint.Prev1;
                        m_nextPoint.Prev1 = point.Value;
                    }
                    else if (point.Value.IsCtpBuffer.Length == 0)
                    {
                        m_encoder.WriteCode(TsscCodeWordsElse.ValueEmpty);
                        m_nextPoint.Prev3 = m_nextPoint.Prev2;
                        m_nextPoint.Prev2 = m_nextPoint.Prev1;
                        m_nextPoint.Prev1 = point.Value;
                    }
                    else
                    {
                        m_encoder.WriteCode(TsscCodeWordsElse.ValueRaw);
                        CtpValueEncodingNative.Save(Writer, point.Value);
                    }
                    break;
                case CtpTypeCode.CtpCommand:
                    if (m_nextPoint.Prev1 == point.Value)
                    {
                        m_encoder.WriteCode(TsscCodeWordsElse.Value1);
                    }
                    else if (m_nextPoint.Prev2 == point.Value)
                    {
                        m_encoder.WriteCode(TsscCodeWordsElse.Value2);
                        m_nextPoint.Prev2 = m_nextPoint.Prev1;
                        m_nextPoint.Prev1 = point.Value;
                    }
                    else if (m_nextPoint.Prev3 == point.Value)
                    {
                        m_encoder.WriteCode(TsscCodeWordsElse.Value3);
                        m_nextPoint.Prev3 = m_nextPoint.Prev2;
                        m_nextPoint.Prev2 = m_nextPoint.Prev1;
                        m_nextPoint.Prev1 = point.Value;
                    }
                    else
                    {
                        m_encoder.WriteCode(TsscCodeWordsElse.ValueRaw);
                        CtpValueEncodingNative.Save(Writer, point.Value);
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
                Writer.Write4BitSegments((uint)channelID);
                m_prevPoint.PrevNextChannelId1 = channelID;
            }
            if (m_prevTimestamp1 != point.Time.Ticks)
            {
                long timestamp = point.Time.Ticks;
                m_prevTimestamp1 = timestamp;
                m_encoder.WriteCode(0);
                Writer.WriteBits2(1);
                Writer.Write8BitSegments((ulong)(timestamp ^ m_prevTimestamp1));
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

        private unsafe void Write64(ulong value)
        {
            throw new NotImplementedException();
        }

        private unsafe void WriteDouble(double value)
        {
            throw new NotSupportedException();
            //TsscPointMetadata point = m_nextPoint;
            //ulong valueRaw = *(ulong*)&value;

            //if (valueRaw == 0)
            //{
            //    m_encoder.WriteCode(TsscCodeWordsFloat.ValueZero);
            //}
            //else if (point.PrevValue1 == valueRaw)
            //{
            //    m_encoder.WriteCode(TsscCodeWordsFloat.ValueXOR0);
            //}
            //else
            //{
            //    uint bitsChanged;
            //    bitsChanged = CompareUInt32.Compare(valueRaw, (uint)point.PrevValue1);
            //    point.PrevValue1 = valueRaw;
            //    if (bitsChanged <= Bits6)
            //    {
            //        m_encoder.WriteCode(TsscCodeWordsFloat.ValueXOR6);
            //        Writer.WriteBits(6, bitsChanged);
            //    }
            //    else if (bitsChanged <= Bits9)
            //    {
            //        m_encoder.WriteCode(TsscCodeWordsFloat.ValueXOR9);
            //        Writer.WriteBits(9, bitsChanged);
            //    }
            //    else if (bitsChanged <= Bits12)
            //    {
            //        m_encoder.WriteCode(TsscCodeWordsFloat.ValueXOR12);
            //        Writer.WriteBits(12, bitsChanged);
            //    }
            //    else if (bitsChanged <= Bits15)
            //    {
            //        m_encoder.WriteCode(TsscCodeWordsFloat.ValueXOR15);
            //        Writer.WriteBits(15, bitsChanged);
            //    }
            //    else if (bitsChanged <= Bits18)
            //    {
            //        m_encoder.WriteCode(TsscCodeWordsFloat.ValueXOR18);
            //        Writer.WriteBits(18, bitsChanged);
            //    }
            //    else if (bitsChanged <= Bits21)
            //    {
            //        m_encoder.WriteCode(TsscCodeWordsFloat.ValueXOR21);
            //        Writer.WriteBits(21, bitsChanged);
            //    }
            //    else if (bitsChanged <= Bits24)
            //    {
            //        m_encoder.WriteCode(TsscCodeWordsFloat.ValueXOR24);
            //        Writer.WriteBits(24, bitsChanged);
            //    }
            //    else if (bitsChanged <= Bits28)
            //    {
            //        m_encoder.WriteCode(TsscCodeWordsFloat.ValueXOR28);
            //        Writer.WriteBits(28, bitsChanged);
            //    }
            //    else
            //    {
            //        m_encoder.WriteCode(TsscCodeWordsFloat.ValueXOR32);
            //        Writer.WriteBits32(bitsChanged);
            //    }
            //}
        }
        private unsafe void WriteSingle(float value)
        {
            TsscPointMetadata point = m_nextPoint;
            uint valueRaw = *(uint*)&value;

            if (valueRaw == 0)
            {
                m_encoder.WriteCode(TsscCodeWordsFloat.ValueZero);
            }
            else if (point.PrevValue1 == valueRaw)
            {
                m_encoder.WriteCode(TsscCodeWordsFloat.ValueXOR0);
            }
            else
            {
                uint bitsChanged;
                bitsChanged = CompareUInt32.Compare(valueRaw, (uint)point.PrevValue1);
                point.PrevValue1 = valueRaw;
                if (bitsChanged <= Bits5)
                {
                    m_encoder.WriteCode(TsscCodeWordsFloat.ValueXOR5);
                    Writer.WriteBits(5, bitsChanged);
                }
                else if (bitsChanged <= Bits7)
                {
                    m_encoder.WriteCode(TsscCodeWordsFloat.ValueXOR7);
                    Writer.WriteBits(7, bitsChanged);
                }
                else if (bitsChanged <= Bits9)
                {
                    m_encoder.WriteCode(TsscCodeWordsFloat.ValueXOR9);
                    Writer.WriteBits(9, bitsChanged);
                }
                else if (bitsChanged <= Bits11)
                {
                    m_encoder.WriteCode(TsscCodeWordsFloat.ValueXOR11);
                    Writer.WriteBits(11, bitsChanged);
                }
                else if (bitsChanged <= Bits13)
                {
                    m_encoder.WriteCode(TsscCodeWordsFloat.ValueXOR13);
                    Writer.WriteBits(13, bitsChanged);
                }
                else if (bitsChanged <= Bits15)
                {
                    m_encoder.WriteCode(TsscCodeWordsFloat.ValueXOR15);
                    Writer.WriteBits(15, bitsChanged);
                }
                else if (bitsChanged <= Bits17)
                {
                    m_encoder.WriteCode(TsscCodeWordsFloat.ValueXOR17);
                    Writer.WriteBits(17, bitsChanged);
                }
                else if (bitsChanged <= Bits19)
                {
                    m_encoder.WriteCode(TsscCodeWordsFloat.ValueXOR19);
                    Writer.WriteBits(19, bitsChanged);
                }
                else if (bitsChanged <= Bits21)
                {
                    m_encoder.WriteCode(TsscCodeWordsFloat.ValueXOR21);
                    Writer.WriteBits(21, bitsChanged);
                }
                else if (bitsChanged <= Bits23)
                {
                    m_encoder.WriteCode(TsscCodeWordsFloat.ValueXOR23);
                    Writer.WriteBits(23, bitsChanged);
                }
                else if (bitsChanged <= Bits26)
                {
                    m_encoder.WriteCode(TsscCodeWordsFloat.ValueXOR26);
                    Writer.WriteBits(26, bitsChanged);
                }
                else if (bitsChanged <= Bits28)
                {
                    m_encoder.WriteCode(TsscCodeWordsFloat.ValueXOR28);
                    Writer.WriteBits(28, bitsChanged);
                }
                else
                {
                    m_encoder.WriteCode(TsscCodeWordsFloat.ValueXOR32);
                    Writer.WriteBits32(bitsChanged);
                }
            }
        }
    }
}