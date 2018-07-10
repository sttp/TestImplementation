using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using CTP.Net;
using GSF.IO;

namespace CTP.SRP
{
    public class SrpServer<T>
    {
        private Dictionary<string, SrpCredential<T>> m_credentials = new Dictionary<string, SrpCredential<T>>();
        private Dictionary<string, SrpPairingCredential<T>> m_pairingCredentials = new Dictionary<string, SrpPairingCredential<T>>();

        private byte[] m_srpDefaultSalt = RNG.CreateSalt(64);
        private SrpStrength m_srpStrength = SrpStrength.Bits1024;

        public void SetSrpDefaults(byte[] salt, SrpStrength strength)
        {
            m_srpDefaultSalt = salt ?? RNG.CreateSalt(64);
            m_srpStrength = strength;
        }

        public void AddPairingUser(string pairingID, string paringPin, DateTime expireTime, string assignedCredentialName, T token)
        {
            var credentials = new SrpPairingCredential<T>(pairingID, paringPin, expireTime, assignedCredentialName, token);
            m_pairingCredentials[credentials.Verifier.CredentialName] = credentials;
        }

        /// <summary>
        /// Creates a user credential from the provided data.
        /// </summary>
        /// <param name="credentialName"></param>
        /// <param name="secret"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public void AddCredential(string credentialName, string secret, T token)
        {
            var credentials = new SrpCredential<T>(credentialName, secret, GenerateSalt(credentialName), m_srpStrength, token);
            m_credentials[credentials.Verifier.CredentialName.ToLower()] = credentials;
        }

        /// <summary>
        /// Creates user credentials
        /// </summary>
        /// <param name="credentialName"></param>
        /// <param name="salt"></param>
        /// <param name="verifierCode"></param>
        /// <param name="srpStrength"></param>
        /// <param name="token"></param>
        public void AddCredential(string credentialName, byte[] verifierCode, byte[] salt, SrpStrength srpStrength, T token)
        {
            var credentials = new SrpCredential<T>(credentialName, verifierCode, salt, srpStrength, token);
            m_credentials[credentials.Verifier.CredentialName.ToLower()] = credentials;
        }

        public void RemoveCredential(string credentialName)
        {
            m_credentials.Remove(credentialName.Normalize(NormalizationForm.FormKC).Trim().ToLower());
        }

        private byte[] GenerateSalt(string credentialName)
        {
            using (var sha = SHA512.Create())
            {
                string un = credentialName.Normalize(NormalizationForm.FormKC).Trim().ToLower();
                byte[] data = Encoding.UTF8.GetBytes(un);
                byte[] rv = new byte[data.Length + m_srpDefaultSalt.Length];
                data.CopyTo(rv, 0);
                m_srpDefaultSalt.CopyTo(rv, data.Length);
                return sha.ComputeHash(m_srpDefaultSalt);
            }
        }

        public SrpCredential<T> LookupCredential(Auth command)
        {
            var identity = command;
            string userName = identity.CredentialName.Normalize(NormalizationForm.FormKC).Trim().ToLower();

            if (!m_credentials.TryGetValue(userName, out var user))
            {
                Console.WriteLine("User Not Found, Generating erroneous data");
                user = new SrpCredential<T>(userName, Guid.NewGuid().ToString(), GenerateSalt(userName), m_srpStrength, default(T));
            }

            return user;
        }

    }
}