using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.BulkTransportCancelSend
{
    public class Encoder : BaseEncoder
    {
        protected override CommandCode Code => CommandCode.BulkTransportCancelSend;

        public Encoder(Action<byte[], int, int> sendPacket, SessionDetails sessionDetails)
            : base(sendPacket, sessionDetails)
        {

        }
        
        public void CancelSend(Guid id)
        {
            BeginCommand();
            Stream.Write(CommandCode.BulkTransportCancelSend);
            Stream.Write(id);
            EndCommand();
        }

    }
}
