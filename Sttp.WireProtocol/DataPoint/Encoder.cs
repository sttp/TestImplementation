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
        private List<DataPointWire> m_newPointKeys;
        private List<DataPointWire> m_newPoints;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
        {
            m_sendPacket = sendPacket;
            m_newPointKeys = new List<DataPointWire>();
            m_newPoints = new List<DataPointWire>();
            m_stream = new PacketWriter(sessionDetails);
        }

        public void RegisterDataPoint(DataPointWire dataPointKey)
        {
            m_newPointKeys.Add(dataPointKey);
        }

        public void SendDataPoint(DataPointWire point)
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
                    m_stream.Write(point.Time.Seconds);
                    m_stream.Write(point.Time.Fraction);
                    m_stream.Write(point.ValueLength);
                    m_stream.Write((byte)point.TimeQualityFlags);
                    m_stream.Write((byte)point.DataQualityFlags);
                    m_stream.Write(point.BulkDataValueID);
                    m_stream.Write(point.Value);
                }
                m_stream.EndCommand(m_sendPacket);
            }

        }

    }
}
