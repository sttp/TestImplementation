using System;

namespace Sttp.WireProtocol.BulkTransportCancelSend
{
    public class Decoder
    {
        public CommandCode CommandCode => CommandCode.BulkTransportCancelSend;
        public Guid Id;

        public void Load(PayloadReader reader)
        {
            Id = reader.ReadGuid();
        }

    }
}