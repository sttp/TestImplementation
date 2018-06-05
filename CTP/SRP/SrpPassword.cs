using System;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using GSF.Security.Cryptography;

namespace CTP.SRP
{
    public class SrpPassword
    {
        public string Identifier;
        public byte[] X;
        public byte[] Salt;
        public int Iterations;

        public SrpVerifier CreateVerifier(SrpStrength strength)
        {
            return new SrpVerifier(Identifier, SrpMethods.ComputeV(strength, X), Salt, strength, Iterations);
        }


        public SrpPassword(byte[] salt, string identifier, SecureString password, int iterations)
        {
            Identifier = identifier;
            Salt = (byte[])salt.Clone();
            X = ComputeX(salt, identifier, password, iterations);
            Iterations = iterations;
        }

        public SrpPassword(byte[] salt, string identifier, string password, int iterations)
        {
            Identifier = identifier;
            Salt = (byte[])salt.Clone();
            X = ComputeX(salt, identifier, password, iterations);
            Iterations = iterations;
        }

        public static byte[] ComputeX(byte[] salt, string identifier, string password, int iterations)
        {
            return ComputeX(salt, identifier, Encoding.UTF8.GetBytes(password), iterations);
        }
        public static byte[] ComputeX(byte[] salt, string identifier, SecureString password, int iterations)
        {
            return ComputeX(salt, identifier, password.ToUTF8(), iterations);
        }

        private static byte[] ComputeX(byte[] salt, string identifier, byte[] password, int iterations)
        {
            byte[] pbkdf = null;
            byte[] inner = null;
            try
            {
                using (var sha = SHA512.Create())
                {
                    byte[] idBytes = Encoding.UTF8.GetBytes(identifier);
                    pbkdf = PBKDF2.Compute(HashAlgorithmName.SHA512, password, salt, iterations, 64);

                    inner = new byte[idBytes.Length + 1 + pbkdf.Length];
                    byte[] outer = new byte[salt.Length + 512 / 8];
                    idBytes.CopyTo(inner, 0);
                    inner[idBytes.Length] = (byte)':';
                    pbkdf.CopyTo(inner, idBytes.Length + 1);
                    sha.ComputeHash(inner).CopyTo(outer, salt.Length);
                    salt.CopyTo(outer, 0);

                    return sha.ComputeHash(outer);
                }
            }
            finally
            {
                if (password != null)
                    Array.Clear(password, 0, password.Length);
                if (pbkdf != null)
                    Array.Clear(pbkdf, 0, pbkdf.Length);
                if (inner != null)
                    Array.Clear(inner, 0, inner.Length);
            }

        }
    }
}
