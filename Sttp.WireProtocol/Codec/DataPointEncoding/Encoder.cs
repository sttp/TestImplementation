//using Sttp.IO;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using Sttp.Codec;
//using Sttp.Codec.DataPoint.TSSC;

//namespace Sttp.WireProtocol.DataPointEncoding
//{
//    public class Encoder
//    {
//        public Action<byte[], int, int> SendData;

//        private List<SttpDataPointID> m_pointIDsToRegister;
//        private List<SttpDataPoint> m_sttpCompatibleDataPoints;
//        private List<SttpDataPoint> m_newPoints;
//        private BitArray m_hasRegistered;
//        private SessionDetails m_sessionDetails;
//        private TsscEncoder m_encoder = new TsscEncoder();
//        private ByteWriter m_stream;


//        public Encoder(Action<byte[], int, int> sendData, SessionDetails sessionDetails)
//        {
//            SendData = sendData;
//            m_stream = new ByteWriter();
//            m_pointIDsToRegister = new List<SttpDataPointID>();
//            m_newPoints = new List<SttpDataPoint>();
//            m_sessionDetails = sessionDetails;
//            m_hasRegistered = new BitArray(m_sessionDetails.MaxRuntimeIDCache);
//        }

//        public void SendDataPoint(SttpDataPoint point)
//        {
//            bool canUsePointID = point.DataPointID.RuntimeID >= 0 && point.DataPointID.RuntimeID < m_sessionDetails.MaxRuntimeIDCache;

//            if (canUsePointID && !m_hasRegistered[point.DataPointID.RuntimeID])
//            {
//                m_hasRegistered[point.DataPointID.RuntimeID] = true;
//                m_pointIDsToRegister.Add(point.DataPointID);
//            }
//            if (canUsePointID && (point.ExtraFields == null || point.ExtraFields.Length == 0) && SupportsTssc(point.Value.ValueTypeCode))
//            {
//                m_sttpCompatibleDataPoints.Add(point);
//            }
//            else
//            {
//                m_newPoints.Add(point);
//            }
//        }

//        private bool SupportsTssc(SttpValueTypeCode value)
//        {
//            switch (value)
//            {
//                case SttpValueTypeCode.Single:
//                    return true;
//            }
//            return false;
//        }

//        public void Send()
//        {
//            if (m_pointIDsToRegister.Count > 0)
//            {
//                m_stream.BeginCommand(CommandCode.MapRuntimeIDs);
//                m_stream.Write(m_pointIDsToRegister.Count);
//                foreach (var key in m_pointIDsToRegister)
//                {
//                    //key.Write(m_stream);
//                }
//                m_stream.EndCommand(m_sendPacket);
//            }

//            if (m_sttpCompatibleDataPoints.Count > 0)
//            {
//                m_stream.BeginCommand(CommandCode.MetadataVersionNotCompatible);
//                m_stream.Write(m_sttpCompatibleDataPoints.Count);
//                foreach (var point in m_sttpCompatibleDataPoints)
//                {
//                    m_encoder.TryAddMeasurement((ushort)point.DataPointID.RuntimeID, point.Timestamp.RawValue, (byte)point.TimestampQuality, point.Value.AsSingle);
//                }
//                m_stream.EndCommand(m_sendPacket);
//            }

//            if (m_newPoints.Count > 0)
//            {
//                m_stream.BeginCommand(CommandCode.SendDataPoints);
//                m_stream.Write(m_newPoints.Count);
//                foreach (var point in m_newPoints)
//                {
//                    point.Write(m_stream);
//                }
//                m_stream.EndCommand(m_sendPacket);
//            }


//        }

//    }
//}
