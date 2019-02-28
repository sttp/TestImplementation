//using System;
//using System.Collections.Generic;
//using System.Runtime.CompilerServices;
//using CTP;
//using Sttp.Codec;

//namespace Sttp.DataPointEncoding
//{
//    public sealed class SimpleEncoder : EncoderBase
//    {
//        const ulong Bits64 = 0xFFFFFFFFFFFFFFFFu;
//        const ulong Bits60 = 0xFFFFFFFFFFFFFFFu;
//        const ulong Bits56 = 0xFFFFFFFFFFFFFFu;
//        const ulong Bits52 = 0xFFFFFFFFFFFFFu;
//        const ulong Bits48 = 0xFFFFFFFFFFFFu;
//        const ulong Bits44 = 0xFFFFFFFFFFFu;
//        const ulong Bits40 = 0xFFFFFFFFFFu;
//        const ulong Bits36 = 0xFFFFFFFFFu;
//        const uint Bits32 = 0xFFFFFFFFu;
//        const uint Bits28 = 0xFFFFFFFu;
//        const uint Bits24 = 0xFFFFFFu;
//        const uint Bits20 = 0xFFFFFu;
//        const uint Bits16 = 0xFFFFu;
//        const uint Bits12 = 0xFFFu;
//        const uint Bits8 = 0xFFu;
//        const uint Bits4 = 0xFu;

//        private CtpObjectWriter m_stream;
//        private BitStreamWriter m_bitStream;
//        /// <summary>
//        /// </summary>
//        private long m_prevTimestamp;
//        private SimpleMetadata m_prevPoint;
//        private SimpleMetadata m_currentPoint;
//        private Dictionary<CtpObject, int> m_pointIDToChannelIDMapping = new Dictionary<CtpObject, int>();
//        private List<SimpleMetadata> m_metadata = new List<SimpleMetadata>();

//        private int m_count;

//        public SimpleEncoder()
//        {
//            m_stream = new CtpObjectWriter();
//            m_bitStream = new BitStreamWriter();
//        }

//        public override int Length => m_stream.Length + m_bitStream.Length;

//        public override void Clear()
//        {
//            m_stream.Clear();
//            m_bitStream.Clear();
//        }

//        public override CtpCommand ToArray()
//        {
//            return new CommandDataStreamSimple(m_stream.ToArray(), m_bitStream.ToArray());
//        }

//        public override void AddDataPoint(SttpDataPoint point)
//        {
//            if (m_prevPoint != null && m_metadata[m_prevPoint.NeighborChannelId].Metadata.DataPointID.Equals(point.Metadata.DataPointID))
//            {
//                //Most of the time, measurements will be sequential, this limits a dictionary lookup
//                m_currentPoint = m_metadata[m_prevPoint.NeighborChannelId];
//            }
//            else
//            {
//                if (!m_pointIDToChannelIDMapping.TryGetValue(point.Metadata.DataPointID, out int channelID))
//                {
//                    DefineChannel(point);
//                    return;
//                }
//                m_currentPoint = m_metadata[channelID];
//            }

//            // ReSharper disable once PossibleNullReferenceException
//            if (m_prevPoint.NeighborChannelId != m_currentPoint.ChannelID)
//            {
//                m_bitStream.WriteBits4((byte)SimpleSymbols.ChannelID);
//                m_bitStream.WriteBits(CompareUInt32.RequiredBits((uint)m_metadata.Count), (uint)m_currentPoint.ChannelID);
//                m_prevPoint.NeighborChannelId = m_currentPoint.ChannelID;
//            }

//            if (m_prevTimestamp != point.Time.Ticks)
//            {
//                m_bitStream.WriteBits4((byte)SimpleSymbols.Timestamp);
//                m_bitStream.Write8BitSegments((ulong)point.Time.Ticks ^ (ulong)m_currentPoint.PrevTime);
//                m_prevTimestamp = point.Time.Ticks;
//            }

//            if (m_currentPoint.PrevQuality != point.Quality)
//            {
//                m_bitStream.WriteBits4((byte)SimpleSymbols.Quality);
//                m_bitStream.Write8BitSegments((ulong)point.Quality);
//            }

//            if (point.Value.ValueTypeCode != m_currentPoint.PrevValue.ValueTypeCode)
//            {
//                m_bitStream.WriteBits4((byte)SimpleSymbols.Type);
//                m_bitStream.WriteBits4((uint)point.Value.ValueTypeCode & 15);
//                m_currentPoint.PrevValue = CtpObject.CreateDefault(point.Value.ValueTypeCode);
//            }

