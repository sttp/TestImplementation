using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using CTP.Net;
using GSF.IO;

namespace CTP.SRP
{
    public class SrpServer<T>
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
            m_pairing[credentials.Verifier.Identifier] = credentials;
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
            var credentials = new SrpUserCredential<T>(username, password, GenerateSalt(username), m_srpStrength, 1000, token);
            m_users[credentials.Verifier.Identifier] = credentials;
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
            var credentials = new SrpUserCredential<T>(username, verification, salt, srpStrength, 1000, token);
            m_users[credentials.Verifier.Identifier] = credentials;
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

            var param = SrpConstants.Lookup(user.Verifier.SrpStrength);
            var verifier = user.Verifier.Verification.ToUnsignedBigInteger();
            var privateB = RNG.CreateSalt(32).ToUnsignedBigInteger();
            var publicB = param.k.ModMul(verifier, param.N).ModAdd(param.g.ModPow(privateB, param.N), param.N);
            stream.Write((ushort)user.Verifier.SrpStrength);
            stream.WriteWithLength(user.Verifier.Salt);
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

        public CtpCommandHandlerBase Authenticate(CtpSession session, SrpIdentity command, X509Certificate clientCertificate, X509Certificate serverCertificate, Action<byte[], T> userAuthenticated)
        {
            var identity = command;
            string userName = identity.UserName.Normalize(NormalizationForm.FormKC).Trim().ToLower();

            if (!m_users.TryGetValue(userName, out var user))
            {
                Console.WriteLine("User Not Found, Generating erroneous data");
                user = new SrpUserCredential<T>(userName, Guid.NewGuid().ToString(), GenerateSalt(userName), m_srpStrength, 1000, default(T));
            }

            var items = new SrpServerHandler<T>(session, user, userAuthenticated);
            items.Start();
            return items;
        }
    }

    public class SrpServerHandler<T> : CtpCommandHandlerBase
    {
        private Action<byte[], T> m_userAuthenticated;
        private int m_state = 0;
        private CtpSession m_stream;
        private SrpUserCredential<T> m_user;
        private byte[] privateSessionKey;
        private SrpConstants param;
        private BigInteger verifier;
        private BigInteger privateB;
        private BigInteger publicB;

        public SrpServerHandler(CtpSession stream, SrpUserCredential<T> user, Action<byte[], T> userAuthenticated)
        {
            m_stream = stream;
            m_user = user;
            m_userAuthenticated = userAuthenticated;
        }

        public void Start()
        {
            m_state = 1;
            param = SrpConstants.Lookup(m_user.Verifier.SrpStrength);
            verifier = m_user.Verifier.Verification.ToUnsignedBigInteger();
            privateB = RNG.CreateSalt(32).ToUnsignedBigInteger();
            publicB = param.k.ModMul(verifier, param.N).ModAdd(param.g.ModPow(privateB, param.N), param.N);
            m_stream.SendCommand(new SrpIdentityLookup(m_user.Verifier.SrpStrength, m_user.Verifier.Salt, publicB.ToUnsignedByteArray(), m_user.Verifier.IterationCount));
        }

        public override CtpCommandHandlerBase ProcessCommand(CtpSession session, CtpDocument command)
        {
            switch (m_state)
            {
                case 1:
                    m_state++;
                    var clientResponse = (SrpClientResponse)command;

                    var publicA = clientResponse.PublicA.ToUnsignedBigInteger();
                    byte[] clientChallenge = clientResponse.ClientChallenge;

                    var u = SrpMethods.ComputeU(param.PaddedBytes, publicA, publicB);
                    var sessionKey = publicA.ModMul(verifier.ModPow(u, param.N), param.N).ModPow(privateB, param.N);

                    var challengeServer = SrpMethods.ComputeChallenge(1, sessionKey, m_stream.RemoteCertificate, m_stream.LocalCertificate);
                    var challengeClient = SrpMethods.ComputeChallenge(2, sessionKey, m_stream.RemoteCertificate, m_stream.LocalCertificate);
                    privateSessionKey = SrpMethods.ComputeChallenge(3, sessionKey, m_stream.RemoteCertificate, m_stream.LocalCertificate);

                    if (!challengeClient.SequenceEqual(clientChallenge))
                        throw new Exception("Failed client challenge");
                    byte[] serverChallenge = challengeServer;

                    m_userAuthenticated(privateSessionKey, m_user.Token);
                    m_stream.SendCommand(new SrpServerResponse(serverChallenge));
                    return null;
                default:
                    throw new NotSupportedException();
            }
        }

        public override void Cancel()
        {
        }
    }

}