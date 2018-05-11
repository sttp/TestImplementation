using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace CTP.SRP
{
    public class Srp6aServer
    {
        private SrpUserCredential m_user;
        private BigInteger m_generator;
        private BigInteger m_prime;
        private BigInteger m_k;
        private BigInteger m_privateB;
        private BigInteger m_publicB;
        private BigInteger m_publicA;
        private BigInteger m_verifier;
        private BigInteger m_u;
        private BigInteger m_sessionKey;
        private int m_paddedBytes;

        private byte[] m_challengeServer;
        private byte[] m_challengeClient;

        public Srp6aServer(SrpUserCredential user)
        {
            SrpConstants param = SrpConstants.Lookup(user.SrpStrength);
            m_user = user;
            m_generator = param.g;
            m_prime = param.N;
            m_k = param.k;
            m_paddedBytes = param.PaddedBytes;
            m_verifier = user.Verification.ToUnsignedBigInteger();

            m_privateB = RNG.CreateSalt(32).ToUnsignedBigInteger(); //RFC says SHOULD be at least 256 bit.
            m_publicB = (m_k * m_verifier % m_prime + m_generator.ModPow(m_privateB, m_prime)) % m_prime;
        }

        public void Step1(out byte[] userSalt, out byte[] publicB)
        {
            userSalt = m_user.Salt;
            publicB = m_publicB.ToUnsignedByteArray();
        }

        public void Step2(byte[] publicA, byte[] clientChallenge, out byte[] serverChallenge)
        {
            m_publicA = publicA.ToUnsignedBigInteger();
            m_u = ComputeHash(m_paddedBytes, m_publicA, m_publicB).ToUnsignedBigInteger();
            m_sessionKey = (m_publicA * m_verifier.ModPow(m_u, m_prime) % m_prime).ModPow(m_privateB, m_prime);
            m_challengeServer = ComputeHash(m_paddedBytes, 1, m_sessionKey);
            m_challengeClient = ComputeHash(m_paddedBytes, 2, m_sessionKey);

            if (!m_challengeClient.SequenceEqual(clientChallenge))
                throw new Exception("Failed client challenge");
            serverChallenge = m_challengeServer;
        }

        private static byte[] ComputeHash(int padLength, BigInteger item1, BigInteger item2)
        {
            using (var sha = SHA512.Create())
            {
                byte[] nb = item1.ToByteArray();
                byte[] gb = item2.ToByteArray();
                byte[] hash = new byte[padLength * 2];
                nb.CopyTo(hash, padLength - nb.Length);
                gb.CopyTo(hash, hash.Length - gb.Length);
                return sha.ComputeHash(hash);
            }
        }
    }
}
