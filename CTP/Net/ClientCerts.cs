using System.Security.Cryptography.X509Certificates;

namespace CTP.Net
{
    public class ClientCerts
    {
        public X509Certificate2 Certificate;
        public CtpClientCert ClientCert;

        public ClientCerts(CtpClientCert clientCert, X509Certificate2 certificate)
        {
            ClientCert = clientCert;
            Certificate = certificate;
        }
    }
}