//******************************************************************************************************
//  TsscEncoder.cs - Gbtc
//
//  Copyright © 2016, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  12/02/2016 - Steven E. Chisholm
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using CTP;
using Sttp.Collection;

namespace Sttp.DataPointEncoding
{
    /// <summary>
    /// An encoder for the TSSC protocol.
    /// </summary>
    public class TsscEncoder
    {
        const uint Bits28 = 0xFFFFFFFu;
        const uint Bits24 = 0xFFFFFFu;
        const uint Bits20 = 0xFFFFFu;
        const uint Bits16 = 0xFFFFu;
        const uint Bits12 = 0xFFFu;
        const uint Bits8 = 0xFFu;
        const uint Bits4 = 0xFu;
        const uint Bits0 = 0x0u;

        public ByteWriter Writer;

        private long m_prevTimestamp1;
        private long m_prevTimestamp2;

        private long m_prevTimeDelta1;
        private long m_prevTimeDelta2;
        private long m_prevTimeDelta3;
        private long m_prevTimeDelta4;

        private TsscPointMetadata m_lastPoint;
        private IndexedArray<TsscPointMetadata> m_points;

        /// <summary>
        /// Creates a encoder for the TSSC protocol.
        /// </summary>
        public TsscEncoder()
        {
            Writer = new ByteWriter();
            Reset(true);
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
                m_lastPoint = new TsscPointMetadata(Writer, null);

                m_prevTimeDelta1 = long.MaxValue;
                m_prevTimeDelta2 = long.MaxValue;
                m_prevTimeDelta3 = long.MaxValue;
                m_prevTimeDelta4 = long.MaxValue;
                m_prevTimestamp1 = 0;
                m_prevTimestamp2 = 0;
            }
        }

        /// <summary>
        /// Adds the supplied measurement to the stream. If the stream is full,
        /// this method returns false.
        /// </summary>
        /// <param name="channelID">the id</param>
        /// <param name="timestamp">the timestamp in ticks</param>
        /// <param name="quality">the quality</param>
        /// <param name="value">the value</param>
        /// <returns>true if successful, false otherwise.</returns>
        public unsafe void AddMeasurement(int channelID, long timestamp, ulong quality, CtpObject value)
        {
            TsscPointMetadata point = m_points[channelID];
            if (point == null)
            {
                point = new TsscPointMetadata(Writer, null);
                point.PrevNextPointId1 = (ushort)(channelID + 1);
                m_points[channelID] = point;
            }

            WritePointId(channelID);

            m_lastPoint = point;

            if (m_prevTimestamp1 != timestamp)
            {
                WriteTimestampChange(timestamp);
            }

            if (point.PrevQuality1 != quality)
            {
                WriteQualityChange(quality, point);
            }

            if (value.ValueTypeCode != m_lastPoint.PrevTypeCode)
            {
                m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsCommon.ValueTypeChanged);
                Writer.WriteBits4((uint)value.ValueTypeCode & 15);
                m_lastPoint.PrevTypeCode = value.ValueTypeCode;
            }

