using Sttp.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sttp.WireProtocol.DataPoint.TSSC;

namespace Sttp.WireProtocol.SendDataPoints
{
    public class Encoder
    {
        private Action<byte[], int, int> m_sendPacket;
        private PacketWriter m_stream;

        private List<SttpPointID> m_pointIDsToRegister;
        private List<SttpDataPoint> m_sttpCompatibleDataPoints;
        private List<SttpDataPoint> m_newPoints;
        private BitArray m_hasRegistered;
        private SessionDetails m_sessionDetails;
        private TsscEncoder m_encoder = new TsscEncoder();

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
        {
            m_sendPacket = sendPacket;
            m_pointIDsToRegister = new List<SttpPointID>();
            m_newPoints = new List<SttpDataPoint>();
            m_stream = new PacketWriter(sessionDetails);
            m_sessionDetails = sessionDetails;
            m_hasRegistered = new BitArray(m_sessionDetails.MaxRuntimeIDCache);
        }

        public void SendDataPoint(SttpDataPoint point)
        {
            bool canUsePointID = point.PointID.RuntimeID >= 0 && point.PointID.RuntimeID < m_sessionDetails.MaxRuntimeIDCache;

            if (canUsePointID && !m_hasRegistered[point.PointID.RuntimeID])
            {
                m_hasRegistered[point.PointID.RuntimeID] = true;
                m_pointIDsToRegister.Add(point.PointID);
            }
            if (canUsePointID && (point.ExtraFields == null || point.ExtraFields.Length == 0) && SupportsTssc(point.Value.ValueTypeCode))
            {
                m_sttpCompatibleDataPoints.Add(point);
            }
            else
            {
                m_newPoints.Add(point);
            }
        }

        private bool SupportsTssc(SttpValueTypeCode value)
        {
            switch (value)
            {
                case SttpValueTypeCode.Single:
                    return true;
            }
            return false;
        }

        public void Send()
        {
            if (m_pointIDsToRegister.Count > 0)
            {
                m_stream.BeginCommand(CommandCode.RegisterDataPointRuntimeIdentifier);
                m_stream.Write(m_pointIDsToRegister.Count);
                foreach (var key in m_pointIDsToRegister)
                {
                    //key.Write(m_stream);
                }
                m_stream.EndCommand(m_sendPacket);
            }

            if (m_sttpCompatibleDataPoints.Count > 0)
            {
                m_stream.BeginCommand(CommandCode.SendDataPoints);
                m_stream.Write(m_sttpCompatibleDataPoints.Count);
                foreach (var point in m_sttpCompatibleDataPoints)
                {
                    m_encoder.TryAddMeasurement((ushort)point.PointID.RuntimeID, point.Timestamp.RawValue, (byte)point.TimestampQuality, point.Value.AsSingle);
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
