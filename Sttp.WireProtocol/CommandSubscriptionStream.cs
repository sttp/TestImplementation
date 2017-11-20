using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.WireProtocol
{
    public class CommandSubscriptionStream
    {
        public CommandCode CommandCode => CommandCode.SubscriptionStream;

        public byte EncodingMethod;
        public byte[] Data;

        public void Fill(PayloadReader reader)
        {
            EncodingMethod = reader.ReadByte();
            Data = reader.ReadBytes();
        }
    }
}
