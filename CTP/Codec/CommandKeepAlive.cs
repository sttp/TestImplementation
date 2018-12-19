using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;
using CTP.Serialization;

namespace CTP
{
    [CommandName("KeepAlive")]
    public class CtpKeepAlive
        : CommandObject<CtpKeepAlive>
    {
        [CommandField()]
        public DateTime CurrentTime;
        [CommandField()]
        public DateTime LastReceivedTime;

        public CtpKeepAlive(DateTime currentTime, DateTime lastReceivedTime)
        {
            CurrentTime = currentTime;
            LastReceivedTime = lastReceivedTime;
        }

        private CtpKeepAlive()
        {
        }

        public static explicit operator CtpKeepAlive(CtpCommand obj)
        {
            return FromDocument(obj);
        }
    }

}
