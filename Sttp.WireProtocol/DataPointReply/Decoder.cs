using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol.DataPointReply
{
    public class Decoder
    {
        public CommandCode CommandCode => CommandCode.DataPointReply;

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
