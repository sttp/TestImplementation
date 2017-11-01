using Sttp.IO;
using System;
using System.Collections.Generic;
using System.IO;

namespace Sttp.WireProtocol.SendDataPoints
{
    public class Encoder
    {
        private Action<byte[], int, int> m_sendPacket;
        private PacketWriter m_stream;
        private List<SttpDataPoint> m_newPointKeys;
        private List<SttpDataPoint> m_newPoints;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
        {
            m_sendPacket = sendPacket;
            m_newPointKeys = new List<SttpDataPoint>();
            m_newPoints = new List<SttpDataPoint>();
            m_stream = new PacketWriter(sessionDetails);
        }

        public void RegisterDataPoint(SttpDataPoint dataPointKey)
        {
            m_newPointKeys.Add(dataPointKey);
        }

        public void SendDataPoint(SttpDataPoint point)
        {
            m_newPoints.Add(point);
        }

        public void Send()
        {
            if (m_newPoints.Count > 0)
            {
                m_stream.BeginCommand(CommandCode.RegisterDataPoint);
                m_stream.Write(m_newPointKeys.Count);
                foreach (var key in m_newPointKeys)
                {
                    m_stream.Write(key.DataPointID);
                    //m_stream.Write(key.Type);
                    //m_stream.Write(key.Flags);
                }
                m_stream.EndCommand(m_sendPacket);
            }

            if (m_newPoints.Count > 0)
            {
                m_stream.BeginCommand(CommandCode.SendDataPoints);
                m_stream.Write(m_newPointKeys.Count);
                foreach (var point in m_newPoints)
                {
                    m_stream.Write(point.DataPointID);
                    m_stream.Write(point.LongTimestamp.Ticks);
                    m_stream.Write(point.LongTimestamp.ExtraPrecision);
                    m_stream.Write(point.ToByteArray());
                    m_stream.Write(point.TimeQuality);
                    m_stream.Write(point.ValueQuality);
                }
                m_stream.EndCommand(m_sendPacket);
            }

        }

    }
}
