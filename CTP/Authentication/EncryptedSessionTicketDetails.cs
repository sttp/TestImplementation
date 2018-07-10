using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using CTP.SRP;

namespace CTP.Authentication
{
    [DocumentName("EncryptedSessionTicketDetails")]
    public class EncryptedSessionTicketDetails
        : DocumentObject<EncryptedSessionTicketDetails>
    {
        /// <summary>
        /// The name of the credential that generated this session ticket.
        /// </summary>
        [DocumentField()] public string CredentialName { get; private set; }

        /// <summary>
        /// The HMAC key used to sign tickets. HMAC(K,'Ticket Signing')
        /// </summary>
        [DocumentField()] public byte[] TicketSigningKey { get; private set; }

        /// <summary>
        /// The HMAC key used to prove a challenge response. HMAC(K,'Challenge Response Key')
        /// </summary>
        [DocumentField()] public byte[] ChallengeResponseKey { get; private set; }

        /// <summary>
        /// The approved start time of the ticket. 
        /// </summary>
        [DocumentField()] public DateTime ValidAfter { get; private set; }

        /// <summary>
        /// The approved end time of the ticket. 
        /// </summary>
        [DocumentField()] public DateTime ValidBefore { get; private set; }

        /// <summary>
        /// The available roles for this ticket.
        /// </summary>
        [DocumentField()] public string[] Roles { get; private set; }

        public EncryptedSessionTicketDetails(string credentialName, byte[] ticketSigningKey, byte[] challengeResponseKey, DateTime validAfter, DateTime validBefore, string[] roles)
        {
            CredentialName = credentialName;
            TicketSigningKey = ticketSigningKey;
            ChallengeResponseKey = challengeResponseKey;
            ValidAfter = validAfter;
            ValidBefore = validBefore;
            Roles = roles;
        }

        private EncryptedSessionTicketDetails()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverKeyLookup">A lookup identifier to find the server's key</param>
        /// <param name="serverCipherKey">this is a key used to encrypt the ticket</param>
        /// <param name="serverMacKey">this is the MAC key used to sign the ticket.</param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] serverKeyLookup, byte[] serverCipherKey, byte[] serverMacKey)
        {
            var ms = new MemoryStream();
            ms.WriteByte((byte)serverKeyLookup.Length);
            byte[] iv = RNG.CreateSalt(16);
            ms.Write(iv, 0, iv.Length);

            byte[] ticket = ToDocument().ToArray();
            using (var aes = Aes.Create())
            {
                aes.IV = iv;
                aes.Key = serverCipherKey;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;
                using (var enc = aes.CreateEncryptor())
                {
                    var cipher = enc.TransformFinalBlock(ticket, 0, ticket.Length);
                    ms.Write(cipher, 0, cipher.Length);
                }
            }

            using (var hmac = new HMACSHA256(serverMacKey))
            {
                byte[] block = ms.ToArray();
                byte[] signKey = hmac.TransformFinalBlock(block, 0, block.Length);
                ms.Write(signKey, 0, signKey.Length);
            }

            return ms.ToArray();
        }

        public static EncryptedSessionTicketDetails TryDecrypt(byte[] ticket, byte[] serverKeyLookup, byte[] serverCipherKey, byte[] serverMacKey)
        {
            if (ticket[0] != serverKeyLookup.Length)
                return null;
            for (int x = 0; x < serverKeyLookup.Length; x++)
            {
                if (ticket[1 + x] != serverKeyLookup[x])
                    return null;
            }
            byte[] iv = new byte[16];
            Array.Copy(ticket, 1 + serverKeyLookup.Length, iv, 0, 16);
            byte[] mac = new byte[32];
            Array.Copy(ticket, ticket.Length - 32, mac, 0, 32);

            using (var hmac = new HMACSHA256(serverMacKey))
            {
                byte[] signKey = hmac.TransformFinalBlock(ticket, 0, ticket.Length - 32);
                if (!signKey.SequenceEqual(mac))
                    return null;
            }

            using (var aes = Aes.Create())
            {
                aes.IV = iv;
                aes.Key = serverCipherKey;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;
                using (var enc = aes.CreateDecryptor())
                {
                    var cipher = enc.TransformFinalBlock(ticket, 1 + serverKeyLookup.Length + 16, ticket.Length - 32 - 1 - serverKeyLookup.Length - 16);
                    return (EncryptedSessionTicketDetails)new CtpDocument(cipher);
                }
            }
        }

        public static explicit operator EncryptedSessionTicketDetails(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}