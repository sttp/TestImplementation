using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;
using CTP.Serialization;

namespace Sttp.Codec
{
    [CtpCommand("KeepAlive")]
    public class CommandKeepAlive
        : CtpDocumentObject<CommandKeepAlive>
    {
        public CommandKeepAlive()
        {
        }

        public static explicit operator CommandKeepAlive(CtpDocument obj)
        {
            return ConvertFromDocument(obj);
        }
    }

}
