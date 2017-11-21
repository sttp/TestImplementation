using System;

namespace Sttp.Codec
{
    public class CommandBulkTransportBeginSend
    {
        public CommandCode CommandCode => CommandCode.BulkTransportBeginSend;

        public readonly byte EncodingMethod;
        public readonly bool IsEndOfResponse;
        public readonly byte[] Data;
        public readonly Guid RequestID;

        public CommandBulkTransportBeginSend(PayloadReader reader)
        {
            RequestID = reader.ReadGuid();
            IsEndOfResponse = reader.ReadBoolean();
            EncodingMethod = reader.ReadByte();
            Data = reader.ReadBytes();
        }

    }
}