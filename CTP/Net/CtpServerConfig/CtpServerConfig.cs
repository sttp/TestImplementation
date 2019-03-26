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
            return FromCommand(obj);
        }

        /// <summary>
        /// Creates an anonymous config that allows all users to connect.
        /// </summary>
        /// <returns></returns>
        public static CtpServerConfig CreateAnonymous()
        {
            var cfg = new CtpServerConfig();
            cfg.EnableSSL = false;
            cfg.AnonymousMappings.Add(new CtpAnonymousMapping()
            {
                MappedAccount = "User",
                Name = "User",
                TrustedIPs = new IpAndMask() { IpAddress = "0.0.0.0", MaskBits = 0 }
            });
            cfg.Accounts.Add(new CtpAccount() { Description = "Default", IsEnabled = true, Name = "User", Roles = new List<string>() });
            return cfg;
        }

    }
}
