using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sttp.Codec;

namespace Sttp.WireProtocol.SendDataPoints
{
    public class Encoder
    {
        private SessionDetails m_sessionDetails;
        public Action<byte[], int, int> SendData;
        private ByteWriter m_stream;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
        {
            m_sessionDetails = sessionDetails;
            m_stream = new ByteWriter();
        }

        public void SendDataPoints(List<SttpDataPoint> dataPoint)
        {
            //I think it's possible for the default encoding method to be 1 byte overhead. 
            //Then GZIP might be able to shrink it some more, but we'll have to see.

            //2-bits PointID Encoding. (Should be 0 for most use cases)
            //1-bit HasExtraFields. (Rarely true)
            //5-bits SttpValueTypeCode. (Probably just single and int16, but it depends on the protocol)
            //1-bit TimestampQuality Changed. (Very Rare)
            //1-bit ValueQuality Changed. (Very Rare)
            //1-bit Timestamp Changed. (Only likely to occur at the end of a frame, but not extremely likely)
            //x-bits PointID Changed by X. (Can be Changed by 1 if properly sorted inside a frame)
            //n-bits Value. (int16 or single usually, if compared to previous single, might be able to shave a nibble)

            //Optimized Encoding:
            //If all of the following are true,
            //PointID is RuntimeID, 
            //HasExtraFields = false, 
            //ValueTypeCode in (Most common, 2nd Most Common, 3rd Most Common), 
            //TimestampQuality Same,
            //ValueQuality Same,
            //Timestamp Same,
            //2-bits ValueTypeCode (Which of the most common it was, 0 is the else case)
            //6-bits, the XOR of the delta in the pointID. if value = 0, this is the else case.

            //Else:
            //2-bits, Value = 0,
            //2-bits PointID Encoding.
            //1-bit HasExtraFields. 
            //1-bit TimestampQuality Changed.
            //1-bit ValueQuality Changed.
            //1-bit Timestamp Changed.
            //5-bits SttpValueTypeCode.
            //3-bits Reserved for something, not sure yet.

            //PointID (natively encoded)
            //n-bits Value. 

            m_stream.WriteInt7Bit(dataPoint.Count);
            int lastPointID = 0;
            SttpTime lastTimestamp = default(SttpTime);
            byte lastTimeQuality = 0;
            byte lastValueQuality = 0;

            foreach (var point in dataPoint)
            {
                //Write the prefix that indicates the type of the PointID, the number of extra fields, and the type of the value
                SttpDataPointLayout layout = default(SttpDataPointLayout);

                bool canUseRuntimeID = point.DataPointID.RuntimeID >= 0 && point.DataPointID.RuntimeID < m_sessionDetails.MaxRuntimeIDCache;
                if (!canUseRuntimeID)
                {
                    layout.DataPointIDType = point.DataPointID.ValueTypeCode;
                }

                layout.ValueType = point.Value.ValueTypeCode;

                if (point.ExtraFields != null && point.ExtraFields.Length > 0)
                {
                    layout.ExtraFields = true;
                }

                m_stream.Write(layout.RawValue);

                if (layout.ExtraFields)
                {
                    WriteExtraFieldsFlag(point.ExtraFields.Length);
                }

                byte encodingHeader = 0;

                if ((byte)point.TimestampQuality != lastTimeQuality)
                {
                    encodingHeader |= 128;
                }
                if ((byte)point.ValueQuality != lastValueQuality)
                {
                    encodingHeader |= 64;
                }
                if (point.Time != lastTimestamp)
                {
                    encodingHeader |= 32;
                }

                if (canUseRuntimeID)
                {
                    int pointIDDelta = point.DataPointID.RuntimeID ^ lastPointID;
                    encodingHeader |= (byte)(pointIDDelta & 15);
                    pointIDDelta >>= 4;
                    if (pointIDDelta > 0)
                    {
                        encodingHeader |= 16;
                    }
                    m_stream.Write(encodingHeader);
                    m_stream.WriteInt7Bit(pointIDDelta);

                    lastPointID = point.DataPointID.RuntimeID;
                }
                else
                {
                    m_stream.Write(encodingHeader);
                    switch (point.DataPointID.ValueTypeCode)
                    {
                        case SttpDataPointIDTypeCode.Null:
                            break;
                        case SttpDataPointIDTypeCode.Guid:
                            m_stream.Write(point.DataPointID.AsGuid);
                            break;
                        case SttpDataPointIDTypeCode.String:
                            m_stream.Write(point.DataPointID.AsString);
                            break;
                        case SttpDataPointIDTypeCode.SttpMarkup:
                            m_stream.Write(point.DataPointID.AsSttpMarkup);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                if ((byte)point.TimestampQuality != lastTimeQuality)
                {
                    m_stream.Write((byte)point.TimestampQuality);
                    lastTimeQuality = (byte)point.TimestampQuality;
                }
                if ((byte)point.ValueQuality != lastValueQuality)
                {
                    m_stream.Write((byte)point.ValueQuality);
                    lastValueQuality = (byte)point.ValueQuality;
                }
                if (point.Time != lastTimestamp)
                {
                    m_stream.Write(point.Time);
                    lastTimestamp = point.Time;
                }

                point.Value.Save(m_stream);
                if (point.ExtraFields != null)
                {
                    foreach (var extra in point.ExtraFields)
                    {
                        if ((object)extra == null)
                        {
                            m_stream.Write((byte)SttpValueTypeCode.Null);
                        }
                        else
                        {
                            extra.Save(m_stream);
                        }
                    }
                }
            }
        }

        private void WriteExtraFieldsFlag(int length)
        {
            while (length > 7)
            {
                m_stream.Write((byte)255);
                length -= 7;
            }
            m_stream.Write((1 << length) - 1);
        }


    }
}
