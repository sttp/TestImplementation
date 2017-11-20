using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.SubscriptionStream
{
    public class Encoder : BaseEncoder
    {
        protected override CommandCode Code => CommandCode.SubscriptionStream;

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