//            if (point.Value.ValueTypeCode == CtpTypeCode.Single)
//            {
//                uint prev = m_currentPoint.PrevValue.UnsafeRawInt32;
//                uint cur = point.Value.UnsafeRawInt32;
//                if (cur == 0)
//                {
//                    m_bitStream.WriteBits4((byte)SimpleSymbols.ValueDefault);
//                }
//                else if (prev == cur)
//                {
//                    m_bitStream.WriteBits4((byte)SimpleSymbols.ValueLast);
//                }
//                else
//                {
//                    m_bitStream.WriteBits1(1);
//                    Write32(prev ^ cur);
//                }
//            }
//            else
//            {
//                AddDataPoint2(point);
//            }

//            m_prevPoint = m_currentPoint;
//            m_currentPoint.Assign(point);
//        }

//        private void DefineChannel(SttpDataPoint point)
//        {
//            int channelID;
//            channelID = m_metadata.Count;
//            m_currentPoint = new SimpleMetadata(channelID, point.Metadata);
//            m_metadata.Add(m_currentPoint);
//            m_pointIDToChannelIDMapping.Add(point.Metadata.DataPointID, channelID);
//            m_currentPoint.Assign(point);
//            if (m_prevPoint != null) //If there is no previous point, the first symbol is unspecified and assumed to be DefineChannel.
//            {
//                m_bitStream.WriteBits4((byte)SimpleSymbols.DefineChannel);
//                m_prevPoint.NeighborChannelId = m_currentPoint.ChannelID;
//            }

//            m_stream.Write(point.Metadata.DataPointID);
//            m_bitStream.Write8BitSegments((ulong)point.Time.Ticks ^ (ulong)m_prevTimestamp);
//            m_bitStream.Write8BitSegments((ulong)point.Quality);
//            m_stream.Write(point.Value);
//            m_prevTimestamp = point.Time.Ticks;
//            m_prevPoint = m_currentPoint;
//        }

//        [MethodImpl(MethodImplOptions.NoInlining)]
//        private void AddDataPoint2(SttpDataPoint point)
//        {
//            switch (point.Value.ValueTypeCode)
//            {
//                case CtpTypeCode.Null:
//                    {
//                        m_bitStream.WriteBits4((byte)SimpleSymbols.ValueDefault);
//                        break;
//                    }
//                case CtpTypeCode.Boolean:
//                    {
//                        if (!point.Value.IsBoolean)
//                        {
//                            m_bitStream.WriteBits4((byte)SimpleSymbols.ValueDefault);
//                        }
//                        else
//                        {
//                            m_bitStream.WriteBits4((byte)SimpleSymbols.ValueLast);
//                        }
//                        break;
//                    }
//                case CtpTypeCode.Integer:
//                case CtpTypeCode.Double:
//                case CtpTypeCode.CtpTime:
//                    {
//                        ulong prev = m_currentPoint.PrevValue.UnsafeRawInt64;
//                        ulong cur = point.Value.UnsafeRawInt64;
//                        if (cur == 0)
//                        {
//                            m_bitStream.WriteBits4((byte)SimpleSymbols.ValueDefault);
//                        }
//                        else if (prev == cur)
//                        {
//                            m_bitStream.WriteBits4((byte)SimpleSymbols.ValueLast);
//                        }
//                        else
//                        {
//                            m_bitStream.WriteBits1(1);
//                            Write64(prev ^ cur);
//                        }
//                        break;
//                    }
//                case CtpTypeCode.Numeric:
//                case CtpTypeCode.Guid:
//                case CtpTypeCode.String:
//                case CtpTypeCode.CtpCommand:
//                case CtpTypeCode.CtpBuffer:
//                    if (point.Value.IsDefault)
//                    {
//                        m_bitStream.WriteBits4((byte)SimpleSymbols.ValueDefault);
//                    }
//                    else if (m_currentPoint.PrevValue.Equals(point.Value))
//                    {
//                        m_bitStream.WriteBits4((byte)SimpleSymbols.ValueLast);
//                    }
//                    else
//                    {
//                        m_bitStream.WriteBits1(1);
//                        m_stream.Write(point.Value);
//                    }
//                    break;
//                case CtpTypeCode.Single:
//                //Single is handled in AddDataPoint
//                default:
//                    throw new ArgumentOutOfRangeException();
//            }
//        }

