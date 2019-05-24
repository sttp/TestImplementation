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
                    Write(ms, name, ((DateTime)value).ToString("O"));
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

            byte[] data = ms.ToArray();
            byte[] signature = SignData(data, 0, data.Length, signingCertificate);

            var rv = new byte[2 + signature.Length + data.Length];

            rv[0] = (byte)(signature.Length >> 8);
            rv[1] = (byte)signature.Length;
            signature.CopyTo(rv, 2);
            data.CopyTo(rv, 2 + signature.Length);
            return rv;
        }
    }
}
