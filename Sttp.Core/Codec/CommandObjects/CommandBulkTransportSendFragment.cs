using System;
using System.Collections.Generic;

namespace Sttp.Codec
{
    public class CommandBulkTransportSendFragment
    {
        public CommandCode CommandCode => CommandCode.BulkTransportSendFragment;

        public readonly Guid Id;
        public readonly long BytesRemaining;
        public readonly byte[] Content;

        public CommandBulkTransportSendFragment(PayloadReader reader)
        {
            Id = reader.ReadGuid();
            BytesRemaining = reader.ReadInt64();
            Content = reader.ReadBytes();
        }
    }
}