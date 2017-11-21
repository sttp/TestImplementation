using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec
{
    public class CommandUnknown
    {
        public readonly CommandCode CommandCode;
        public readonly byte[] Data;

        public CommandUnknown(PayloadReader reader)
        {
            CommandCode = reader.Command;
            Data = reader.ReadBytes();
        }
    }
}
