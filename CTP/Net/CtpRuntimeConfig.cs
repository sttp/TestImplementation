using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace CTP.Net
{
    public class CtpRuntimeConfig
    {
        public SortedList<IpMatchDefinition, EncryptionOptions> EncryptionOptions = new SortedList<IpMatchDefinition, EncryptionOptions>();

        public SortedList<IpMatchDefinition, string> AnonymousMappings = new SortedList<IpMatchDefinition, string>();

        public Dictionary<string, List<string>> Accounts = new Dictionary<string, List<string>>();

        public Dictionary<string, ClientCerts> CertificateClients = new Dictionary<string, ClientCerts>();

        public CtpRuntimeConfig(CtpServerConfig config)
        {
            config.Validate();
            foreach (var item in config.InstalledCertificates)
            {
                if (item.IsEnabled)
                {
                    X509Certificate2 cert = null;
                    if (item.EnableSSL)
                    {
                        if (!File.Exists(item.CertificatePath))
                            throw new Exception($"Missing certificate for {item.Name} at {item.CertificatePath}");
                        cert = new X509Certificate2(item.CertificatePath);
                    }

                    foreach (var ip in item.RemoteIPs)
                    {
                        var def = new IpMatchDefinition(IPAddress.Parse(ip.IpAddress), ip.MaskBits);
                        EncryptionOptions.Add(def, new EncryptionOptions(def, cert));
                    }
                }
            }

            foreach (var item in config.AnonymousMappings)
            {
                var def = new IpMatchDefinition(IPAddress.Parse(item.AccessList.IpAddress), item.AccessList.MaskBits);
                AnonymousMappings.Add(def, item.AccountName);
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