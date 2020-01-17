using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SqlServerAuthorizationCLR
{
    public static class EphemeralCertificate
    {
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

        public static byte[] SignCertificate(X509Certificate2 signingCertificate, DataTable table)
        {
            var ms = new MemoryStream();
            using (var sha = SHA1.Create())
                Write(ms, sha.ComputeHash(signingCertificate.RawData));

            foreach (DataRow dataRow in table.Rows)
            {
                string name = dataRow[0].ToString();
                object value = dataRow[1];

                if (value is DateTime)
                {
                    Write(ms, name, ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else if (value is byte[])
                {
                    Write(ms, name, (byte[])value);
                }
                else
                {
                    Write(ms, name, value.ToString());
                }
            }

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

            rv[0] = (byte)(signature.Length >> 8);
            rv[1] = (byte)signature.Length;
            signature.CopyTo(rv, 2);
            data.CopyTo(rv, 2 + signature.Length);
            return rv;
        }
    }
}
