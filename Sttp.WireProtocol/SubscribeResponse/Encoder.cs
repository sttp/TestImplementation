using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.SubscribeResponse
{
    public class Encoder : BaseEncoder
    {
        public override CommandCode Code => CommandCode.GetMetadata;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }
       
        public void RequestFailed(string reason, string details)
        {
            Stream.Write(SubCommand.RequestFailed);
            Stream.Write(reason);
            Stream.Write(details);
        }

        public void RequestSuccess(string reason, string details)
        {
            Stream.Write(SubCommand.RequestSuccess);
            Stream.Write(reason);
            Stream.Write(details);
        }

        


    }
}
