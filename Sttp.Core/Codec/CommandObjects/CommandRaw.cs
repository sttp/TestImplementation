using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec
{
    public class CommandRaw
    {
        public readonly byte RawCommandCode;
        public readonly byte[] Payload;

        public CommandRaw(byte rawCommandCode, byte[] payload)
        {
            RawCommandCode = rawCommandCode;
            Payload = payload;
        }
    }
}
