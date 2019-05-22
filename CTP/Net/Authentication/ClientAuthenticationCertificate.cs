using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace CTP.Net
{
    public class ClientAuthenticationCertificate : IClientAuthentication, IClientHandshake
    {
        private string m_spn;
        private X509Certificate2 m_trustedClientCertificate;
        private string m_loginName;
        private List<string> m_grantedRoles;
        private List<string> m_deniedRoles;
        private CertificateTrust m_trustedRemotes;

        public ClientAuthenticationCertificate(string spn, X509Certificate2 trustedClientCertificate, string trustedCertificatesPath, string loginName, List<string> grantedRoles, List<string> deniedRoles)
        {
            m_spn = spn;
            m_trustedClientCertificate = trustedClientCertificate;
            m_loginName = loginName;
            m_trustedRemotes = new CertificateTrust(trustedCertificatesPath);
            m_grantedRoles = grantedRoles;
            m_deniedRoles = deniedRoles;
        }

        public bool UseSSL => true;

        public bool IsEphemeralCertificate => false;

        public X509Certificate2 GetCertificate()
        {
            return m_trustedClientCertificate;
        }

        public X509CertificateCollection GetCertificateCollection()
        {
            return new X509CertificateCollection(new X509Certificate[] { m_trustedClientCertificate });
        }

        public ClientDone GetClientDone()
        {
            return new ClientDone(m_spn, m_loginName, m_grantedRoles, m_deniedRoles);
        }

        public CertificateProof GetCertificateProof()
        {
            throw new InvalidOperationException();
        }

        public bool IsCertificateTrusted(X509Certificate channelCertificate)
        {
            return m_trustedRemotes.IsCertificateTrusted(channelCertificate);
        }

        public bool IsCertificateTrusted(X509Certificate channelCertificate, CertificateProof proof)
        {
            return m_trustedRemotes.IsCertificateTrusted(channelCertificate, proof);
        }

        public IClientHandshake StartHandshake()
        {
            return this;
        }

        public void Dispose()
        {
           
        }
    }
}