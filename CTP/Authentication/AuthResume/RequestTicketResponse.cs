using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTP.Net;

namespace CTP.Authentication
{
    /// <summary>
    /// Requests a ticket that can be used to resume a session.
    /// </summary>
    [DocumentName("RequestTicketResponse")]
    public class RequestTicketResponse
        : DocumentObject<RequestTicketResponse>
    {
        [DocumentField()] public byte[] Ticket { get; private set; }

        public RequestTicketResponse(byte[] ticket)
        {
            Ticket = ticket;
        }

        private RequestTicketResponse()
        {

        }

        public static explicit operator RequestTicketResponse(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}
