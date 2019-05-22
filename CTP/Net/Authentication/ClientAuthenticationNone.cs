using System;
using System.Security.Cryptography.X509Certificates;

namespace CTP.Net
{
    public class ClientAuthenticationNone : IClientAuthentication, IClientHandshake
    {
        public bool UseSSL => false;

        public bool IsEphemeralCertificate => throw new NotImplementedException();

        public ClientAuthenticationNone()
        {

        }

        public X509Certificate2 GetCertificate()
        {
            throw new NotImplementedException();
        }

        public X509CertificateCollection GetCertificateCollection()
        {
            throw new NotImplementedException();
        }

        public ClientDone GetClientDone()
        {
            throw new NotImplementedException();
        }

        public CertificateProof GetCertificateProof()
        {
            throw new NotImplementedException();
        }

        public bool IsCertificateTrusted(X509Certificate channelCertificate)
        {
            throw new NotImplementedException();
        }

        public bool IsCertificateTrusted(X509Certificate channelCertificate, CertificateProof proof)
        {
            throw new NotImplementedException();
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