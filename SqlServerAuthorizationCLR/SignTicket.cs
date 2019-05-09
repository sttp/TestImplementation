using System;
using System.Collections.Generic;
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
            var allowedRoles = new List<string>();
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

        /// <summary>
        /// Read a byte from the stream.
        /// Will throw an exception if the end of the stream has been reached.
        /// </summary>
        /// <param name="stream">the stream to read from.</param>
        /// <returns>the value read</returns>
        private static byte ReadNextByte(this Stream stream)
        {
            int num = stream.ReadByte();
            if (num < 0)
                throw new EndOfStreamException();
            return (byte)num;
        }

        /// <summary>
        /// Reads a byte array from a <see cref="T:System.IO.Stream" />.
        /// The number of bytes should be prefixed in the stream.
        /// </summary>
        /// <param name="stream">the stream to read from</param>
        /// <param name="length">gets the number of bytes to read.</param>
        /// <returns>A new array containing the bytes.</returns>
        private static byte[] ReadBytes(this Stream stream, int length)
        {
            if (length < 0)
                throw new Exception("Invalid length");
            byte[] buffer = new byte[length];
            if (length > 0)
                stream.ReadAll(buffer, 0, buffer.Length);
            return buffer;
        }

        /// <summary>
        /// Reads all of the provided bytes. Will not return prematurely,
        /// but continue to execute a <see cref="M:System.IO.Stream.Read(System.Byte[],System.Int32,System.Int32)" /> command until the entire
        /// <paramref name="length" /> has been read.
        /// </summary>
        /// <param name="stream">The stream to read from</param>
        /// <param name="buffer">The buffer to write to</param>
        /// <param name="position">the start position in the <paramref name="buffer" /></param>
        /// <param name="length">the number of bytes to read</param>
        /// <exception cref="T:System.IO.EndOfStreamException">occurs if the end of the stream has been reached.</exception>
        private static void ReadAll(this Stream stream, byte[] buffer, int position, int length)
        {
            while (length > 0)
            {
                int num = stream.Read(buffer, position, length);
                if (num == 0)
                    throw new EndOfStreamException();
                length -= num;
                position += num;
            }
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

        private static bool TryRead(MemoryStream stream, out string name, out string data)
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
            byte[] dataBytes = stream.ReadBytes(length);
            name = Encoding.UTF8.GetString(nameBytes);
            data = Encoding.UTF8.GetString(dataBytes);
            return true;
        }
    }





}
