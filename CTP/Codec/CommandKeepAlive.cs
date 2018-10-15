using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTP;
using CTP.Serialization;

namespace CTP
{
    [DocumentName("KeepAlive")]
    public class CtpKeepAlive
        : DocumentObject<CtpKeepAlive>
    {
        [DocumentField()]
        public DateTime CurrentTime;
        [DocumentField()]
        public DateTime LastReceivedTime;

        public CtpKeepAlive(DateTime currentTime, DateTime lastReceivedTime)
        {
            CurrentTime = currentTime;
            LastReceivedTime = lastReceivedTime;
        }

        private CtpKeepAlive()
        {
        }

        public static explicit operator CtpKeepAlive(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }

}
