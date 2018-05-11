using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CTP.IO;

namespace CTP.SRP
{
    public class Srp6aClient
    {
        private SrpConstants m_params;
        private BigInteger m_generator;
        private BigInteger m_prime;
        private BigInteger m_k;
        private BigInteger m_privateA;
        private BigInteger m_publicA;
        private BigInteger m_publicB;
        private BigInteger m_u;
        private BigInteger m_sessionKey;
        private string m_identity;
        private string m_password;
        private byte[] m_challengeServer;
        private byte[] m_challengeClient;

        public Srp6aClient(string identity, string password)
        {
            m_identity = identity.Normalize(NormalizationForm.FormKC).Trim().ToLower();
            m_password = password.Normalize(NormalizationForm.FormKC);
            m_privateA = RNG.CreateSalt(32).ToUnsignedBigInteger(); //RFC says SHOULD be at least 256 bit.
        }

        public void Authenticate(Stream stream)
        {
            stream.Write(m_identity);

            var strength = (SrpStrength)stream.ReadNextByte();
            var salt = stream.ReadBytes();
            var publicB = stream.ReadBytes();

            Step1(strength, out byte[] publicA);

            stream.Write(publicA);

            Step2(salt, publicB, out byte[] clientChallenge);

            stream.Write(clientChallenge);

            var serverChallenge = stream.ReadBytes();

            Step3(serverChallenge);
        }

        public void Step1(SrpStrength strength, out byte[] publicA)
        {
            m_params = SrpConstants.Lookup(strength);
            m_generator = m_params.g;
            m_prime = m_params.N;
            m_k = m_params.k;
            m_publicA = BigInteger.ModPow(m_generator, m_privateA, m_prime);
            publicA = m_publicA.ToUnsignedByteArray();
        }

        public void Step2(byte[] userSalt, byte[] publicB, out byte[] clientChallenge)
        {
            m_publicB = publicB.ToUnsignedBigInteger();
            m_u = ComputeHash(m_params.PaddedBytes, m_publicA, m_publicB).ToUnsignedBigInteger();
            var x = ComputePrivateKey(userSalt, m_identity, m_password).ToUnsignedBigInteger();
            var exp1 = (m_privateA + m_u * x % m_prime) % m_prime;
            var base1 = (m_publicB - m_k * m_generator.ModPow(x, m_prime) % m_prime) % m_prime;
            m_sessionKey = base1.ModPow(exp1, m_prime);
            m_challengeServer = ComputeHash(m_params.PaddedBytes, 1, m_sessionKey);
            m_challengeClient = ComputeHash(m_params.PaddedBytes, 2, m_sessionKey);
            clientChallenge = m_challengeClient;
        }

        public void Step3(byte[] serverChallenge)
        {
            if (!m_challengeServer.SequenceEqual(serverChallenge))
                throw new Exception("Failed server challenge");
        }

        private static byte[] ComputePrivateKey(byte[] salt, string identifier, string password)
        {
            int identifierUtfLen = Encoding.UTF8.GetByteCount(identifier);
            int passwordUtfLen = Encoding.UTF8.GetByteCount(password);

            byte[] inner = new byte[identifierUtfLen + 1 + passwordUtfLen];
            byte[] outer = new byte[salt.Length + 512 / 8];

            Encoding.UTF8.GetBytes(identifier, 0, identifier.Length, inner, 0);
            inner[identifierUtfLen] = (byte)':';
            Encoding.UTF8.GetBytes(password, 0, password.Length, inner, identifierUtfLen + 1);

            byte[] x;
            using (var sha = SHA512.Create())
            {
                sha.ComputeHash(inner).CopyTo(outer, salt.Length);
                salt.CopyTo(outer, 0);
                x = sha.ComputeHash(outer);
            }
            return x;
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
