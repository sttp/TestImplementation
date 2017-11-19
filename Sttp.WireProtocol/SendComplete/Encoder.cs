using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.SendComplete
{
    public class Encoder : BaseEncoder
    {
        protected override CommandCode Code => CommandCode.SendComplete;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }

        public void SendComplete()
        {
            BeginCommand();
          
            EndCommand();
        }
     

    }
}
