using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using GSF;
using GSF.IO;

namespace CTP
{
    public class EphemeralCertificate
    {
        private byte[] m_signedData;

        /// <summary>
        /// Valid starting this time.
        /// </summary>
        public DateTime NotBefore { get; private set; }

        /// <summary>
        /// Valid until this time.
        /// </summary>
        public DateTime NotAfter { get; private set; }

        /// <summary>
        /// Gets the service provider name associated with a request.
        /// </summary>
        public string SPN { get; private set; }

        /// <summary>
        /// A string that identifies the user of this ticket.
        /// </summary>
        public string LoginName { get; private set; }

        /// <summary>
        /// The list of roles granted to this certificate.
        /// </summary>
        public List<string> GrantRoles { get; private set; }

        /// <summary>
        /// The list of roles granted to this certificate.
        /// </summary>
        public List<string> DenyRoles { get; private set; }

        /// <summary>
        /// The public certificate that has this permission.
        /// </summary>
        public byte[] ClientCertificate { get; private set; }

        /// <summary>
        /// The public certificate that has this permission.
        /// </summary>
        public byte[] ServerCertificate { get; private set; }

        public byte[] TrustedCertThumbprint { get; private set; }

        public byte[] TrustedCertSignature { get; private set; }

        public string SignatureAlgorithm { get; private set; }

        public EphemeralCertificate(byte[] data)
        {
            NotBefore = DateTime.MinValue;
            NotAfter = DateTime.MaxValue;
            SPN = "";
            LoginName = "";
            GrantRoles = new List<string>();
            DenyRoles = new List<string>();

            var ms = new MemoryStream(data);
            TrustedCertSignature = ReadBytes(ms);
            int offset = (int)ms.Position;
            m_signedData = new byte[data.Length - offset];
            Array.Copy(data, offset, m_signedData, 0, data.Length - offset);

            TrustedCertThumbprint = ReadBytes(ms);

            while (TryRead(ms, out var name, out byte[] value))
            {
                if (name.Length < 1)
                    throw new Exception("Name cannot be an empty string");
                switch (name.ToLower())
                {
                    case "spn":
                        SPN = Encoding.UTF8.GetString(value);
                        break;
                    case "salt":
                        break;
                    case "login":
                        LoginName = Encoding.UTF8.GetString(value);
                        break;
                    case "grantrole":
                        GrantRoles.Add(Encoding.UTF8.GetString(value));
                        break;
                    case "denyrole":
                        DenyRoles.Add(Encoding.UTF8.GetString(value));
                        break;
                    case "notbefore":
                        NotBefore = DateTime.SpecifyKind(DateTime.Parse(Encoding.UTF8.GetString(value)), DateTimeKind.Utc);
                        break;
                    case "notAfter":
                        NotAfter = DateTime.SpecifyKind(DateTime.Parse(Encoding.UTF8.GetString(value)), DateTimeKind.Utc);
                        break;
                    case "clientcertificate":
                        ClientCertificate = value;
                        break;
                    case "servercertificate":
                        ServerCertificate = value;
                        break;
                    case "signaturealgorithm":
                        SignatureAlgorithm = Encoding.UTF8.GetString(value);
                        break;
                    default:
                        {
                            if (char.IsLower(name[0]))
                            {
                                //lower character names are optional.
                                break;
                            }
                            throw new Exception("Unknown tag");
                        }
                }
            }
        }

