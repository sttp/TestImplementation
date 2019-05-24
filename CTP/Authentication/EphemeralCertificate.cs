using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using GSF;
using GSF.IO;
using GSF.Security.Cryptography.X509;

namespace CTP
{
    public class EphemeralCertificate
    {
        private byte[] m_signedData;

        /// <summary>
        /// Valid starting this time.
        /// </summary>
        public DateTime ValidFrom { get; private set; }

        /// <summary>
        /// Valid until this time.
        /// </summary>
        public DateTime ValidTo { get; private set; }

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
        public List<string> GrantedRoles { get; private set; }

        /// <summary>
        /// The list of roles granted to this certificate.
        /// </summary>
        public List<string> DeniedRoles { get; private set; }

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

        public EphemeralCertificate(byte[] data)
        {
            ValidFrom = DateTime.MinValue;
            ValidTo = DateTime.MaxValue;
            SPN = "";
            LoginName = "";
            GrantedRoles = new List<string>();
            DeniedRoles = new List<string>();

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
                    case "grantedrole":
                        GrantedRoles.Add(Encoding.UTF8.GetString(value));
                        break;
                    case "deniedrole":
                        DeniedRoles.Add(Encoding.UTF8.GetString(value));
                        break;
                    case "validfrom":
                        ValidFrom = DateTime.Parse(Encoding.UTF8.GetString(value));
                        break;
                    case "validto":
                        ValidTo = DateTime.Parse(Encoding.UTF8.GetString(value));
                        break;
                    case "clientcertificate":
                        ClientCertificate = value;
                        break;
                    case "servercertificate":
                        ServerCertificate = value;
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
            return ValidateSignature(m_signedData, TrustedCertSignature, TrustedCertThumbprint, signingCertificate);
        }

        private static bool ValidateSignature(byte[] data, byte[] signature, byte[] thumbprint, X509Certificate2 signingCertificate)
        {
            using (var sha = SHA1.Create())
            {
                if (!thumbprint.SequenceEqual(sha.ComputeHash(signingCertificate.RawData)))
                    return false;
            }

            //ToDO: Base the signing mode to be the same as specified in the certificate
            using (var ecdsa = signingCertificate.GetECDsaPublicKey())
            {
                if (ecdsa != null)
                {
                    HashAlgorithmName name;
                    if (ecdsa.KeySize <= 256)
                        name = HashAlgorithmName.SHA256;
                    else if (ecdsa.KeySize <= 384)
                        name = HashAlgorithmName.SHA384;
                    else
                        name = HashAlgorithmName.SHA512;

                    return ecdsa.VerifyData(data, signature, name);
                }
            }
            using (var rsa = signingCertificate.GetRSAPublicKey())
            {
                if (rsa != null)
                {
                    HashAlgorithmName name;
                    if (rsa.KeySize <= 3072)
                        name = HashAlgorithmName.SHA256;
                    else if (rsa.KeySize <= 15360)
                        name = HashAlgorithmName.SHA384;
                    else
                        name = HashAlgorithmName.SHA512;

                    return rsa.VerifyData(data, signature, name, RSASignaturePadding.Pkcs1);
                }
            }

            return false;
        }


        private static byte[] SignData(byte[] data, int offset, int length, X509Certificate2 signingCertificate)
        {
            //ToDO: Base the signing mode to be the same as specified in the certificate
            using (var ecdsa = signingCertificate.GetECDsaPrivateKey())
            {
                if (ecdsa != null)
                {
                    HashAlgorithmName name;
                    if (ecdsa.KeySize <= 256)
                        name = HashAlgorithmName.SHA256;
                    else if (ecdsa.KeySize <= 384)
                        name = HashAlgorithmName.SHA384;
                    else
                        name = HashAlgorithmName.SHA512;

                    return ecdsa.SignData(data, offset, length, name);
                }
            }
            using (var rsa = signingCertificate.GetRSAPrivateKey())
            {
                if (rsa != null)
                {
                    HashAlgorithmName name;
                    if (rsa.KeySize <= 3072)
                        name = HashAlgorithmName.SHA256;
                    else if (rsa.KeySize <= 15360)
                        name = HashAlgorithmName.SHA384;
                    else
                        name = HashAlgorithmName.SHA512;

                    return rsa.SignData(data, offset, length, name, RSASignaturePadding.Pkcs1);
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

        private bool TryRead(MemoryStream stream, out string name, out string data)
        {
            if (TryRead(stream, out name, out byte[] dataBytes))
            {
                data = Encoding.UTF8.GetString(dataBytes);
                return true;
            }
            data = null;
            return false;
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

        public static byte[] SignClientCertificate(X509Certificate2 signingCertificate, string spn, string loginName, List<string> grantedRoles, List<string> deniedRoles, DateTime validAfter, DateTime validBefore, byte[] clientCertificate)
        {
            if (string.IsNullOrWhiteSpace(spn))
                throw new ArgumentNullException(nameof(spn));
            if (string.IsNullOrWhiteSpace(loginName))
                throw new ArgumentNullException(nameof(loginName));
            if (grantedRoles == null || grantedRoles.Count == 0)
                throw new ArgumentNullException(nameof(grantedRoles));
            if (grantedRoles.Any(string.IsNullOrWhiteSpace))
                throw new ArgumentNullException(nameof(grantedRoles));
            if (deniedRoles == null || deniedRoles.Count == 0)
                throw new ArgumentNullException(nameof(deniedRoles));
            if (deniedRoles.Any(string.IsNullOrWhiteSpace))
                throw new ArgumentNullException(nameof(deniedRoles));
            if (validAfter >= validBefore)
                throw new ArgumentNullException(nameof(validAfter));
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
            foreach (var role in grantedRoles)
                Write(ms, "GrantedRoles", role);
            foreach (var role in deniedRoles)
                Write(ms, "DeniedRoles", role);
            Write(ms, "ValidBefore", validBefore.ToString("O"));
            Write(ms, "ValidAfter", validAfter.ToString("O"));
            Write(ms, "ClientCertificate", clientCertificate);

            byte[] data = ms.ToArray();
            byte[] signature = SignData(data, 0, data.Length, signingCertificate);

            var rv = new byte[2 + signature.Length + data.Length];
            BigEndian.CopyBytes((ushort)signature.Length, rv, 0);
            signature.CopyTo(rv, 2);
            data.CopyTo(rv, 2 + signature.Length);
            return rv;
        }

        public static byte[] SignServerCertificate(X509Certificate2 signingCertificate, string spn, DateTime validAfter, DateTime validBefore, byte[] serverCertificate)
        {
            if (string.IsNullOrWhiteSpace(spn))
                throw new ArgumentNullException(nameof(spn));
            if (validAfter >= validBefore)
                throw new ArgumentNullException(nameof(validAfter));
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
            Write(ms, "ValidBefore", validBefore.ToString("O"));
            Write(ms, "ValidAfter", validAfter.ToString("O"));
            Write(ms, "ServerCertificate", serverCertificate);

            byte[] data = ms.ToArray();
            byte[] signature = SignData(data, 0, data.Length, signingCertificate);

            var rv = new byte[2 + signature.Length + data.Length];
            BigEndian.CopyBytes((ushort)signature.Length, rv, 0);
            signature.CopyTo(rv, 2);
            data.CopyTo(rv, 2 + signature.Length);
            return rv;
        }


    }
}