//        private void Write32(uint bitsChanged)
//        {
//            if (bitsChanged <= Bits4)
//            {
//                m_bitStream.WriteBits3(0);
//                m_bitStream.WriteBits4(bitsChanged);
//            }
//            else if (bitsChanged <= Bits8)
//            {
//                m_bitStream.WriteBits3(1);
//                m_bitStream.WriteBits8(bitsChanged);
//            }
//            else if (bitsChanged <= Bits12)
//            {
//                m_bitStream.WriteBits3(2);
//                m_bitStream.WriteBits12(bitsChanged);
//            }
//            else if (bitsChanged <= Bits16)
//            {
//                m_bitStream.WriteBits3(3);
//                m_bitStream.WriteBits16(bitsChanged);
//            }
//            else if (bitsChanged <= Bits20)
//            {
//                m_bitStream.WriteBits3(4);
//                m_bitStream.WriteBits20(bitsChanged);
//            }
//            else if (bitsChanged <= Bits24)
//            {
//                m_bitStream.WriteBits3(5);
//                m_bitStream.WriteBits24(bitsChanged);
//            }
//            else if (bitsChanged <= Bits28)
//            {
//                m_bitStream.WriteBits3(6);
//                m_bitStream.WriteBits28(bitsChanged);
//            }
//            else
//            {
//                m_bitStream.WriteBits3(7);
//                m_bitStream.WriteBits32(bitsChanged);
//            }
//        }

//        private void Write64(ulong bitsChanged)
//        {
//            if (bitsChanged <= Bits4)
//            {
//                m_bitStream.WriteBits4(0);
//                m_bitStream.WriteBits4((uint)bitsChanged);
//            }
//            else if (bitsChanged <= Bits8)
//            {
//                m_bitStream.WriteBits4(1);
//                m_bitStream.WriteBits8((uint)bitsChanged);
//            }
//            else if (bitsChanged <= Bits12)
//            {
//                m_bitStream.WriteBits4(2);
//                m_bitStream.WriteBits12((uint)bitsChanged);
//            }
//            else if (bitsChanged <= Bits16)
//            {
//                m_bitStream.WriteBits4(3);
//                m_bitStream.WriteBits16((uint)bitsChanged);
//            }
//            else if (bitsChanged <= Bits20)
//            {
//                m_bitStream.WriteBits4(4);
//                m_bitStream.WriteBits20((uint)bitsChanged);
//            }
//            else if (bitsChanged <= Bits24)
//            {
//                m_bitStream.WriteBits4(5);
//                m_bitStream.WriteBits24((uint)bitsChanged);
//            }
//            else if (bitsChanged <= Bits28)
//            {
//                m_bitStream.WriteBits4(6);
//                m_bitStream.WriteBits28((uint)bitsChanged);
//            }
//            else if (bitsChanged <= Bits32)
//            {
//                m_bitStream.WriteBits4(7);
//                m_bitStream.WriteBits32((uint)bitsChanged);
//            }
//            else if (bitsChanged <= Bits36)
//            {
//                m_bitStream.WriteBits4(8);
//                m_bitStream.WriteBits(36, bitsChanged);
//            }
//            else if (bitsChanged <= Bits40)
//            {
//                m_bitStream.WriteBits4(9);
//                m_bitStream.WriteBits(40, bitsChanged);
//            }
//            else if (bitsChanged <= Bits44)
//            {
//                m_bitStream.WriteBits4(10);
//                m_bitStream.WriteBits(44, bitsChanged);
//            }
//            else if (bitsChanged <= Bits48)
//            {
//                m_bitStream.WriteBits4(11);
//                m_bitStream.WriteBits(48, bitsChanged);
//            }
//            else if (bitsChanged <= Bits52)
//            {
//                m_bitStream.WriteBits4(12);
//                m_bitStream.WriteBits(52, bitsChanged);
//            }
//            else if (bitsChanged <= Bits56)
//            {
//                m_bitStream.WriteBits4(13);
//                m_bitStream.WriteBits(56, bitsChanged);
//            }
//            else if (bitsChanged <= Bits60)
//            {
//                m_bitStream.WriteBits4(14);
//                m_bitStream.WriteBits(60, bitsChanged);
//            }
//            else
//            {
//                m_bitStream.WriteBits4(15);
//                m_bitStream.WriteBits(64, bitsChanged);
//            }
//        }

//    }
//}
