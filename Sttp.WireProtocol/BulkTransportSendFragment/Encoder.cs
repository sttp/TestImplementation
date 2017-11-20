using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.BulkTransportSendFragment
{
    public class Encoder : BaseEncoder
    {
        protected override CommandCode Code => CommandCode.BulkTransportSendFragment;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }

        public void SendFragment(Guid id, long bytesRemaining, byte[] content, long position, int length)
        {
            BeginCommand();
            Stream.Write(CommandCode.BulkTransportSendFragment);
            Stream.Write(id);
            Stream.Write(bytesRemaining);
            Stream.Write(content, position, length);
            EndCommand();
        }

    }
}
