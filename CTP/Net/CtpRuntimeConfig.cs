using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace CTP.Net
{
    public class CtpRuntimeConfig
    {

        public EncryptionOptions EncryptionOptions;

        public SortedList<IpMatchDefinition, string> AnonymousMappings = new SortedList<IpMatchDefinition, string>();

        public Dictionary<string, List<string>> Accounts = new Dictionary<string, List<string>>();

        public Dictionary<string, ClientCerts> CertificateClients = new Dictionary<string, ClientCerts>();

        public CtpRuntimeConfig(CtpServerConfig config)
        {
            config.Validate();
            X509Certificate2 cert = null;
            if (config.EnableSSL)
            {
                if (!File.Exists(config.ServerCertificatePath))
                    throw new Exception($"Missing certificate at {config.ServerCertificatePath}");
                cert = new X509Certificate2(config.ServerCertificatePath);
            }

            EncryptionOptions = new EncryptionOptions(cert);

            foreach (var item in config.AnonymousMappings)
            {
                var def = new IpMatchDefinition(IPAddress.Parse(item.TrustedIPs.IpAddress), item.TrustedIPs.MaskBits);
                AnonymousMappings.Add(def, item.MappedAccount);
            }

            foreach (var item in config.Accounts)
            {
                Accounts.Add(item.Name, item.Roles);
            }

            foreach (var item in config.ClientCerts)
            {
                foreach (var path in item.CertificatePaths)
                {
                    if (File.Exists(path))
                    {
                        X509Certificate2 certificate = new X509Certificate2(path);
                        CertificateClients.Add(certificate.Thumbprint, new ClientCerts(item, certificate));
                    }
                }
            }
        }
    }
}