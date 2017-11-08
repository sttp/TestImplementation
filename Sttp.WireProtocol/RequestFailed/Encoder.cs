using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.RequestFailed
{
    public class Encoder : BaseEncoder
    {
        public override CommandCode Code => CommandCode.GetMetadataSchema;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }

        public void GetMetadataSchema(CommandCode failedCommand, string reason, string details)
        {
            BeginCommand();
            Stream.Write(failedCommand);
            Stream.Write(reason);
            Stream.Write(details);
            EndCommand();
        }


    }
}
