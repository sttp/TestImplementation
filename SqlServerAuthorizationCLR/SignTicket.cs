using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;
using Microsoft.SqlServer.Server;

namespace SqlServerAuthorizationCLR
{
    public static class SignTicket
    {
        private class AuthorizationTicket
        {
            /// <summary>
            /// Gets the UTC time this ticket is valid from.
            /// </summary>
            public DateTime ValidFrom { get; private set; }

            /// <summary>
            /// Gets the UTC time this ticket is valid until.
            /// </summary>
            public DateTime ValidTo { get; private set; }

            /// <summary>
            /// A string that identifies the user of this ticket.
            /// </summary>
            public string LoginName { get; private set; }

            /// <summary>
            /// The list of roles granted by this ticket. If blank, all roles will be assumed.
            /// </summary>
            public List<string> Roles { get; private set; }

            /// <summary>
            /// If specified, this is the public key that the client must be using.
            /// If blank, the client does not have to supply a client side certificate.
            /// This can be used to prevent hijacking credentials.
            /// </summary>
            public string ApprovedPublicKey { get; private set; }

            public AuthorizationTicket(byte[] data)
            {
                ValidFrom = DateTime.MinValue;
                ValidTo = DateTime.MaxValue;
                LoginName = "";
                Roles = new List<string>();
                ApprovedPublicKey = "";

                var ms = new MemoryStream(data);
                while (TryRead(ms, out var name, out var value))
                {
                    if (name.Length < 1)
                        throw new Exception("Name cannot be an empty string");
                    switch (name.ToLower())
                    {
                        case "salt":
                            break;
                        case "validfrom":
                            ValidFrom = DateTime.Parse(value);
                            break;
                        case "validto":
                            ValidTo = DateTime.Parse(value);
                            break;
                        case "loginname":
                            LoginName = value;
                            break;
                        case "role":
                            Roles.Add(value);
                            break;
                        case "approvedpublickey":
                            ApprovedPublicKey = value;
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

            public AuthorizationTicket(DateTime? validFrom, DateTime? validTo, string loginName, List<string> roles, string approvedPublicKey)
            {
                ValidFrom = validFrom ?? DateTime.MinValue;
                ValidTo = validTo ?? DateTime.MaxValue;
                LoginName = loginName ?? string.Empty;
                Roles = roles ?? new List<string>();
                ApprovedPublicKey = approvedPublicKey ?? string.Empty;
            }

            public byte[] ToArray()
            {
                var ms = new MemoryStream();
                using (var rng = RandomNumberGenerator.Create())
                {
                    byte[] salt = new byte[16];
                    rng.GetBytes(salt);
                    Write(ms, "salt", Convert.ToBase64String(salt)); //Salt means the data can be ignored.
                }
                if (ValidFrom != DateTime.MinValue)
                {
                    Write(ms, "ValidFrom", ValidFrom.ToString("O"));
                }
                if (ValidTo != DateTime.MaxValue)
                {
                    Write(ms, "ValidTo", ValidTo.ToString("O"));
                }
                if (!string.IsNullOrWhiteSpace(LoginName))
                {
                    Write(ms, "LoginName", LoginName);
                }
                if (Roles != null)
                {
                    foreach (var role in Roles)
                    {
                        if (!string.IsNullOrWhiteSpace(role))
                            Write(ms, "Role", role);
                    }
                }
                if (!string.IsNullOrWhiteSpace(ApprovedPublicKey))
                {
                    Write(ms, "ApprovedPublicKey", ApprovedPublicKey);
                }
                return ms.ToArray();
            }

            private void Write(MemoryStream stream, string name, string data)
            {
                byte[] nameBytes = Encoding.UTF8.GetBytes(name);
                if (nameBytes.Length > 65536)
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

            private bool TryRead(MemoryStream stream, out string name, out string data)
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

        [SqlProcedure()]
        public static void Sign(out byte[] ticket, out byte[] signature, string configPath, DateTime validFrom, DateTime validTo, string loginName, string rolesQuery, string approvedPublicKey)
        {
            var allowedRoles = new List<string>();

            if (string.IsNullOrWhiteSpace(rolesQuery))
            {
                var roles = new DataTable();
                using (var connection = new SqlConnection("context connection=true"))
                {
                    connection.Open();
                    var ta = new SqlDataAdapter(rolesQuery, connection);
                    ta.Fill(roles);
                    connection.Close();
                }
                foreach (DataRow dataRow in roles.Rows)
                {
                    allowedRoles.Add(dataRow[0].ToString());
                }
            }

            ticket = new AuthorizationTicket(validFrom, validTo, loginName, allowedRoles, approvedPublicKey).ToArray();

            var cert = Find(configPath);

            using (var ecdsa = cert.GetECDsaPrivateKey())
            {
                if (ecdsa != null)
                {
                    signature = ecdsa.SignData(ticket, HashAlgorithmName.SHA256);
                    return;
                }
            }
            using (var rsa = cert.GetRSAPrivateKey())
            {
                if (rsa != null)
                {
                    signature = rsa.SignData(ticket, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                    return;
                }
            }
            throw new Exception("Unknown signing algorithm");
        }

        [SqlProcedure()]
        public static void GetTrustedEndpoints(string configPath)
        {
            SqlDataRecord record = new SqlDataRecord(new SqlMetaData("Thumbprint", SqlDbType.NVarChar, -1), new SqlMetaData("Public Key", SqlDbType.NVarChar, -1));
            SqlContext.Pipe.SendResultsStart(record);

            foreach (var file in Directory.GetFiles(configPath, "*.cer", SearchOption.TopDirectoryOnly))
            {
                var crt = new X509Certificate2(file);
                record.SetString(0, crt.Thumbprint);
                record.SetString(1, crt.GetPublicKeyString());

                SqlContext.Pipe.SendResultsRow(record);

            }
            SqlContext.Pipe.SendResultsEnd();
        }


        private static X509Certificate2 Find(string configPath)
        {
            var crt = new X509Certificate2(Path.Combine(configPath, "Main.cer"));
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
    }





}
