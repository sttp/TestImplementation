using System.Collections.Generic;

namespace CTP.Net
{
    [DocumentName("CtpServerConfig")]
    public class CtpServerConfig
        : DocumentObject<CtpServerConfig>
    {
        [DocumentField()]
        public List<CtpAccount> Accounts { get; set; }

        [DocumentField()]
        public List<CtpInstalledCertificates> InstalledCertificates { get; set; }

        [DocumentField()]
        public List<CtpAnonymousMapping> AnonymousMappings { get; set; }

        [DocumentField()]
        public List<CtpClientCert> ClientCerts { get; set; }

        public CtpServerConfig()
        {
            ClientCerts = new List<CtpClientCert>();
            Accounts = new List<CtpAccount>();
            InstalledCertificates = new List<CtpInstalledCertificates>();
            AnonymousMappings = new List<CtpAnonymousMapping>();
        }

        public static explicit operator CtpServerConfig(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}
