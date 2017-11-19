using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.NegotiateSession
{
    public class Encoder : BaseEncoder
    {
        protected override CommandCode Code => CommandCode.NegotiateSession;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }

        public void Encode(SttpNamedSet connectionString)
        {
            BeginCommand();
            Stream.Write(connectionString);
            EndCommand();
        }


    }
}
