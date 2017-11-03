using Sttp.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Sttp.WireProtocol.SendDataPoints
{
    public class Encoder
    {
        private Action<byte[], int, int> m_sendPacket;
        private PacketWriter m_stream;

        private List<SttpPointInfo> m_newPointIDs;
        private List<SttpDataPoint> m_newPoints;

        private HashSet<long> m_hasRegistered = new HashSet<long>(); //ToDo: Rethink how to do this. This could be a big list. Maybe BitArray, but what about session defined pointIDs.


        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
        {
            m_sendPacket = sendPacket;
            m_newPointIDs = new List<SttpPointInfo>();
            m_newPoints = new List<SttpDataPoint>();
            m_stream = new PacketWriter(sessionDetails);
        }

        public void SendDataPoint(SttpDataPoint point)
        {
            if (!m_hasRegistered.Contains(point.PointInfo.RuntimeID))
            {
                m_hasRegistered.Add(point.PointInfo.RuntimeID);
                m_newPointIDs.Add(point.PointInfo);
            }
        }

        public void Send()
        {
            if (m_newPoints.Count > 0)
            {
                m_stream.BeginCommand(CommandCode.RegisterDataPoint);
                m_stream.Write(m_newPointIDs.Count);
                foreach (var key in m_newPointIDs)
                {
                    key.Write(m_stream);
                }
                m_stream.EndCommand(m_sendPacket);
            }

            if (m_newPoints.Count > 0)
            {
                m_stream.BeginCommand(CommandCode.SendDataPoints);
                m_stream.Write(m_newPoints.Count);
                foreach (var point in m_newPoints)
                {
                    point.Write(m_stream);
                }
                m_stream.EndCommand(m_sendPacket);
            }

        }

    }
}
