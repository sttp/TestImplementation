using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using GSF;
using GSF.IO;

namespace CTP.Authentication
{
    public class Ticket
    {
        /// <summary>
        /// Gets the UTC time this ticket is valid from.
        /// </summary>
        public readonly DateTime ValidFrom;

        /// <summary>
        /// Gets the UTC time this ticket is valid until.
        /// </summary>
        public readonly DateTime ValidTo;

        /// <summary>
        /// A string that identifies the user of this ticket.
        /// </summary>
        public readonly string LoginName;

        /// <summary>
        /// The list of roles granted by this ticket.
        /// </summary>
        public readonly List<string> Roles;

        /// <summary>
        /// The list of approved certificates that the remote resource may use. 
        /// This is the SHA-256 hash of the public key.
        /// </summary>
        public readonly List<byte[]> ApprovedClientCertificates;

        /// <summary>
        /// The thumbprint of the authorization service's certificate.
        /// </summary>
        public byte[] AuthorizationThumbprint { get; private set; }

        /// <summary>
        /// The signature of the ticket.
        /// </summary>
        public byte[] AuthorizationSignature { get; private set; }

        public Ticket(DateTime validFrom, DateTime validTo, string loginName, List<string> roles, List<byte[]> approvedClientCertificates)
        {
            ValidFrom = validFrom;
            ValidTo = validTo;
            LoginName = loginName;
            Roles = roles;
            ApprovedClientCertificates = approvedClientCertificates;
        }

        public Ticket(byte[] data)
        {
            var ms = new MemoryStream(data);
            ValidFrom = new DateTime(BigEndian.ToInt64(ms.ReadBytes(8), 0));
            ValidTo = new DateTime(BigEndian.ToInt64(ms.ReadBytes(8), 0));

            LoginName = Encoding.UTF8.GetString(ms.ReadBytes((int)Encoding7Bit.ReadUInt32(ms.ReadNextByte)));

            Roles = new List<string>();
            uint cnt = Encoding7Bit.ReadUInt32(ms.ReadNextByte);
            while (cnt > 0)
            {
                Roles.Add(Encoding.UTF8.GetString(ms.ReadBytes((int)Encoding7Bit.ReadUInt32(ms.ReadNextByte))));
                cnt--;
            }

            cnt = Encoding7Bit.ReadUInt32(ms.ReadNextByte);
            while (cnt > 0)
            {
                ApprovedClientCertificates.Add(ms.ReadBytes((int)Encoding7Bit.ReadUInt32(ms.ReadNextByte)));
                cnt--;
            }

            AuthorizationSignature = ms.ReadBytes((int)Encoding7Bit.ReadUInt32(ms.ReadNextByte));
        }

        public byte[] ToArray()
        {
            var ms = new MemoryStream();
            ms.Write(BigEndian.GetBytes(ValidFrom.Ticks));
            ms.Write(BigEndian.GetBytes(ValidTo.Ticks));

            byte[] str = Encoding.UTF8.GetBytes(LoginName);
            Encoding7Bit.Write(ms.WriteByte, (uint)str.Length);
            ms.Write(str, 0, str.Length);

            Encoding7Bit.Write(ms.WriteByte, (uint)Roles.Count);
            foreach (var role in Roles)
            {
                str = Encoding.UTF8.GetBytes(role);
                Encoding7Bit.Write(ms.WriteByte, (uint)str.Length);
                ms.Write(str, 0, str.Length);
            }

            Encoding7Bit.Write(ms.WriteByte, (uint)ApprovedClientCertificates.Count);
            foreach (var role in ApprovedClientCertificates)
            {
                Encoding7Bit.Write(ms.WriteByte, (uint)role.Length);
                ms.Write(role, 0, role.Length);
            }

            Encoding7Bit.Write(ms.WriteByte, (uint)AuthorizationThumbprint.Length);
            ms.Write(AuthorizationThumbprint, 0, AuthorizationThumbprint.Length);

            Encoding7Bit.Write(ms.WriteByte, (uint)AuthorizationSignature.Length);
            ms.Write(AuthorizationSignature, 0, AuthorizationSignature.Length);
            return ms.ToArray();
        }

        public byte[] Sign(X509Certificate2 certificate)
        {
            var ms = new MemoryStream();
            ms.Write(BigEndian.GetBytes(ValidFrom.Ticks));
            ms.Write(BigEndian.GetBytes(ValidTo.Ticks));

            byte[] str = Encoding.UTF8.GetBytes(LoginName);
            Encoding7Bit.Write(ms.WriteByte, (uint)str.Length);
            ms.Write(str, 0, str.Length);

            Encoding7Bit.Write(ms.WriteByte, (uint)Roles.Count);
            foreach (var role in Roles)
            {
                str = Encoding.UTF8.GetBytes(role);
                Encoding7Bit.Write(ms.WriteByte, (uint)str.Length);
                ms.Write(str, 0, str.Length);
            }

            Encoding7Bit.Write(ms.WriteByte, (uint)ApprovedClientCertificates.Count);
            foreach (var role in ApprovedClientCertificates)
            {
                Encoding7Bit.Write(ms.WriteByte, (uint)role.Length);
                ms.Write(role, 0, role.Length);
            }

            AuthorizationThumbprint = certificate.GetPublicKey();

            Encoding7Bit.Write(ms.WriteByte, (uint)AuthorizationThumbprint.Length);
            ms.Write(AuthorizationThumbprint, 0, AuthorizationThumbprint.Length);

            using (var rsa = certificate.GetRSAPrivateKey())
            {
                AuthorizationSignature = rsa.SignData(ms.ToArray(), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }

            Encoding7Bit.Write(ms.WriteByte, (uint)AuthorizationSignature.Length);
            ms.Write(AuthorizationSignature, 0, AuthorizationSignature.Length);
            return ms.ToArray();
        }

        public bool VerifySignature(X509Certificate2 certificate)
        {

            var ms = new MemoryStream();
            ms.Write(BigEndian.GetBytes(ValidFrom.Ticks));
            ms.Write(BigEndian.GetBytes(ValidTo.Ticks));

            byte[] str = Encoding.UTF8.GetBytes(LoginName);
            Encoding7Bit.Write(ms.WriteByte, (uint)str.Length);
            ms.Write(str, 0, str.Length);

            Encoding7Bit.Write(ms.WriteByte, (uint)Roles.Count);
            foreach (var role in Roles)
            {
                str = Encoding.UTF8.GetBytes(role);
                Encoding7Bit.Write(ms.WriteByte, (uint)str.Length);
                ms.Write(str, 0, str.Length);
            }

            Encoding7Bit.Write(ms.WriteByte, (uint)ApprovedClientCertificates.Count);
            foreach (var role in ApprovedClientCertificates)
            {
                Encoding7Bit.Write(ms.WriteByte, (uint)role.Length);
                ms.Write(role, 0, role.Length);
            }

            AuthorizationThumbprint = certificate.GetPublicKey();

            Encoding7Bit.Write(ms.WriteByte, (uint)AuthorizationThumbprint.Length);
            ms.Write(AuthorizationThumbprint, 0, AuthorizationThumbprint.Length);

            using (var rsa = certificate.GetRSAPublicKey())
            {
                return rsa.VerifyData(ms.ToArray(), AuthorizationSignature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }
    }
}
