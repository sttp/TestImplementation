using System.Collections.Generic;

namespace CTP.Net
{
    [CommandName("CtpServerConfig")]
    public class CtpServerConfig
        : CommandObject<CtpServerConfig>
    {
        [CommandField()]
        public string SPN { get; set; }

        [CommandField()]
        public bool EnableSSL { get; set; }

        [CommandField()]
        public bool UseEphemeralCertificates { get; set; }

        [CommandField()]
        public string ServerCertificatePath { get; set; }

        /// <summary>
        /// If the ServerCertificatePath is a PFX file, this password will decrypt the certificate.
        /// If it's a .CER, the private key will be obtained from the certificate store.
        /// </summary>
        [CommandField()]
        public string CertificatePassword { get; set; }


        [CommandField()]
        public List<CtpAccount> Accounts { get; set; }

     

        public CtpServerConfig()
        {
            Accounts = new List<CtpAccount>();
        }

        public static explicit operator CtpServerConfig(CtpCommand obj)
        {
            return FromCommand(obj);
        }

        /// <summary>
        /// Creates an anonymous config that allows all users to connect.
        /// </summary>
        /// <returns></returns>
        public static CtpServerConfig CreateAnonymous(string spn)
        {
            var cfg = new CtpServerConfig();
            cfg.SPN = spn;
            cfg.EnableSSL = false;
            return cfg;
        }

    }
}
