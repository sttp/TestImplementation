using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec
{
    public class CommandSubscriptionStream
    {
        public CommandCode CommandCode => CommandCode.SubscriptionStream;

        public readonly byte EncodingMethod;
        public readonly byte[] Data;

        public CommandSubscriptionStream(PayloadReader reader)
        {
            EncodingMethod = reader.ReadByte();
            Data = reader.ReadBytes();
        }
    }
}
