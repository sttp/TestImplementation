using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace CTP.Net
{
    public class CertificateTrust
    {
        private Dictionary<string, byte[]> m_list = new Dictionary<string, byte[]>();
        private List<byte[]> m_sha1 = new List<byte[]>();
        private List<byte[]> m_sha256 = new List<byte[]>();
        private List<byte[]> m_sha384 = new List<byte[]>();
        private List<byte[]> m_sha512 = new List<byte[]>();
        private List<byte[]> m_raw = new List<byte[]>();

        public CertificateTrust(string path)
        {

        }

        public bool IsCertificateTrusted(X509Certificate channelCertificate)
        {
            throw new NotImplementedException();
        }

        public bool IsCertificateTrusted(X509Certificate channelCertificate, CertificateProof proof)
        {
            throw new NotImplementedException();
        }

        public void Add(byte[] certificate)
        {
            using (var cert = new X509Certificate2(certificate))
            {
                m_list.Add(cert.Thumbprint.ToLower(),(byte[])certificate.Clone());
            }
        }

        //public bool IsTrustedAndSigned(Auth certificate)
        //{
        //    if (m_list.TryGetValue(certificate.CertificateThumbprint, out var data))
        //    {
        //        using (var cert = new X509Certificate2(data))
        //        {
        //            return certificate.ValidateSignature(cert);
        //        }
        //    }
        //    return false;
        //}
    }
}