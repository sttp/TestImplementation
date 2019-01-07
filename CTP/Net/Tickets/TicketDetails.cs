using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CTP.Net
{
    public class TicketDetails
    {
        public Auth Auth;
        public List<string> ValidServerSidePublicKeys = new List<string>();
        public X509Certificate2 ClientCertificate;
    }
}