        public bool ValidateSignature(X509Certificate2 signingCertificate)
        {
            using (var sha = SHA1.Create())
            {
                if (!TrustedCertThumbprint.SequenceEqual(sha.ComputeHash(signingCertificate.RawData)))
                    return false;
            }

            switch (SignatureAlgorithm)
            {
                case "SHA512-ECDSA":
                    using (var ecdsa = signingCertificate.GetECDsaPublicKey())
                    {
                        if (ecdsa != null)
                        {
                            return ecdsa.VerifyData(m_signedData, TrustedCertSignature, HashAlgorithmName.SHA512);
                        }
                    }
                    break;
                case "SHA512-RSA-PKCS1":
                    using (var rsa = signingCertificate.GetRSAPublicKey())
                    {
                        if (rsa != null)
                        {
                            return rsa.VerifyData(m_signedData, TrustedCertSignature, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
                        }
                    }
                    break;
                default:
                    throw new Exception("Unknown Signature Algorithm: " + SignatureAlgorithm);

            }
            return false;
        }

        private static byte[] SignData(byte[] data, X509Certificate2 signingCertificate)
        {
            using (var ecdsa = signingCertificate.GetECDsaPrivateKey())
            {
                if (ecdsa != null)
                {
                    return ecdsa.SignData(data, HashAlgorithmName.SHA512);
                }
            }
            using (var rsa = signingCertificate.GetRSAPrivateKey())
            {
                if (rsa != null)
                {
                    return rsa.SignData(data, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
                }
            }

            throw new Exception("Signature Algorithm not found");
        }


        private static void Write(MemoryStream stream, string name, string data)
        {
            Write(stream, name, Encoding.UTF8.GetBytes(data));
        }

        private static void Write(MemoryStream stream, string name, byte[] data)
        {
            byte[] nameBytes = Encoding.UTF8.GetBytes(name);
            if (nameBytes.Length > byte.MaxValue)
                throw new Exception("String length too long");
            byte[] dataBytes = data;
            if (dataBytes.Length > ushort.MaxValue)
                throw new Exception("Data length too long");

            stream.WriteByte((byte)nameBytes.Length);
            stream.Write(nameBytes, 0, nameBytes.Length);
            stream.WriteByte((byte)(dataBytes.Length >> 8));
            stream.WriteByte((byte)(dataBytes.Length));
            stream.Write(dataBytes, 0, dataBytes.Length);
        }

        private static void Write(MemoryStream stream, byte[] dataBytes)
        {
            if (dataBytes.Length > ushort.MaxValue)
                throw new Exception("data length too long");

            stream.WriteByte((byte)(dataBytes.Length >> 8));
            stream.WriteByte((byte)(dataBytes.Length));
            stream.Write(dataBytes, 0, dataBytes.Length);
        }

        private bool TryRead(MemoryStream stream, out string name, out byte[] data)
        {
            if (stream.Position == stream.Length)
            {
                name = null;
                data = null;
                return false;
            }

            int length = stream.ReadNextByte();
            byte[] nameBytes = stream.ReadBytes(length);
            length = stream.ReadNextByte() << 8;
            length += stream.ReadNextByte();
            data = stream.ReadBytes(length);
            name = Encoding.UTF8.GetString(nameBytes);
            return true;
        }

        private byte[] ReadBytes(MemoryStream stream)
        {
            int length = stream.ReadNextByte() << 8;
            length += stream.ReadNextByte();
            return stream.ReadBytes(length);
        }

        public static byte[] SignClientCertificate(X509Certificate2 signingCertificate, string spn, string loginName, List<string> grantRoles, List<string> denyRoles, DateTime notBefore, DateTime notAfter, byte[] clientCertificate)
        {
            if (string.IsNullOrWhiteSpace(spn))
                throw new ArgumentNullException(nameof(spn));
            if (string.IsNullOrWhiteSpace(loginName))
                throw new ArgumentNullException(nameof(loginName));
            if (grantRoles == null || grantRoles.Count == 0)
                throw new ArgumentNullException(nameof(grantRoles));
            if (grantRoles.Any(string.IsNullOrWhiteSpace))
                throw new ArgumentNullException(nameof(grantRoles));
            if (denyRoles == null || denyRoles.Count == 0)
                throw new ArgumentNullException(nameof(denyRoles));
            if (denyRoles.Any(string.IsNullOrWhiteSpace))
                throw new ArgumentNullException(nameof(denyRoles));
            if (notBefore >= notAfter)
                throw new ArgumentNullException(nameof(notBefore));
            if (clientCertificate == null || clientCertificate.Length < 30)
                throw new ArgumentNullException(nameof(clientCertificate));

            var ms = new MemoryStream();
            using (var sha = SHA1.Create())
                Write(ms, sha.ComputeHash(signingCertificate.RawData));

            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] salt = new byte[16];
                rng.GetBytes(salt);
                Write(ms, "salt", salt);
            }

            Write(ms, "SPN", spn);
            Write(ms, "login", loginName);
            foreach (var role in grantRoles)
                Write(ms, "GrantRole", role);
            foreach (var role in denyRoles)
                Write(ms, "DenyRole", role);
            Write(ms, "NotAfter", notAfter.ToString("yyyy-MM-dd HH:mm:ss"));
            Write(ms, "NotBefore", notBefore.ToString("yyyy-MM-dd HH:mm:ss"));
            Write(ms, "ClientCertificate", clientCertificate);

            if (signingCertificate.PublicKey.Oid.Value == "1.2.840.10045.2.1") //iso/member-body/us/ansi-x962/keyType/ecPublicKey
            {
                Write(ms, "SignatureAlgorithm", "SHA512-ECDSA");
            }
            else if (signingCertificate.PublicKey.Oid.Value.StartsWith("1.2.840.113549.1.1.")) //iso/member-body/us/rsadsi/pkcs/pkcs-1
            {
                Write(ms, "SignatureAlgorithm", "SHA512-RSA-PKCS1");
            }
            else
            {
                throw new Exception("Unknown public key type" + signingCertificate.PublicKey.Oid.Value);
            }
            byte[] data = ms.ToArray();
            byte[] signature = SignData(data, signingCertificate);

            var rv = new byte[2 + signature.Length + data.Length];
            BigEndian.CopyBytes((ushort)signature.Length, rv, 0);
            signature.CopyTo(rv, 2);
            data.CopyTo(rv, 2 + signature.Length);
            return rv;
        }

