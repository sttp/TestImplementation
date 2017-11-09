using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.SendDataPointsCustom
{
    public class Encoder : BaseEncoder
    {
        protected override CommandCode Code => CommandCode.SendDataPointsCustom;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }

        public void SendDataPointsCustom(byte encodingMethod, byte[] buffer)
        {
            BeginCommand();
            Stream.Write(encodingMethod);
            Stream.Write(buffer);
            EndCommand();
        }


    }
}
