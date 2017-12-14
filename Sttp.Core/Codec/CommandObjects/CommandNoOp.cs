using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sttp.Codec
{
    public class CommandNoOp
    {
        public CommandCode CommandCode => CommandCode.NoOp;
        public readonly bool ShouldEcho;

        public CommandNoOp(PayloadReader reader)
        {
            ShouldEcho = reader.ReadBoolean();
        }
    }
}
