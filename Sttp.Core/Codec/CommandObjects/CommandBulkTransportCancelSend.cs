using System;

namespace Sttp.Codec
{
    public class CommandBulkTransportCancelSend
    {
        public CommandCode CommandCode => CommandCode.BulkTransportCancelSend;
        public readonly Guid Id;

        public CommandBulkTransportCancelSend(PayloadReader reader)
        {
            Id = reader.ReadGuid();
        }

    }
}