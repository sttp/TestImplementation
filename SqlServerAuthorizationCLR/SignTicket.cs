using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.SqlServer.Server;

namespace SqlServerAuthorizationCLR
{
    public static class SignTicket
    {
        [SqlProcedure()]
        public static void Sign(out byte[] ticket, out string certificateThumbprint, out byte[] signature, string certificatePath, string ticketQuery)
        {
            var ticketValues = new DataTable();

            if (string.IsNullOrWhiteSpace(ticketQuery))
                throw new Exception("Ticket cannot be null");

            var ms = new MemoryStream();

            using (var connection = new SqlConnection("context connection=true"))
            {
                connection.Open();
                var cmd = new SqlCommand(ticketQuery, connection);
                var ta = new SqlDataAdapter(ticketQuery, connection);
                ta.Fill(ticketValues);
                connection.Close();
            }

            foreach (DataRow dataRow in ticketValues.Rows)
            {
                string name = dataRow[0].ToString();
                object value = dataRow[1];

                if (value is DateTime)
                    value = ((DateTime)value).ToString("O");
                else if (value is byte[])
                    value = Convert.ToBase64String((byte[])value);
                else
                    value = value.ToString();

                Write(ms, name, (string)value);
            }

            ticket = ms.ToArray();

            var cert = Find(certificatePath);
            certificateThumbprint = cert.Thumbprint;

            using (var ecdsa = cert.GetECDsaPrivateKey())
            {
                if (ecdsa != null)
                {
                    signature = ecdsa.SignData(ticket, HashAlgorithmName.SHA512);
                    return;
                }
            }
            using (var rsa = cert.GetRSAPrivateKey())
            {
                if (rsa != null)
                {
                    signature = rsa.SignData(ticket, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
                    return;
                }
            }
            throw new Exception("Unknown signing algorithm");
        }

        [SqlProcedure()]
        public static void GetTrustedEndpoints(string configPath, int hashBits)
        {
            SqlDataRecord record = new SqlDataRecord(new SqlMetaData("Hash", SqlDbType.VarBinary, -1));
            SqlContext.Pipe.SendResultsStart(record);

            HashAlgorithm hash;
            switch (hashBits)
            {
                case -1:
                    hash = null;
                    break;
                case 160:
                    hash = new SHA1Cng();
                    break;
                case 256:
                    hash = new SHA256Cng();
                    break;
                case 384:
                    hash = new SHA384Cng();
                    break;
                case 512:
                    hash = new SHA512Cng();
                    break;
                default:
                    throw new ArgumentException(nameof(hashBits), "Must be in (-1, 160, 256, 384, 512)");
            }

            using (hash)
            {
                foreach (var file in Directory.GetFiles(configPath, "*.cer", SearchOption.TopDirectoryOnly))
                {
                    var crt = new X509Certificate2(file);
                    if (hash == null)
                    {
                        record.SetSqlBytes(0, new SqlBytes(crt.GetRawCertData()));
                    }
                    else
                    {
                        record.SetSqlBytes(0, new SqlBytes(hash.ComputeHash(crt.GetRawCertData())));
                    }
                    SqlContext.Pipe.SendResultsRow(record);
                }
            }

            SqlContext.Pipe.SendResultsEnd();
        }


        private static X509Certificate2 Find(string certificatePath)
        {
            var crt = new X509Certificate2(certificatePath);
            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            foreach (var cert in store.Certificates)
            {
                if (cert.HasPrivateKey)
                {
                    if (cert.Thumbprint == crt.Thumbprint)
                    {
                        return cert;
                    }
                }
            }
            throw new Exception("Certificate Not Found");
        }

        private static void Write(MemoryStream stream, string name, string data)
        {
            byte[] nameBytes = Encoding.UTF8.GetBytes(name);
            if (nameBytes.Length > 256)
                throw new Exception("String length too long");
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            if (dataBytes.Length > 65536)
                throw new Exception("String length too long");

            stream.WriteByte((byte)nameBytes.Length);
            stream.Write(nameBytes, 0, nameBytes.Length);
            stream.WriteByte((byte)(dataBytes.Length >> 8));
            stream.WriteByte((byte)(dataBytes.Length));
            stream.Write(dataBytes, 0, dataBytes.Length);
        }
      
    }





}
