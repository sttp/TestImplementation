using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.DataPointReply
{
    public class Encoder : BaseEncoder
    {
        protected override CommandCode Code => CommandCode.DataPointReply;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }

        public void SendDataPointsCustom(Guid requestID, bool isEndOfResponse, byte encodingMethod, byte[] buffer)
        {
            BeginCommand();
            Stream.Write(requestID);
            Stream.Write(isEndOfResponse);
            Stream.Write(encodingMethod);
            Stream.Write(buffer);
            EndCommand();
        }


    }
}
