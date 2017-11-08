using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.RequestSucceeded
{
    public class Encoder : BaseEncoder
    {
        protected override CommandCode Code => CommandCode.RequestSucceeded;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }

        public void RequestSucceeded(CommandCode commandSucceeded, string reason, string details)
        {
            BeginCommand();
            Stream.Write(commandSucceeded);
            Stream.Write(reason);
            Stream.Write(details);
            EndCommand();
        }


    }
}
