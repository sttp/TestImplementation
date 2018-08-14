using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTP.Authentication
{
    /// <summary>
    /// Requests a ticket that can be used to resume a session.
    /// </summary>
    [DocumentName("RequestTicket")]
    public class RequestTicket
        : DocumentObject<RequestTicket>
    {
        public RequestTicket()
        {

        }

        public static explicit operator RequestTicket(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}
