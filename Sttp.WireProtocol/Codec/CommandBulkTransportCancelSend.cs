using System;

namespace Sttp.Codec
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