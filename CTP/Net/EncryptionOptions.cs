using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace CTP.Net
{
    public class EncryptionOptions
    {
        public X509Certificate ServerCertificate;
        public bool EnableSSL => ServerCertificate != null;

        public EncryptionOptions(X509Certificate localCertificate)
        {
            ServerCertificate = localCertificate;
        }
    }
}