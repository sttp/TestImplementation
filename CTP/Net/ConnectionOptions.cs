using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CTP.Net
{
    public enum EncryptionMode
    {
        None,
        SSL,
    }

    public enum ServerAuthenticationMode
    {
        None,
        TrustCertificate,
        TrustCA,
        TrustedSharedSecret
    }

    public enum ClientAuthenticationMode
    {
        None,
        TrustCertificate,
        TrustCA,
        TrustedSharedSecret
    }

    public class ConnectionOptions
    {
        public X509Certificate2 ServerCertificate;
        public X509CertificateCollection TrustedCertificates;
        public bool SslCheckCertificateRevocation;

        public EncryptionMode Encryption;
        public bool EncryptAsServer;

        public bool AuthenticateAsServer;
        public ServerAuthenticationMode ServerAuthentication;

        public string ServerName;
        public ClientAuthenticationMode ClientAuthentication;


        public bool IsServerTrusted;

    }
}
