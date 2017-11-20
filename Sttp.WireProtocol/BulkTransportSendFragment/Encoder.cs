using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.BulkTransportSendFragment
{
    public class Encoder : BaseEncoder
    {
        protected override CommandCode Code => CommandCode.BulkTransportSendFragment;

        public Encoder(CommandEncoder commandEncoder, SessionDetails sessionDetails)
            : base(commandEncoder, sessionDetails)
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
