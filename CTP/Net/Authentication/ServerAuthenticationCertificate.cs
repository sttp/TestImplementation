using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using GSF;
using GSF.Security.Cryptography.X509;
using GSF.Threading;

namespace CTP.Net
{
    public class ServerAuthenticationCertificate : IServerAuthentication, IServerHandshake
    {
        private static readonly X509Certificate2 TempCert = CertificateMaker.GenerateSelfSignedCertificate(CertificateSigningMode.ECDSA_256_SHA2_256, "Ephemeral Certificate", new DateTime(DateTime.Now.Year - 1, 1, 1), new DateTime(DateTime.Now.Year + 10, 1, 1));

        private X509Certificate2 m_certificate;
        private X509Certificate2 m_signingCertificate;
        private CtpServerConfig m_config;
        private Dictionary<string, CtpAccount> m_accounts = new Dictionary<string, CtpAccount>();
        private Dictionary<string, Tuple<string, byte[]>> m_certificateAccounts = new Dictionary<string, Tuple<string, byte[]>>();
        private object m_syncRoot = new object();
        private ShortTime m_lastCertRefresh;

        public ServerAuthenticationCertificate(CtpServerConfig config)
        {
            m_config = config;
            if (config.EnableSSL)
            {
                X509Certificate2 cert = null;
                if (!File.Exists(config.ServerCertificatePath))
                    throw new Exception($"Missing certificate at {config.ServerCertificatePath}");
                if (string.IsNullOrWhiteSpace(config.CertificatePassword))
                {
                    cert = new X509Certificate2(config.ServerCertificatePath);
                }
                else
                {
                    cert = new X509Certificate2(config.ServerCertificatePath, config.CertificatePassword);
                }
                if (!cert.HasPrivateKey)
                {
                    var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                    store.Open(OpenFlags.ReadOnly);
                    foreach (var c in store.Certificates)
                    {
                        if (c.HasPrivateKey && c.Thumbprint == cert.Thumbprint)
                        {
                            cert.Dispose();
                            cert = c;
                            break;
                        }
                    }
                    store.Close();
                }
                if (!cert.HasPrivateKey)
                    throw new Exception("Missing private key for certificate " + cert.ToString());

                if (m_config.UseEphemeralCertificates)
                {
                    m_certificate = TempCert;
                    m_signingCertificate = cert;
                }
                else
                {
                    m_certificate = cert;
                }
            }

            foreach (var item in config.Accounts)
            {
                item.ImplicitRoles = item.ImplicitRoles ?? new List<string>();
                item.ExplicitRoles = item.ExplicitRoles ?? new List<string>();
                item.AllowedRemoteIPs = item.AllowedRemoteIPs ?? new List<IpAndMask>();
                m_accounts.Add(item.Name, item);
            }

            RebuildCerts();
        }

        private bool TryFindCertificate(string thumbprint, out X509Certificate2 cert, out CtpAccount account)
        {
            lock (m_syncRoot)
            {
                TryAgain:
                if (!m_certificateAccounts.TryGetValue(thumbprint, out var tuple))
                {
                    if (m_lastCertRefresh.ElapsedSeconds() > 60)//check at most once per minute
                    {
                        RebuildCerts();
                        goto TryAgain;
                    }

                    cert = null;
                    account = null;
                    return false;
                }
                cert = new X509Certificate2(tuple.Item2);
                account = m_accounts[tuple.Item1];
                return true;
            }
        }


        private void RebuildCerts()
        {
            var cert = new Dictionary<string, Tuple<string, byte[]>>();
            foreach (var item in m_config.Accounts)
            {
                if (Directory.Exists(item.CertificateDirectory))
                {
                    foreach (var file in Directory.GetFiles(item.CertificateDirectory, "*.cer"))
                    {
                        X509Certificate2 certificate = new X509Certificate2(file);
                        cert.Add(certificate.Thumbprint, new Tuple<string, byte[]>(item.Name, certificate.RawData));
                        certificate.Dispose();
                    }
                }
            }

            m_certificateAccounts = cert;
            m_lastCertRefresh = ShortTime.Now;

        }


        public void Dispose()
        {

        }

        public bool UseSSL => m_config.EnableSSL;
        public bool IsEphemeralCertificate => m_config.UseEphemeralCertificates;

        public X509Certificate2 GetCertificate()
        {
            return m_certificate;
        }

        public ServerDone GetServerDone()
        {
            return new ServerDone(m_config.SPN);
        }

        public CertificateProof GetCertificateProof()
        {
            return new CertificateProof(EphemeralCertificate.SignServerCertificate(m_signingCertificate, m_config.SPN,
                DateTime.UtcNow.AddMinutes(-5), DateTime.UtcNow.AddMinutes(15), m_signingCertificate.RawData));
        }

        public bool IsCertificateTrusted(CtpNetStream stream, ClientDone clientDone)
        {
            if (!TryFindCertificate(stream.RemoteCertificate.GetCertHashString(), out var cert, out var account))
            {
                return false;
            }

            if (!account.IsIPAllowed(stream.RemoteEndpoint.Address))
            {
                return false;
            }

            GrantPermissions(stream, account, clientDone.LoginName, clientDone.GrantedRoles, clientDone.DeniedRoles);
            return true;
        }

        private void GrantPermissions(CtpNetStream stream, CtpAccount account, string loginName, List<string> grantedRoles, List<string> deniedRoles)
        {
            stream.LoginName = loginName;
            stream.AccountName = account.Name;
            stream.GrantedRoles.UnionWith(account.ImplicitRoles);
            stream.GrantedRoles.UnionWith(account.ExplicitRoles.Union(grantedRoles));
            stream.GrantedRoles.ExceptWith(deniedRoles);
        }

        public bool IsCertificateTrusted(CtpNetStream stream, CertificateProof proof)
        {
            var eph = new EphemeralCertificate(proof.EphemeralCertificate);

            if (!TryFindCertificate(HexToString(eph.TrustedCertThumbprint), out var signingCert, out var account))
            {
                return false;
            }

            if (!eph.ValidateSignature(signingCert))
            {
                return false;
            }

            if (!account.IsIPAllowed(stream.RemoteEndpoint.Address))
            {
                return false;
            }

            GrantPermissions(stream, account, eph.LoginName, eph.GrantedRoles, eph.DeniedRoles);

            return true;
        }

        private static char ToChar(int numeric)
        {
            return numeric < 10 ? (char)(numeric + '0') : (char)(numeric + ('A' - 10));
        }

        private static string HexToString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 2);
            foreach (var b in data)
            {
                sb.Append(ToChar(b >> 4));
                sb.Append(ToChar(b & 15));
            }
            return sb.ToString();
        }

        public IServerHandshake StartHandshake()
        {
            return this;
        }
    }
}