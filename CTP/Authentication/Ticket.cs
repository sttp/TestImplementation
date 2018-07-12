using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using GSF;

namespace CTP.Authentication
{
    public class Ticket
    {
        /// <summary>
        /// Identifies the master secret the server uses to generate the crypto keys.
        /// </summary>
        public uint MasterSecretID;

        /// <summary>
        /// Gets the number of minutes that have already elapsed in this master key.
        /// </summary>
        public uint RemainingSeconds;

        /// <summary>
        /// Gets the number of minutes this key is valid for.
        /// </summary>
        public uint ExpireTimeMinutes;

        /// <summary>
        /// A sequence number that identifies which credential name created this ticket.
        /// </summary>
        public uint CredentialNameID;

        /// <summary>
        /// IDs associated with the roles granted by this ticket.
        /// </summary>
        public List<uint> Roles;

        /// <summary>
        /// A string that identifies the user of this ticket.
        /// </summary>
        public string LoginName;

        /// <summary>
        /// The signature of this ticket
        /// </summary>
        public byte[] Signature;

        public Ticket(uint masterSecretID, uint remainingSeconds, uint expireTimeMinutes, uint credentialNameID, List<uint> roles, string loginName, byte[] signature)
        {
            MasterSecretID = masterSecretID;
            RemainingSeconds = remainingSeconds;
            ExpireTimeMinutes = expireTimeMinutes;
            CredentialNameID = credentialNameID;
            Roles = roles;
            LoginName = loginName;
            Signature = signature;
        }

        public Ticket(byte[] data)
        {
            int position = 0;
            MasterSecretID = Encoding7Bit.ReadUInt32(data, ref position);
            RemainingSeconds = Encoding7Bit.ReadUInt32(data, ref position);
            ExpireTimeMinutes = Encoding7Bit.ReadUInt32(data, ref position);
            CredentialNameID = Encoding7Bit.ReadUInt32(data, ref position);
            Roles = new List<uint>();
            uint roleCount = Encoding7Bit.ReadUInt32(data, ref position);
            while (roleCount > 0)
            {
                Roles.Add(Encoding7Bit.ReadUInt32(data, ref position));
                roleCount--;
            }
            uint stringLen = Encoding7Bit.ReadUInt32(data, ref position);
            LoginName = Encoding.UTF8.GetString(data, position, (int)stringLen);
            position += (int)stringLen;
            Signature = new byte[data.Length - position];
            Array.Copy(data, position, Signature, 0, Signature.Length);
        }

        public byte[] ToArray()
        {
            var ms = new MemoryStream();
            Encoding7Bit.Write(ms.WriteByte, MasterSecretID);
            Encoding7Bit.Write(ms.WriteByte, RemainingSeconds);
            Encoding7Bit.Write(ms.WriteByte, ExpireTimeMinutes);
            Encoding7Bit.Write(ms.WriteByte, CredentialNameID);
            Encoding7Bit.Write(ms.WriteByte, (uint)Roles.Count);
            foreach (var role in Roles)
            {
                Encoding7Bit.Write(ms.WriteByte, role);
            }
            byte[] loginName = Encoding.UTF8.GetBytes(LoginName);
            Encoding7Bit.Write(ms.WriteByte, (uint)loginName.Length);
            ms.Write(loginName, 0, loginName.Length);
            ms.Write(Signature, 0, Signature.Length);
            return ms.ToArray();
        }

        public byte[] Sign(byte[] signatureKey)
        {
            var ms = new MemoryStream();
            Encoding7Bit.Write(ms.WriteByte, MasterSecretID);
            Encoding7Bit.Write(ms.WriteByte, RemainingSeconds);
            Encoding7Bit.Write(ms.WriteByte, ExpireTimeMinutes);
            Encoding7Bit.Write(ms.WriteByte, CredentialNameID);
            Encoding7Bit.Write(ms.WriteByte, (uint)Roles.Count);
            foreach (var role in Roles)
            {
                Encoding7Bit.Write(ms.WriteByte, role);
            }
            byte[] loginName = Encoding.UTF8.GetBytes(LoginName);
            Encoding7Bit.Write(ms.WriteByte, (uint)loginName.Length);
            ms.Write(loginName, 0, loginName.Length);
            using (var mac = new HMACSHA256(signatureKey))
            {
                byte[] data = ms.ToArray();
                Signature = mac.ComputeHash(data, 0, data.Length);
            }
            ms.Write(Signature, 0, Signature.Length);
            return ms.ToArray();
        }

        public bool VerifySignature(byte[] signatureKey)
        {
            var ms = new MemoryStream();
            Encoding7Bit.Write(ms.WriteByte, MasterSecretID);
            Encoding7Bit.Write(ms.WriteByte, RemainingSeconds);
            Encoding7Bit.Write(ms.WriteByte, ExpireTimeMinutes);
            Encoding7Bit.Write(ms.WriteByte, CredentialNameID);
            Encoding7Bit.Write(ms.WriteByte, (uint)Roles.Count);
            foreach (var role in Roles)
            {
                Encoding7Bit.Write(ms.WriteByte, role);
            }
            byte[] loginName = Encoding.UTF8.GetBytes(LoginName);
            Encoding7Bit.Write(ms.WriteByte, (uint)loginName.Length);
            ms.Write(loginName, 0, loginName.Length);
            using (var mac = new HMACSHA256(signatureKey))
            {
                byte[] data = ms.ToArray();

                var signature = mac.ComputeHash(data, 0, data.Length);
                return Signature.SequenceEqual(signature);
            }
        }
    }
}
