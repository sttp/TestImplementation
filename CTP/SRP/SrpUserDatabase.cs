using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using CTP.IO;

namespace CTP.SRP
{
    public class SrpUserDatabase<T>
    {
        private Dictionary<string, Tuple<T, SrpUserCredential>> m_users = new Dictionary<string, Tuple<T, SrpUserCredential>>();

        private byte[] m_srpDefaultSalt = RNG.CreateSalt(64);
        private SrpStrength m_srpStrength = SrpStrength.Bits1024;

        public void SetSrpDefaults(byte[] salt, SrpStrength strength)
        {
            m_srpDefaultSalt = salt ?? RNG.CreateSalt(64);
            m_srpStrength = strength;
        }

        /// <summary>
        /// Creates user credentials
        /// </summary>
        /// <param name="username"></param>
        /// <param name="salt"></param>
        /// <param name="verification"></param>
        /// <param name="srpStrength"></param>
        /// <param name="token"></param>
        public void AddUser(string username, byte[] verification, byte[] salt, SrpStrength srpStrength, T token)
        {
            var credentials = new SrpUserCredential(username, verification, salt, srpStrength);
            m_users[credentials.UserName] = new Tuple<T, SrpUserCredential>(token, credentials);
        }

        /// <summary>
        /// Creates a user credential from the provided data.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public void AddUser(string username, string password, T token)
        {
            using (var sha = SHA512.Create())
            {
                string un = username.Normalize(NormalizationForm.FormKC).Trim().ToLower();
                byte[] data = Encoding.UTF8.GetBytes(un);
                byte[] rv = new byte[data.Length + m_srpDefaultSalt.Length];
                data.CopyTo(rv, 0);
                m_srpDefaultSalt.CopyTo(rv, data.Length);
                byte[] salt = sha.ComputeHash(m_srpDefaultSalt);
                var credentials = new SrpUserCredential(username, password, salt, m_srpStrength);
                m_users[credentials.UserName] = new Tuple<T, SrpUserCredential>(token, credentials);
            }
        }

        public void RemoveUser(string username)
        {
            m_users.Remove(username);
        }

        public T Authenticate(Stream stream)
        {
            string userName = stream.ReadString();
            userName = userName.Normalize(NormalizationForm.FormKC).Trim().ToLower();
            if (!m_users.TryGetValue(userName, out var user))
            {
                using (var sha = SHA512.Create())
                {
                    byte[] data = Encoding.UTF8.GetBytes(userName);
                    byte[] rv = new byte[data.Length + m_srpDefaultSalt.Length];
                    data.CopyTo(rv, 0);
                    m_srpDefaultSalt.CopyTo(rv, data.Length);
                    byte[] salt = sha.ComputeHash(m_srpDefaultSalt);
                    var credentials = new SrpUserCredential(userName, Guid.NewGuid().ToString(), salt, m_srpStrength);
                    user = new Tuple<T, SrpUserCredential>(default(T), credentials);
                }
            }
            var server = new Srp6aServer(user.Item2);
            server.Step1(out var srpStrength, out byte[] userSalt, out byte[] publicB);
            stream.Write((byte)srpStrength);
            stream.Write(userSalt);
            stream.Write(publicB);

            byte[] publicA = stream.ReadBytes();
            byte[] clientChallenge = stream.ReadBytes();

            server.Step2(publicA, clientChallenge, out byte[] serverChallenge);

            stream.Write(serverChallenge);
            return user.Item1;
        }


    }
}