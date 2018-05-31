using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CtpSerializable("KeepAlive")]
    public class CommandKeepAlive
    {
        public CommandKeepAlive()
        {
        }
    }
}
