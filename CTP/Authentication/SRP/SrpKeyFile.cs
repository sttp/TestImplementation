using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTP.SRP;

namespace CTP.Authentication.SRP
{
    public class SrpKeyFile
    {
        public readonly SrpStrength Strength;

        public byte[] Salt => (byte[])m_salt.Clone();

        public byte[] Verifier => (byte[])m_verifier.Clone();

        public byte[] Secret => (byte[])m_secret.Clone();

        public readonly DateTime LastChangedDate;

        public readonly bool IsKeyMaterial;

        private readonly byte[] m_salt;
        private readonly byte[] m_verifier;
        private readonly byte[] m_secret;

        public byte[] Export(bool exportPrivateKey, string filePassword = null)
        {
            throw new NotImplementedException();
        }

        private SrpKeyFile(string secret, SrpStrength strength)
        {
            m_salt = Security.CreateSalt(32);
            Strength = strength;
            m_secret = SrpMethods.ComputeX(Salt, secret);
            m_verifier = SrpMethods.ComputeV(strength, m_secret);

            LastChangedDate = DateTime.UtcNow;
            IsKeyMaterial = false;
        }

        private SrpKeyFile(byte[] x, SrpStrength strength)
        {
            m_salt = new byte[0];
            m_secret = x;
            m_verifier = SrpMethods.ComputeV(strength, Secret);

            LastChangedDate = DateTime.UtcNow;
            IsKeyMaterial = true;
        }

        public static SrpKeyFile CreateKey(SrpStrength strength, string additionalEntropy = null)
        {
            var x = SrpMethods.ComputeX(Security.CreateSalt(64), additionalEntropy ?? string.Empty);
            return new SrpKeyFile(x, strength);
        }

        public static SrpKeyFile CreatePassword(SrpStrength strength, string password)
        {
            return new SrpKeyFile(password, strength);
        }

        public static SrpKeyFile Open(byte[] data, bool importPrivateKey = false, string filePassword = null)
        {
            throw new NotImplementedException();
        }
    }
}
