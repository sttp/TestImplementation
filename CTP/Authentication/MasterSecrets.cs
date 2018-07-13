using System.Diagnostics;
using System.Security.Cryptography;
using CTP.SRP;
using GSF;

namespace CTP.Net
{
    public class MasterSecrets
    {
        public readonly uint ID;
        private readonly Stopwatch m_creationTime;
        public readonly int ExpireMinutes;
        private readonly byte[] m_secretKey;

        public bool Expired => m_creationTime.Elapsed.TotalMinutes > ExpireMinutes;

        public bool NearExpiration => m_creationTime.Elapsed.TotalMinutes > (ExpireMinutes >> 1);
        public uint RemainingSeconds => (uint)(60 * ExpireMinutes - (int)m_creationTime.Elapsed.TotalSeconds);

        public MasterSecrets(uint id, int expireMinutes)
        {
            ID = id;
            ExpireMinutes = expireMinutes;
            m_creationTime = Stopwatch.StartNew();
            m_secretKey = Security.CreateSalt(64);
        }

        public byte[] GetSignatureKey(uint credentialNameID)
        {
            using (var hmac = new HMACSHA256(m_secretKey))
            {
                return hmac.ComputeHash(BigEndian.GetBytes(credentialNameID));
            }
        }
    }
}