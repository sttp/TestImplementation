using System;

namespace Sttp.WireProtocol
{
    public class CommandBulkTransportBeginSend
    {
        public CommandCode CommandCode => CommandCode.BulkTransportBeginSend;

        public byte EncodingMethod;
        public bool IsEndOfResponse;
        public byte[] Data;
        public Guid RequestID;

        public void Fill(PayloadReader reader)
        {
            RequestID = reader.ReadGuid();
            IsEndOfResponse = reader.ReadBoolean();
            EncodingMethod = reader.ReadByte();
            Data = reader.ReadBytes();
        }

    }
}