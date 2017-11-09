using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.SendDataPoints
{
    public class Encoder : BaseEncoder
    {
        protected override CommandCode Code => CommandCode.SendDataPoints;
        private SessionDetails m_sessionDetails;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {
            m_sessionDetails = sessionDetails;
        }

        public void SendDataPoints(List<SttpDataPoint> dataPoint)
        {
            BeginCommand();
            Stream.Write(dataPoint.Count);
            int lastPointID = 0;
            long lastTimestamp = 0;
            byte lastTimeQuality = 0;
            byte lastValueQuality = 0;

            foreach (var point in dataPoint)
            {
                //Write the prefix that indicates the type of the PointID, the number of extra fields, and the type of the value
                SttpDataPointLayout layout = default(SttpDataPointLayout);

                bool canUseRuntimeID = point.PointID.RuntimeID >= 0 && point.PointID.RuntimeID < m_sessionDetails.MaxRuntimeIDCache;
                if (!canUseRuntimeID)
                {
                    layout.PointIDType = point.PointID.ValueTypeCode;
                }

                layout.ValueType = point.Value.ValueTypeCode;

                if (point.ExtraFields != null && point.ExtraFields.Length > 0)
                {
                    layout.ExtraFields = true;
                }

                Stream.Write(layout);

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
                if (point.Timestamp.RawValue != lastTimestamp)
                {
                    encodingHeader |= 32;
                }

                if (canUseRuntimeID)
                {
                    int pointIDDelta = point.PointID.RuntimeID ^ lastPointID;
                    encodingHeader |= (byte)(pointIDDelta & 15);
                    pointIDDelta >>= 4;
                    if (pointIDDelta > 0)
                    {
                        encodingHeader |= 16;
                    }
                    Stream.Write(encodingHeader);
                    Stream.WriteInt7Bit(pointIDDelta);

                    lastPointID = point.PointID.RuntimeID;
                }
                else
                {
                    switch (point.PointID.ValueTypeCode)
                    {
                        case SttpPointIDTypeCode.Null:
                            break;
                        case SttpPointIDTypeCode.Guid:
                            Stream.Write(point.PointID.AsGuid);
                            break;
                        case SttpPointIDTypeCode.String:
                            Stream.Write(point.PointID.AsString);
                            break;
                        case SttpPointIDTypeCode.NamedSet:
                            Stream.Write(point.PointID.AsNamedSet);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                if ((byte)point.TimestampQuality != lastTimeQuality)
                {
                    Stream.Write(point.TimestampQuality);
                    lastTimeQuality = (byte)point.TimestampQuality;
                }
                if ((byte)point.ValueQuality != lastValueQuality)
                {
                    Stream.Write(point.ValueQuality);
                    lastValueQuality = (byte)point.ValueQuality;
                }
                if (point.Timestamp.RawValue != lastTimestamp)
                {
                    Stream.WriteInt7Bit(point.Timestamp.RawValue ^ lastTimestamp);
                    lastTimestamp = point.Timestamp.RawValue;
                }

                point.Value.Save(Stream, false);
                if (point.ExtraFields != null)
                {
                    foreach (var extra in point.ExtraFields)
                    {
                        if ((object)extra == null)
                        {
                            Stream.Write(SttpValueTypeCode.Null);
                        }
                        else
                        {
                            extra.Save(Stream, true);
                        }
                    }
                }
            }
            EndCommand();
        }

        private void WriteExtraFieldsFlag(int length)
        {
            while (length > 7)
            {
                Stream.Write((byte)255);
                length -= 7;
            }
            Stream.Write((1 << length) - 1);
        }


    }
}
