using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace CTP.Net
{
    public class CertificateUserMapping
    {
        public List<string> NameRecord;
        public X509CertificateCollection TrustedRootCertificates;

        public string LoginName;
        public string[] Roles;
    }
}