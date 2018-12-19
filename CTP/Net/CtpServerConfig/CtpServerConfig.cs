using System.Collections.Generic;

namespace CTP.Net
{
    [CommandName("CtpServerConfig")]
    public class CtpServerConfig
        : CommandObject<CtpServerConfig>
    {
        [CommandField()]
        public List<CtpAccount> Accounts { get; set; }

        [CommandField()]
        public List<CtpAnonymousMapping> AnonymousMappings { get; set; }

        [CommandField()]
        public List<CtpClientCert> ClientCerts { get; set; }

        [CommandField()]
        public bool EnableSSL;

        [CommandField()]
        public string ServerCertificatePath { get; set; }

        /// <summary>
        /// If the ServerCertificatePath is a PFX file, this password will decrypt the certificate.
        /// </summary>
        [CommandField()]
        public string CertificatePassword { get; set; }

        public CtpServerConfig()
        {
            ClientCerts = new List<CtpClientCert>();
            Accounts = new List<CtpAccount>();
            AnonymousMappings = new List<CtpAnonymousMapping>();
        }

        public static explicit operator CtpServerConfig(CtpCommand obj)
        {
            return FromDocument(obj);
        }

        public void Validate()
        {


        }
    }
}
