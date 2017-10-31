using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sttp.WireProtocol.SendDataPoints;

namespace Sttp.WireProtocol.RegisterDataPoint
{
    public class Encoder : BaseEncoder
    {
        public override CommandCode Code => CommandCode.GetMetadata;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }

        public void NewPoint(uint dataPointID, uint runtimeID, ValueType type, StateFlags flags)
        {
            m_stream.Write(SubCommand.NewPoint);
            m_stream.Write(dataPointID);
            m_stream.Write(runtimeID);
            m_stream.Write(type);
            m_stream.Write(flags);
        }

    }
}
