using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace CTP.Net
{
    public class TicketSourceCertificate : ITicketSource
    {
        private X509Certificate2 m_certificate;
        private List<X509Certificate2> m_remoteCertificates;
        private string m_loginName;
        private List<string> m_roles;

        public TicketSourceCertificate(X509Certificate2 localCertificate, List<X509Certificate2> remoteCertificates, string loginName, List<string> roles)
        {
            m_remoteCertificates = remoteCertificates;
            m_certificate = localCertificate;
            m_loginName = loginName;
            m_roles = roles;
        }

        public Auth GetTicket()
        {
            var t = new Ticket(DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow.AddMinutes(1), m_loginName, m_roles, m_remoteCertificates.Select(x => x.Thumbprint).ToList());
            var auth = new Auth(t, m_certificate);
            return auth;
        }
    }
}