using System;

namespace Sttp.WireProtocol.BulkTransport
{
    public class CmdCancelSend : ICmd
    {
        public SubCommand SubCommand => SubCommand.CancelSend;
        public Guid Id;

        public void Load(PacketReader reader)
        {
            Id = reader.ReadGuid();
        }

    }
}