        public static byte[] SignServerCertificate(X509Certificate2 signingCertificate, string spn, DateTime notBefore, DateTime notAfter, byte[] serverCertificate)
        {
            if (string.IsNullOrWhiteSpace(spn))
                throw new ArgumentNullException(nameof(spn));
            if (notBefore >= notAfter)
                throw new ArgumentNullException(nameof(notBefore));
            if (serverCertificate == null || serverCertificate.Length < 30)
                throw new ArgumentNullException(nameof(serverCertificate));

            var ms = new MemoryStream();
            using (var sha = SHA1.Create())
                Write(ms, sha.ComputeHash(signingCertificate.RawData));

            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] salt = new byte[16];
                rng.GetBytes(salt);
                Write(ms, "salt", salt);
            }

            Write(ms, "SPN", spn);
            Write(ms, "NotAfter", notAfter.ToString("yyyy-MM-dd HH:mm:ss"));
            Write(ms, "NotBefore", notBefore.ToString("yyyy-MM-dd HH:mm:ss"));
            Write(ms, "ServerCertificate", serverCertificate);

            if (signingCertificate.PublicKey.Oid.Value == "1.2.840.10045.2.1") //iso/member-body/us/ansi-x962/keyType/ecPublicKey
            {
                Write(ms, "SignatureAlgorithm", "SHA512-ECDSA");
            }
            else if (signingCertificate.PublicKey.Oid.Value.StartsWith("1.2.840.113549.1.1.")) //iso/member-body/us/rsadsi/pkcs/pkcs-1
            {
                Write(ms, "SignatureAlgorithm", "SHA512-RSA-PKCS1");
            }
            else
            {
                throw new Exception("Unknown public key type" + signingCertificate.PublicKey.Oid.Value);
            }

            byte[] data = ms.ToArray();
            byte[] signature = SignData(data, signingCertificate);

            var rv = new byte[2 + signature.Length + data.Length];
            BigEndian.CopyBytes((ushort)signature.Length, rv, 0);
            signature.CopyTo(rv, 2);
            data.CopyTo(rv, 2 + signature.Length);
            return rv;
        }


    }
}
