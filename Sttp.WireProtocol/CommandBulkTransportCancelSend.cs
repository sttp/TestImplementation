using System;

namespace Sttp.WireProtocol
{
    public class CommandBulkTransportCancelSend
    {
        public CommandCode CommandCode => CommandCode.BulkTransportCancelSend;
        public Guid Id;

        public void Load(PayloadReader reader)
        {
            Id = reader.ReadGuid();
        }

    }
}