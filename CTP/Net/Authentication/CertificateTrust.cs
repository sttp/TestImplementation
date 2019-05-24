using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using GSF;

namespace CTP.Net
{
    public class CertificateTrust
    {
        private object m_syncRoot = new object();

        private Dictionary<string, byte[]> m_list = new Dictionary<string, byte[]>();

        private ShortTime m_lastCertRefresh;

        private string m_path;

        public CertificateTrust(string path)
        {
            m_path = path;
            RebuildCerts();
        }

        public bool IsCertificateTrusted(X509Certificate channelCertificate)
        {
            lock (m_syncRoot)
            {
                TryAgain:
                if (!m_list.TryGetValue(channelCertificate.GetCertHashString(), out var data))
                {
                    if (m_lastCertRefresh.ElapsedSeconds() > 60)//check at most once per minute
                    {
                        RebuildCerts();
                        goto TryAgain;
                    }

                    return false;
                }

                return channelCertificate.GetRawCertData().SequenceEqual(data);
            }
        }

        public bool IsCertificateTrusted(X509Certificate channelCertificate, CertificateProof proof)
        {
            var cert = new EphemeralCertificate(proof.EphemeralCertificate);
            if (!(cert.ValidFrom <= DateTime.UtcNow && DateTime.UtcNow <= cert.ValidTo))
                return false;

            if (!channelCertificate.GetRawCertData().SequenceEqual(cert.ClientCertificate))
                return false;

            lock (m_syncRoot)
            {
                TryAgain:
                if (!m_list.TryGetValue(HexToString(cert.TrustedCertThumbprint), out var data))
                {
                    if (m_lastCertRefresh.ElapsedSeconds() > 60)//check at most once per minute
                    {
                        RebuildCerts();
                        goto TryAgain;
                    }

                    return false;
                }
                return cert.ValidateSignature(new X509Certificate2(data));
            }
        }

        private void RebuildCerts()
        {
            var cert = new Dictionary<string, byte[]>();
            foreach (var file in Directory.GetFiles(m_path, "*.cer"))
            {
                using (X509Certificate2 certificate = new X509Certificate2(file))
                {
                    cert.Add(certificate.Thumbprint, certificate.RawData);
                }
            }

            m_list = cert;
            m_lastCertRefresh = ShortTime.Now;
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
    }
}