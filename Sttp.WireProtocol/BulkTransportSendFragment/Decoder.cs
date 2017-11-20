using System;
using System.Collections.Generic;

namespace Sttp.WireProtocol.BulkTransportSendFragment
{
    public class Decoder
    {
        public CommandCode CommandCode => CommandCode.BulkTransportSendFragment;

        public Guid Id;
        public long BytesRemaining;
        public byte[] Content;

        public void Load(PayloadReader reader)
        {
            Id = reader.ReadGuid();
            BytesRemaining = reader.ReadInt64();
            Content = reader.ReadBytes();
        }
    }
}