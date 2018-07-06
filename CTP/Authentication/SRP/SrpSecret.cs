using System;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace CTP.SRP
{
    public class SrpSecret
    {
        public string CredentialName;

        public byte[] Salt;
        /// <summary>
        /// Corresponds to the Private Key
        /// </summary>
        public byte[] X;

        public SrpVerifier CreateVerifier(SrpStrength strength)
        {
            return new SrpVerifier(CredentialName, SrpMethods.ComputeV(strength, X), Salt, strength);
        }

        public SrpSecret(byte[] salt, string credentialName, SecureString secret)
        {
            CredentialName = credentialName.Normalize(NormalizationForm.FormKC).Trim();
            credentialName = credentialName.ToLower();
            Salt = (byte[])salt.Clone();
            X = ComputeX(salt, credentialName.ToLower(), secret);
        }

        public SrpSecret(byte[] salt, string credentialName, string secret)
        {
            credentialName = credentialName.Normalize(NormalizationForm.FormKC).Trim().ToLower();
            CredentialName = credentialName;
            Salt = (byte[])salt.Clone();
            X = ComputeX(salt, credentialName, secret);
        }

        public static byte[] ComputeX(byte[] salt, string identifier, string secret)
        {
            return ComputeX(salt, identifier, Encoding.UTF8.GetBytes(secret));
        }
        public static byte[] ComputeX(byte[] salt, string identifier, SecureString secret)
        {
            return ComputeX(salt, identifier, secret.ToUTF8());
        }

        private static byte[] ComputeX(byte[] salt, string identifier, byte[] secret)
        {
            byte[] inner = null;
            try
            {
                using (var sha = SHA512.Create())
                {
                    byte[] idBytes = Encoding.UTF8.GetBytes(identifier);
                    inner = new byte[idBytes.Length + 1 + secret.Length];
                    byte[] outer = new byte[salt.Length + 512 / 8];
                    idBytes.CopyTo(inner, 0);
                    inner[idBytes.Length] = (byte)':';
                    secret.CopyTo(inner, idBytes.Length + 1);
                    sha.ComputeHash(inner).CopyTo(outer, salt.Length);
                    salt.CopyTo(outer, 0);

                    return sha.ComputeHash(outer);
                }
            }
            finally
            {
                if (secret != null)
                    Array.Clear(secret, 0, secret.Length);
                if (inner != null)
                    Array.Clear(inner, 0, inner.Length);
            }

        }
    }
}
