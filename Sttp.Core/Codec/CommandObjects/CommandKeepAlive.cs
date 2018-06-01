using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [DocumentName("KeepAlive")]
    public class CommandKeepAlive
        : DocumentObject<CommandKeepAlive>
    {
        public CommandKeepAlive()
        {
        }

        public static explicit operator CommandKeepAlive(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }

}
