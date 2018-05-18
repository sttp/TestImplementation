using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using GSF.IO;

namespace CTP.SRP
{
    public class Srp6aServer<T>
    {
        private Dictionary<string, SrpUserCredential<T>> m_users = new Dictionary<string, SrpUserCredential<T>>();
        private Dictionary<string, SrpPairingCredential<T>> m_pairing = new Dictionary<string, SrpPairingCredential<T>>();

        private byte[] m_srpDefaultSalt = RNG.CreateSalt(64);
        private SrpStrength m_srpStrength = SrpStrength.Bits1024;

        public void SetSrpDefaults(byte[] salt, SrpStrength strength)
        {
            m_srpDefaultSalt = salt ?? RNG.CreateSalt(64);
            m_srpStrength = strength;
        }

        public void AddPairingUser(string pairingID, string paringPin, DateTime expireTime, string assignedUserName, bool allowCertificatePairing, bool allowSessionPairing, T token)
        {
            var credentials = new SrpPairingCredential<T>(pairingID, paringPin, expireTime, assignedUserName, allowCertificatePairing, allowSessionPairing, token);
            m_pairing[credentials.PairingID] = credentials;
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
            var credentials = new SrpUserCredential<T>(username, password, GenerateSalt(username), m_srpStrength, token);
            m_users[credentials.UserName] = credentials;
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
            var credentials = new SrpUserCredential<T>(username, verification, salt, srpStrength, token);
            m_users[credentials.UserName] = credentials;
        }

        public void RemoveUser(string username)
        {
            m_users.Remove(username);
        }

        private byte[] GenerateSalt(string username)
        {
            using (var sha = SHA512.Create())
            {
                string un = username.Normalize(NormalizationForm.FormKC).Trim().ToLower();
                byte[] data = Encoding.UTF8.GetBytes(un);
                byte[] rv = new byte[data.Length + m_srpDefaultSalt.Length];
                data.CopyTo(rv, 0);
                m_srpDefaultSalt.CopyTo(rv, data.Length);
                return sha.ComputeHash(m_srpDefaultSalt);
            }
        }

        public SrpPairingCredential<T> Pairing(Stream stream, X509Certificate clientCertificate, X509Certificate serverCertificate, out byte[] privateSessionKey)
        {
            string userName = stream.ReadString();
            userName = userName.Normalize(NormalizationForm.FormKC).Trim().ToLower();

            if (!m_pairing.TryGetValue(userName, out var user))
            {
                Console.WriteLine("User Not Found, Generating erroneous data");
                user = new SrpPairingCredential<T>(userName, Guid.NewGuid().ToString(), DateTime.MinValue, "", false, false, default(T));
            }
            else
            {
                m_pairing.Remove(userName); //Pairing is only allowed once per session.
            }
            
            var param = SrpConstants.Lookup(user.SrpStrength);
            var verifier = user.Verification.ToUnsignedBigInteger();
            var privateB = RNG.CreateSalt(32).ToUnsignedBigInteger();
            var publicB = param.k.ModMul(verifier, param.N).ModAdd(param.g.ModPow(privateB, param.N), param.N);
            stream.Write((ushort)user.SrpStrength);
            stream.WriteWithLength(user.Salt);
            stream.WriteWithLength(publicB.ToUnsignedByteArray());
            stream.Flush();

            var publicA = stream.ReadBytes().ToUnsignedBigInteger();
            byte[] clientChallenge = stream.ReadBytes();

            var u = SrpMethods.ComputeU(param.PaddedBytes, publicA, publicB);
            var sessionKey = publicA.ModMul(verifier.ModPow(u, param.N), param.N).ModPow(privateB, param.N);

            var challengeServer = SrpMethods.ComputeChallenge(1, sessionKey, clientCertificate, serverCertificate);
            var challengeClient = SrpMethods.ComputeChallenge(2, sessionKey, clientCertificate, serverCertificate);
            privateSessionKey = SrpMethods.ComputeChallenge(3, sessionKey, clientCertificate, serverCertificate);

            if (!challengeClient.SequenceEqual(clientChallenge))
                throw new Exception("Failed client challenge");
            byte[] serverChallenge = challengeServer;

            stream.WriteWithLength(serverChallenge);
            stream.Flush();

            return user;
        }


        public T Authenticate(Stream stream, X509Certificate clientCertificate, X509Certificate serverCertificate)
        {
            return Authenticate(stream, clientCertificate, serverCertificate, out byte[] privateSessionKey);
        }

        public T Authenticate(Stream stream, X509Certificate clientCertificate, X509Certificate serverCertificate, out byte[] privateSessionKey)
        {
            string userName = stream.ReadString();
            userName = userName.Normalize(NormalizationForm.FormKC).Trim().ToLower();

            if (!m_users.TryGetValue(userName, out var user))
            {
                Console.WriteLine("User Not Found, Generating erroneous data");
                user = new SrpUserCredential<T>(userName, Guid.NewGuid().ToString(), GenerateSalt(userName), m_srpStrength, default(T));
            }

            var param = SrpConstants.Lookup(user.SrpStrength);
            var verifier = user.Verification.ToUnsignedBigInteger();
            var privateB = RNG.CreateSalt(32).ToUnsignedBigInteger();
            var publicB = param.k.ModMul(verifier, param.N).ModAdd(param.g.ModPow(privateB, param.N), param.N);
            stream.Write((ushort)user.SrpStrength);
            stream.WriteWithLength(user.Salt);
            stream.WriteWithLength(publicB.ToUnsignedByteArray());
            stream.Flush();

            var publicA = stream.ReadBytes().ToUnsignedBigInteger();
            byte[] clientChallenge = stream.ReadBytes();

            var u = SrpMethods.ComputeU(param.PaddedBytes, publicA, publicB);
            var sessionKey = publicA.ModMul(verifier.ModPow(u, param.N), param.N).ModPow(privateB, param.N);

            var challengeServer = SrpMethods.ComputeChallenge(1, sessionKey, clientCertificate, serverCertificate);
            var challengeClient = SrpMethods.ComputeChallenge(2, sessionKey, clientCertificate, serverCertificate);
            privateSessionKey = SrpMethods.ComputeChallenge(3, sessionKey, clientCertificate, serverCertificate);

            if (!challengeClient.SequenceEqual(clientChallenge))
                throw new Exception("Failed client challenge");
            byte[] serverChallenge = challengeServer;

            stream.WriteWithLength(serverChallenge);
            stream.Flush();

            return user.Token;
        }


    }
}