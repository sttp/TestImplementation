using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.CompletedSendingDataPoints
{
    public class Encoder : BaseEncoder
    {
        protected override CommandCode Code => CommandCode.CompletedSendingDataPoints;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }

        public void CompletedSendingDataPoints()
        {
            BeginCommand();
          
            EndCommand();
        }
     

    }
}