            switch (value.ValueTypeCode)
            {
                case CtpTypeCode.Null:
                    m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsElse.ValueRaw);
                    break;
                case CtpTypeCode.Int64:
                    ulong ul = (ulong)value.IsInt64;
                    if (ul == (uint)ul)
                        Write32((uint)ul, point);
                    else
                        Write64(ul, point);
                    break;
                case CtpTypeCode.Single:
                    float s = value.IsSingle;
                    Write32(*(uint*)&s, point);
                    break;
                case CtpTypeCode.Double:
                    double d = value.IsDouble;
                    Write64(*(ulong*)&s, point);
                    break;
                case CtpTypeCode.CtpTime:
                    Write64((ulong)value.IsCtpTime.Ticks, point);
                    break;
                case CtpTypeCode.Boolean:
                    if (value.IsBoolean)
                        m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsBool.ValueTrue);
                    else
                        m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsBool.ValueFalse);
                    break;
                case CtpTypeCode.Guid:
                    if (point.Prev1 == value)
                    {
                        m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsElse.Value1);
                    }
                    else if (point.Prev2 == value)
                    {
                        m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsElse.Value2);
                        point.Prev2 = point.Prev1;
                        point.Prev1 = value;
                    }
                    else if (point.Prev3 == value)
                    {
                        m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsElse.Value3);
                        point.Prev3 = point.Prev2;
                        point.Prev2 = point.Prev1;
                        point.Prev1 = value;
                    }
                    else if (value.AsGuid == Guid.Empty)
                    {
                        m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsElse.ValueEmpty);
                        point.Prev3 = point.Prev2;
                        point.Prev2 = point.Prev1;
                        point.Prev1 = value;
                    }
                    else
                    {
                        m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsElse.ValueRaw);
                        CtpValueEncodingNative.Save(Writer, value);
                    }
                    break;
                case CtpTypeCode.String:
                    if (point.Prev1 == value)
                    {
                        m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsElse.Value1);
                    }
                    else if (point.Prev2 == value)
                    {
                        m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsElse.Value2);
                        point.Prev2 = point.Prev1;
                        point.Prev1 = value;
                    }
                    else if (point.Prev3 == value)
                    {
                        m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsElse.Value3);
                        point.Prev3 = point.Prev2;
                        point.Prev2 = point.Prev1;
                        point.Prev1 = value;
                    }
                    else if (value.IsString.Length == 0)
                    {
                        m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsElse.ValueEmpty);
                        point.Prev3 = point.Prev2;
                        point.Prev2 = point.Prev1;
                        point.Prev1 = value;
                    }
                    else
                    {
                        m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsElse.ValueRaw);
                        CtpValueEncodingNative.Save(Writer, value);
                    }
                    break;
                case CtpTypeCode.CtpBuffer:
                    if (point.Prev1 == value)
                    {
                        m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsElse.Value1);
                    }
                    else if (point.Prev2 == value)
                    {
                        m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsElse.Value2);
                        point.Prev2 = point.Prev1;
                        point.Prev1 = value;
                    }
                    else if (point.Prev3 == value)
                    {
                        m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsElse.Value3);
                        point.Prev3 = point.Prev2;
                        point.Prev2 = point.Prev1;
                        point.Prev1 = value;
                    }
                    else if (value.IsCtpBuffer.Length == 0)
                    {
                        m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsElse.ValueEmpty);
                        point.Prev3 = point.Prev2;
                        point.Prev2 = point.Prev1;
                        point.Prev1 = value;
                    }
                    else
                    {
                        m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsElse.ValueRaw);
                        CtpValueEncodingNative.Save(Writer, value);
                    }
                    break;
                case CtpTypeCode.CtpCommand:
                    if (point.Prev1 == value)
                    {
                        m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsElse.Value1);
                    }
                    else if (point.Prev2 == value)
                    {
                        m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsElse.Value2);
                        point.Prev2 = point.Prev1;
                        point.Prev1 = value;
                    }
                    else if (point.Prev3 == value)
                    {
                        m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsElse.Value3);
                        point.Prev3 = point.Prev2;
                        point.Prev2 = point.Prev1;
                        point.Prev1 = value;
                    }
                    else
                    {
                        m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsElse.ValueRaw);
                        CtpValueEncodingNative.Save(Writer, value);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return;
        }

        private unsafe void Write64(ulong value, TsscPointMetadata point)
        {
            uint valueRaw = *(uint*)&value;
            if (point.PrevValue1 == valueRaw)
            {
                m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsNumeric.Value1);
            }
            else if (point.PrevValue2 == valueRaw)
            {
                m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsNumeric.Value2);
                point.PrevValue2 = point.PrevValue1;
                point.PrevValue1 = valueRaw;
            }
            else if (point.PrevValue3 == valueRaw)
            {
                m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsNumeric.Value3);
                point.PrevValue3 = point.PrevValue2;
                point.PrevValue2 = point.PrevValue1;
                point.PrevValue1 = valueRaw;
            }
            else if (valueRaw == 0)
            {
                m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsNumeric.ValueZero);
                point.PrevValue3 = point.PrevValue2;
                point.PrevValue2 = point.PrevValue1;
                point.PrevValue1 = 0;
            }
            else
            {
                uint bitsChanged = valueRaw ^ (uint)point.PrevValue1;

                if (bitsChanged <= Bits4)
                {
                    m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsNumeric.ValueXOR4);
                    Writer.WriteBits4(bitsChanged);
                }
                else if (bitsChanged <= Bits8)
                {
                    m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsNumeric.ValueXOR8);
                    Writer.WriteBits8(bitsChanged);
                }
                else if (bitsChanged <= Bits12)
                {
                    m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsNumeric.ValueXOR12);
                    Writer.WriteBits12(bitsChanged);
                }
                else if (bitsChanged <= Bits16)
                {
                    m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsNumeric.ValueXOR16);
                    Writer.WriteBits16(bitsChanged);
                }
                else if (bitsChanged <= Bits20)
                {
                    m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsNumeric.ValueXOR20);
                    Writer.WriteBits20(bitsChanged);
                }
                else if (bitsChanged <= Bits24)
                {
                    m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsNumeric.ValueXOR24);
                    Writer.WriteBits24(bitsChanged);
                }
                else if (bitsChanged <= Bits28)
                {
                    m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsNumeric.ValueXOR28);
                    Writer.WriteBits28(bitsChanged);
                }
                else
                {
                    m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsNumeric.ValueXOR32);
                    Writer.WriteBits32(bitsChanged);
                }

                point.PrevValue3 = point.PrevValue2;
                point.PrevValue2 = point.PrevValue1;
                point.PrevValue1 = valueRaw;
            }
        }

        private unsafe void Write32(uint value, TsscPointMetadata point)
        {
            uint valueRaw = *(uint*)&value;
            if (point.PrevValue1 == valueRaw)
            {
                m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsNumeric.Value1);
            }
            else if (point.PrevValue2 == valueRaw)
            {
                m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsNumeric.Value2);
                point.PrevValue2 = point.PrevValue1;
                point.PrevValue1 = valueRaw;
            }
            else if (point.PrevValue3 == valueRaw)
            {
                m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsNumeric.Value3);
                point.PrevValue3 = point.PrevValue2;
                point.PrevValue2 = point.PrevValue1;
                point.PrevValue1 = valueRaw;
            }
            else if (valueRaw == 0)
            {
                m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsNumeric.ValueZero);
                point.PrevValue3 = point.PrevValue2;
                point.PrevValue2 = point.PrevValue1;
                point.PrevValue1 = 0;
            }
            else
            {
                uint bitsChanged = valueRaw ^ (uint)point.PrevValue1;

                if (bitsChanged <= Bits4)
                {
                    m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsNumeric.ValueXOR4);
                    Writer.WriteBits4(bitsChanged);
                }
                else if (bitsChanged <= Bits8)
                {
                    m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsNumeric.ValueXOR8);
                    Writer.WriteBits8(bitsChanged);
                }
                else if (bitsChanged <= Bits12)
                {
                    m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsNumeric.ValueXOR12);
                    Writer.WriteBits12(bitsChanged);
                }
                else if (bitsChanged <= Bits16)
                {
                    m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsNumeric.ValueXOR16);
                    Writer.WriteBits16(bitsChanged);
                }
                else if (bitsChanged <= Bits20)
                {
                    m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsNumeric.ValueXOR20);
                    Writer.WriteBits20(bitsChanged);
                }
                else if (bitsChanged <= Bits24)
                {
                    m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsNumeric.ValueXOR24);
                    Writer.WriteBits24(bitsChanged);
                }
                else if (bitsChanged <= Bits28)
                {
                    Writer.WriteBits28(bitsChanged);
                    m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsNumeric.ValueXOR28);
                }
                else
                {
                    m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsNumeric.ValueXOR32);
                    Writer.WriteBits32(bitsChanged);
                }

                point.PrevValue3 = point.PrevValue2;
                point.PrevValue2 = point.PrevValue1;
                point.PrevValue1 = valueRaw;
            }
        }

        private void WritePointId(int id)
        {
            if (id == m_lastPoint.PrevNextPointId1)
            {
                m_lastPoint.PointIDEncoding.WriteCode(TsscCodeWordsPointID.PointNextIDSameAsPrevious1);

            }
            else if (id == m_lastPoint.PrevNextPointId2)
            {
                m_lastPoint.PointIDEncoding.WriteCode(TsscCodeWordsPointID.PointNextIDSameAsPrevious2);
                m_lastPoint.PrevNextPointId2 = m_lastPoint.PrevNextPointId1;
                m_lastPoint.PrevNextPointId1 = id;
            }
            else
            {
                uint bitsChanged = (uint)(id ^ m_lastPoint.PrevNextPointId1);

                if (bitsChanged <= Bits8)
                {
                    m_lastPoint.PointIDEncoding.WriteCode(TsscCodeWordsPointID.PointIDXOR8);
                    Writer.WriteBits8(bitsChanged);
                }
                else if (bitsChanged <= Bits12)
                {
                    m_lastPoint.PointIDEncoding.WriteCode(TsscCodeWordsPointID.PointIDXOR12);
                    Writer.WriteBits12(bitsChanged);
                }
                else if (bitsChanged <= Bits16)
                {
                    m_lastPoint.PointIDEncoding.WriteCode(TsscCodeWordsPointID.PointIDXOR16);
                    Writer.WriteBits16(bitsChanged);
                }
                else if (bitsChanged <= Bits24)
                {
                    m_lastPoint.PointIDEncoding.WriteCode(TsscCodeWordsPointID.PointIDXOR24);
                    Writer.WriteBits24(bitsChanged);
                }
                else
                {
                    m_lastPoint.PointIDEncoding.WriteCode(TsscCodeWordsPointID.PointID32);
                    Writer.WriteBits32(bitsChanged);
                }
                m_lastPoint.PrevNextPointId2 = m_lastPoint.PrevNextPointId1;
                m_lastPoint.PrevNextPointId1 = id;
            }
        }

        private void WriteTimestampChange(long timestamp)
        {
            if (m_prevTimestamp2 == timestamp)
            {
                m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsCommon.Timestamp2);
            }
            else if (m_prevTimestamp1 < timestamp)
            {
                if (m_prevTimestamp1 + m_prevTimeDelta1 == timestamp)
                {
                    m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsCommon.TimeDelta1Forward);
                }
                else if (m_prevTimestamp1 + m_prevTimeDelta2 == timestamp)
                {
                    m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsCommon.TimeDelta2Forward);
                }
                else if (m_prevTimestamp1 + m_prevTimeDelta3 == timestamp)
                {
                    m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsCommon.TimeDelta3Forward);
                }
                else if (m_prevTimestamp1 + m_prevTimeDelta4 == timestamp)
                {
                    m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsCommon.TimeDelta4Forward);
                }
                else
                {
                    m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsCommon.TimeXOR7Bit);
                    Writer.Write8BitSegments((ulong)(timestamp ^ m_prevTimestamp1));
                }
            }
            else
            {
                if (m_prevTimestamp1 - m_prevTimeDelta1 == timestamp)
                {
                    m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsCommon.TimeDelta1Reverse);
                }
                else if (m_prevTimestamp1 - m_prevTimeDelta2 == timestamp)
                {
                    m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsCommon.TimeDelta2Reverse);
                }
                else if (m_prevTimestamp1 - m_prevTimeDelta3 == timestamp)
                {
                    m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsCommon.TimeDelta3Reverse);
                }
                else if (m_prevTimestamp1 - m_prevTimeDelta4 == timestamp)
                {
                    m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsCommon.TimeDelta4Reverse);
                }
                else
                {
                    m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsCommon.TimeXOR7Bit);
                    Writer.Write8BitSegments((ulong)(timestamp ^ m_prevTimestamp1));
                }
            }

            //Save the smallest delta time
            long minDelta = Math.Abs(m_prevTimestamp1 - timestamp);

            if (minDelta < m_prevTimeDelta4
                && minDelta != m_prevTimeDelta1
                && minDelta != m_prevTimeDelta2
                && minDelta != m_prevTimeDelta3)
            {
                if (minDelta < m_prevTimeDelta1)
                {
                    m_prevTimeDelta4 = m_prevTimeDelta3;
                    m_prevTimeDelta3 = m_prevTimeDelta2;
                    m_prevTimeDelta2 = m_prevTimeDelta1;
                    m_prevTimeDelta1 = minDelta;
                }
                else if (minDelta < m_prevTimeDelta2)
                {
                    m_prevTimeDelta4 = m_prevTimeDelta3;
                    m_prevTimeDelta3 = m_prevTimeDelta2;
                    m_prevTimeDelta2 = minDelta;
                }
                else if (minDelta < m_prevTimeDelta3)
                {
                    m_prevTimeDelta4 = m_prevTimeDelta3;
                    m_prevTimeDelta3 = minDelta;
                }
                else
                {
                    m_prevTimeDelta4 = minDelta;
                }
            }
            m_prevTimestamp2 = m_prevTimestamp1;
            m_prevTimestamp1 = timestamp;
        }

        private void WriteQualityChange(ulong quality, TsscPointMetadata point)
        {
            if (point.PrevQuality2 == quality)
            {
                m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsCommon.Quality2);
            }
            else
            {
                m_lastPoint.ValueEncoding.WriteCode(TsscCodeWordsCommon.Quality7Bit32);
                Writer.Write8BitSegments(quality);
            }
            point.PrevQuality2 = point.PrevQuality1;
            point.PrevQuality1 = quality;
        }

    }
}
