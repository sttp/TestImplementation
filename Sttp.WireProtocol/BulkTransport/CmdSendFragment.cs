using System;
using System.Collections.Generic;

namespace Sttp.WireProtocol.BulkTransport
{
    public class CmdSendFragment : ICmd
    {
        public SubCommand SubCommand => SubCommand.SendFragment;

        public Guid Id;
        public long BytesRemaining;
        public byte[] Content;

        public void Load(PacketReader reader)
        {
            Id = reader.ReadGuid();
            BytesRemaining = reader.ReadInt64();
            Content = reader.ReadBytes();
        }
    }
}