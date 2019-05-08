using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using GSF.IO;

namespace CTP
{
    public class AuthorizationTicket
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
}
