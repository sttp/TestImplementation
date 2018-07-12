using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using CTP.Net;
using GSF.IO;

namespace CTP.SRP
{
    public class SrpServer
    {
        private int m_credentialNameID;
        private Dictionary<string, SrpCredential> m_credentials = new Dictionary<string, SrpCredential>();
        private Dictionary<string, SrpPairingCredential> m_pairingCredentials = new Dictionary<string, SrpPairingCredential>();

        private byte[] m_srpDefaultSalt = Security.CreateSalt(64);
        private SrpStrength m_srpStrength = SrpStrength.Bits1024;

        public void SetSrpDefaults(byte[] salt, SrpStrength strength)
        {
            m_srpDefaultSalt = salt ?? Security.CreateSalt(64);
            m_srpStrength = strength;
        }

        public void AddPairingUser(string pairingID, string paringPin, DateTime expireTime, string assignedCredentialName, string loginName, string[] roles)
        {
            var credentials = new SrpPairingCredential(pairingID, paringPin, expireTime, assignedCredentialName, loginName, roles);
            m_pairingCredentials[credentials.Verifier.CredentialName] = credentials;
        }

        /// <summary>
        /// Creates a user credential from the provided data.
        /// </summary>
        /// <param name="credentialName"></param>
        /// <param name="secret"></param>
        /// <param name="loginName"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public void AddCredential(string credentialName, string secret, string loginName, string[] roles)
        {
            var credentials = new SrpCredential((uint)Interlocked.Increment(ref m_credentialNameID), credentialName, secret, GenerateSalt(credentialName), m_srpStrength, loginName, roles);
            m_credentials[credentials.Verifier.CredentialName.ToLower()] = credentials;
        }

        /// <summary>
        /// Creates user credentials
        /// </summary>
        /// <param name="credentialName"></param>
        /// <param name="salt"></param>
        /// <param name="verifierCode"></param>
        /// <param name="srpStrength"></param>
        /// <param name="loginName"></param>
        /// <param name="roles"></param>
        public void AddCredential(string credentialName, byte[] verifierCode, byte[] salt, SrpStrength srpStrength, string loginName, string[] roles)
        {
            var credentials = new SrpCredential((uint)Interlocked.Increment(ref m_credentialNameID), credentialName, verifierCode, salt, srpStrength, loginName, roles);
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

        public SrpCredential LookupCredential(Auth command)
        {
            var identity = command;
            string userName = identity.CredentialName.Normalize(NormalizationForm.FormKC).Trim().ToLower();

            if (!m_credentials.TryGetValue(userName, out var user))
            {
                Console.WriteLine("User Not Found, Generating erroneous data");
                user = new SrpCredential(0, userName, Guid.NewGuid().ToString(), GenerateSalt(userName), m_srpStrength, null, null);
            }

            return user;
        }

    }
}