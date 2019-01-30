//******************************************************************************************************
//  TsscDecoder.cs - Gbtc
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
    /// The decoder for the TSSC protocol.
    /// </summary>
    public class TsscDecoder
    {
        private long m_prevTimestamp1;
        private long m_prevTimestamp2;

        private long m_prevTimeDelta1;
        private long m_prevTimeDelta2;
        private long m_prevTimeDelta3;
        private long m_prevTimeDelta4;

        private AdvancedPointMetadata m_lastPoint;
        private IndexedArray<AdvancedPointMetadata> m_points;
        private ByteReader m_reader;

        /// <summary>
        /// Creates a decoder for the TSSC protocol.
        /// </summary>
        public TsscDecoder()
        {
            m_reader = new ByteReader();
            Reset();
        }

        /// <summary>
        /// Resets the TSSC Decoder to the initial state. 
        /// </summary>
        /// <remarks>
        /// TSSC is a stateful encoder that requires a state
        /// of the previous data to be maintained. Therefore, if 
        /// the state ever becomes corrupt (out of order, dropped, corrupted, or duplicated)
        /// the state must be reset on both ends.
        /// </remarks>
        public void Reset()
        {
            m_points = new IndexedArray<AdvancedPointMetadata>();
            m_lastPoint = new AdvancedPointMetadata(null, m_reader, null);
            m_prevTimeDelta1 = long.MaxValue;
            m_prevTimeDelta2 = long.MaxValue;
            m_prevTimeDelta3 = long.MaxValue;
            m_prevTimeDelta4 = long.MaxValue;
            m_prevTimestamp1 = 0;
            m_prevTimestamp2 = 0;
        }

        /// <summary>
        /// Sets the internal buffer to read data from.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="startingPosition"></param>
        /// <param name="length"></param>
        public void SetBuffer(byte[] data, int startingPosition, int length)
        {
            data.ValidateParameters(startingPosition, length);
            m_reader.SetBuffer(data, startingPosition, length);
        }

        /// <summary>
        /// Reads the next measurement from the stream. If the end of the stream has been encountered, 
        /// return false.
        /// </summary>
        /// <param name="id">the id</param>
        /// <param name="timestamp">the timestamp in ticks</param>
        /// <param name="quality">the quality</param>
        /// <param name="value">the value</param>
        /// <returns>true if successful, false otherwise.</returns>
        public unsafe bool TryGetMeasurement(out int id, out CtpTime timestamp, out ulong quality, out CtpObject value)
        {
            AdvancedPointMetadata nextPoint = null;

            if (m_reader.IsEmpty)
            {
                id = 0;
                timestamp = default(CtpTime);
                quality = 0;
                value = 0;
                return false;
            }

            id = 0;
            timestamp = default(CtpTime);
            quality = 0;
            value = 0;
            return false;

            //Note: since I will not know the incoming pointID. The most recent
            //      measurement received will be the one that contains the 
            //      coding algorithm for this measurement. Since for the more part
            //      measurements generally have some sort of sequence to them, 
            //      this still ends up being a good enough assumption.

            //uint code = m_encoding.ReadCode();

            //if (code == TsscExceptionWords.EndOfStream)
            //{
            //    id = 0;
            //    timestamp = default(CtpTime);
            //    quality = 0;
            //    value = 0;
            //    return false;
            //}

            //id = DecodePointID(code, m_lastPoint);
            //code = m_encoding.ReadCode();
            //if (code < TsscExceptionWords.TimeDelta1Forward)
            //    throw new Exception($"Expecting code >= {TsscExceptionWords.TimeDelta1Forward} Received {code}");


            //if (code <= TsscExceptionWords.TimeXOR7Bit)
            //{
            //    timestamp = DecodeTimestamp(code);
            //    code = m_encoding.ReadCode();
            //    if (code < TsscExceptionWords.Quality2)
            //        throw new Exception($"Expecting code >= {TsscExceptionWords.Quality2} Received {code}");
            //}
            //else
            //{
            //    timestamp = new CtpTime(m_prevTimestamp1);
            //}

            //if (code <= TsscExceptionWords.Quality7Bit32)
            //{
            //    quality = DecodeQuality(code, m_lastPoint);
            //    code = m_encoding.ReadCode();
            //    if (code < TsscExceptionWords.ValueTypeChanged)
            //        throw new Exception($"Expecting code >= {TsscExceptionWords.ValueTypeChanged} Received {code}");
            //}
            //else
            //{
            //    quality = nextPoint.PrevQuality1;
            //}

            //if (code == TsscExceptionWords.ValueTypeChanged)
            //{
            //    m_lastPoint.PrevTypeCode = (CtpTypeCode)m_reader.ReadBits4();
            //}

            ////Since value will almost always change, 
            ////This is not put inside a function call.
            //ulong valueRaw = 0;
            //if (code == TsscCodeWordsNumeric.Value1)
            //{
            //    valueRaw = nextPoint.PrevValue1;
            //}
            //else if (code == TsscCodeWordsNumeric.Value2)
            //{
            //    valueRaw = nextPoint.PrevValue2;
            //    nextPoint.PrevValue2 = nextPoint.PrevValue1;
            //    nextPoint.PrevValue1 = valueRaw;
            //}
            //else if (code == TsscCodeWordsNumeric.Value3)
            //{
            //    valueRaw = nextPoint.PrevValue3;
            //    nextPoint.PrevValue3 = nextPoint.PrevValue2;
            //    nextPoint.PrevValue2 = nextPoint.PrevValue1;
            //    nextPoint.PrevValue1 = valueRaw;
            //}
            //else if (code == TsscCodeWordsNumeric.ValueZero)
            //{
            //    valueRaw = 0;
            //    nextPoint.PrevValue3 = nextPoint.PrevValue2;
            //    nextPoint.PrevValue2 = nextPoint.PrevValue1;
            //    nextPoint.PrevValue1 = valueRaw;
            //}
            //else
            //{
            //    switch (code)
            //    {
            //        case TsscCodeWordsNumeric.ValueZero + 4:
            //            valueRaw = m_reader.ReadBits4() ^ nextPoint.PrevValue1;
            //            break;
            //        case TsscCodeWordsNumeric.ValueZero + 8:
            //            valueRaw = m_reader.ReadBits8() ^ nextPoint.PrevValue1;
            //            break;
            //        case TsscCodeWordsNumeric.ValueZero + 12:
            //            valueRaw = m_reader.ReadBits12() ^ nextPoint.PrevValue1;
            //            break;
            //        case TsscCodeWordsNumeric.ValueZero + 16:
            //            valueRaw = m_reader.ReadBits16() ^ nextPoint.PrevValue1;
            //            break;
            //        case TsscCodeWordsNumeric.ValueZero + 20:
            //            valueRaw = m_reader.ReadBits20() ^ nextPoint.PrevValue1;
            //            break;
            //        case TsscCodeWordsNumeric.ValueZero + 24:
            //            valueRaw = m_reader.ReadBits24() ^ nextPoint.PrevValue1;
            //            break;
            //        case TsscCodeWordsNumeric.ValueZero + 28:
            //            valueRaw = m_reader.ReadBits28() ^ nextPoint.PrevValue1;
            //            break;
            //        case TsscCodeWordsNumeric.ValueZero + 32:
            //            valueRaw = m_reader.ReadBits32() ^ nextPoint.PrevValue1;
            //            break;
            //        //case TsscCodeWordsNumeric.ValueXOR40:
            //        //    valueRaw = m_reader.ReadBits40() ^ nextPoint.PrevValue1;
            //        //    break;
            //        //case TsscCodeWordsNumeric.ValueXOR48:
            //        //    valueRaw = m_reader.ReadBits48() ^ nextPoint.PrevValue1;
            //        //    break;
            //        //case TsscCodeWordsNumeric.ValueXOR56:
            //        //    valueRaw = m_reader.ReadBits56() ^ nextPoint.PrevValue1;
            //        //    break;
            //        //case TsscCodeWordsNumeric.ValueXOR64:
            //        //    valueRaw = m_reader.ReadBits64() ^ nextPoint.PrevValue1;
            //        //    break;
            //        default:
            //            throw new Exception($"Invalid code received {code}");
            //    }

            //    nextPoint.PrevValue3 = nextPoint.PrevValue2;
            //    nextPoint.PrevValue2 = nextPoint.PrevValue1;
            //    nextPoint.PrevValue1 = valueRaw;
            //}

            //value = *(float*)&valueRaw;
            //m_lastPoint = nextPoint;
            //return true;
        }

        //private int DecodePointID(uint code, TsscPointMetadata lastPoint)
        //{
        //    if (code == TsscExceptionWords.PointIDXOR8)
        //    {
        //        lastPoint.PrevNextChannelId1 ^= (int)m_reader.ReadBits8();
        //    }
        //    else if (code == TsscExceptionWords.PointIDXOR12)
        //    {
        //        lastPoint.PrevNextChannelId1 ^= (int)m_reader.ReadBits12();
        //    }
        //    else if (code == TsscExceptionWords.PointIDXOR16)
        //    {
        //        lastPoint.PrevNextChannelId1 ^= (int)m_reader.ReadBits16();
        //    }
        //    else if (code == TsscExceptionWords.PointIDXOR24)
        //    {
        //        lastPoint.PrevNextChannelId1 ^= (int)m_reader.ReadBits24();
        //    }
        //    else
        //    {
        //        lastPoint.PrevNextChannelId1 ^= (int)m_reader.ReadBits32();
        //    }


        //    int id = m_lastPoint.PrevNextChannelId1;
        //    var nextPoint = m_points[m_lastPoint.PrevNextChannelId1];
        //    if (nextPoint == null)
        //    {
        //        nextPoint = new TsscPointMetadata(null, m_reader);
        //        m_points[id] = nextPoint;
        //        nextPoint.PrevNextChannelId1 = (ushort)(id + 1);
        //    }
        //    m_lastPoint = nextPoint;
        //    return id;
        //}

        //private CtpTime DecodeTimestamp(uint code)
        //{
        //    long timestamp;
        //    if (code == TsscExceptionWords.TimeDelta1Forward)
        //    {
        //        timestamp = m_prevTimestamp1 + m_prevTimeDelta1;
        //    }
        //    else if (code == TsscExceptionWords.TimeDelta2Forward)
        //    {
        //        timestamp = m_prevTimestamp1 + m_prevTimeDelta2;
        //    }
        //    else if (code == TsscExceptionWords.TimeDelta3Forward)
        //    {
        //        timestamp = m_prevTimestamp1 + m_prevTimeDelta3;
        //    }
        //    else if (code == TsscExceptionWords.TimeDelta4Forward)
        //    {
        //        timestamp = m_prevTimestamp1 + m_prevTimeDelta4;
        //    }
        //    else if (code == TsscExceptionWords.TimeDelta1Reverse)
        //    {
        //        timestamp = m_prevTimestamp1 - m_prevTimeDelta1;
        //    }
        //    else if (code == TsscExceptionWords.TimeDelta2Reverse)
        //    {
        //        timestamp = m_prevTimestamp1 - m_prevTimeDelta2;
        //    }
        //    else if (code == TsscExceptionWords.TimeDelta3Reverse)
        //    {
        //        timestamp = m_prevTimestamp1 - m_prevTimeDelta3;
        //    }
        //    else if (code == TsscExceptionWords.TimeDelta4Reverse)
        //    {
        //        timestamp = m_prevTimestamp1 - m_prevTimeDelta4;
        //    }
        //    else if (code == TsscExceptionWords.Timestamp2)
        //    {
        //        timestamp = m_prevTimestamp2;
        //    }
        //    else
        //    {
        //        timestamp = m_prevTimestamp1 ^ (long)m_reader.Read8BitSegments();
        //    }

        //    //Save the smallest delta time
        //    long minDelta = Math.Abs(m_prevTimestamp1 - timestamp);

        //    if (minDelta < m_prevTimeDelta4 && minDelta != m_prevTimeDelta1 && minDelta != m_prevTimeDelta2 && minDelta != m_prevTimeDelta3)
        //    {
        //        if (minDelta < m_prevTimeDelta1)
        //        {
        //            m_prevTimeDelta4 = m_prevTimeDelta3;
        //            m_prevTimeDelta3 = m_prevTimeDelta2;
        //            m_prevTimeDelta2 = m_prevTimeDelta1;
        //            m_prevTimeDelta1 = minDelta;
        //        }
        //        else if (minDelta < m_prevTimeDelta2)
        //        {
        //            m_prevTimeDelta4 = m_prevTimeDelta3;
        //            m_prevTimeDelta3 = m_prevTimeDelta2;
        //            m_prevTimeDelta2 = minDelta;
        //        }
        //        else if (minDelta < m_prevTimeDelta3)
        //        {
        //            m_prevTimeDelta4 = m_prevTimeDelta3;
        //            m_prevTimeDelta3 = minDelta;
        //        }
        //        else
        //        {
        //            m_prevTimeDelta4 = minDelta;
        //        }
        //    }

        //    m_prevTimestamp2 = m_prevTimestamp1;
        //    m_prevTimestamp1 = timestamp;
        //    return new CtpTime(timestamp);
        //}

        //private ulong DecodeQuality(uint code, TsscPointMetadata nextPoint)
        //{
        //    ulong quality;
        //    if (code == TsscExceptionWords.Quality2)
        //    {
        //        quality = nextPoint.PrevQuality2;
        //    }
        //    else
        //    {
        //        quality = m_reader.Read8BitSegments();
        //    }
        //    nextPoint.PrevQuality2 = nextPoint.PrevQuality1;
        //    nextPoint.PrevQuality1 = quality;
        //    return quality;
        //}

    }

}
