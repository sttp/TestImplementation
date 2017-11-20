using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.BulkTransportCancelSend
{
    public class Encoder : BaseEncoder
    {
        protected override CommandCode Code => CommandCode.BulkTransportCancelSend;

        public Encoder(CommandEncoder commandEncoder, SessionDetails sessionDetails)
            : base(commandEncoder, sessionDetails)
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
