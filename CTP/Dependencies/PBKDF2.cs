using System;
using System.Security.Cryptography;
using System.Text;

namespace GSF.Security.Cryptography
{
    /// <summary>
    /// Computes a PBKDF2 function.
    /// </summary>
    public static class PBKDF2
    {
        /// <summary>
        /// Computes a PBKEF2 (RFC2898) using the provided hash algorithm.
        /// </summary>
        /// <param name="algorithm">The algorithm to use as part of the HMAC</param>
        /// <param name="password">The password string</param>
        /// <param name="salt">The salt</param>
        /// <param name="iterations">The number of iterations.</param>
        /// <param name="keyBytes">The length of the key to generate from the supplied bytes.</param>
        /// <returns></returns>
        public static byte[] Compute(HashAlgorithmName algorithm, string password, byte[] salt, int iterations, int keyBytes)
        {
            return Compute(algorithm, Encoding.UTF8.GetBytes(password), salt, iterations, keyBytes);
        }

        /// <summary>
        /// Computes a PBKEF2 (RFC2898) using the provided hash algorithm.
        /// </summary>
        /// <param name="algorithm">The algorithm to use as part of the HMAC</param>
        /// <param name="password">The password string</param>
        /// <param name="salt">The salt</param>
        /// <param name="iterations">The number of iterations.</param>
        /// <param name="keyBytes">The length of the key to generate from the supplied bytes.</param>
        /// <returns></returns>
        public static byte[] Compute(HashAlgorithmName algorithm, string password, string salt, int iterations, int keyBytes)
        {
            return Compute(algorithm, Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes(salt), iterations, keyBytes);
        }

        /// <summary>
        /// Computes a PBKEF2 (RFC2898) using the provided hash algorithm.
        /// </summary>
        /// <param name="algorithm">The algorithm to use as part of the HMAC</param>
        /// <param name="password">The password bytes</param>
        /// <param name="salt">The salt</param>
        /// <param name="iterations">The number of iterations.</param>
        /// <param name="keyBytes">The length of the key to generate from the supplied bytes.</param>
        /// <returns></returns>
        public static byte[] Compute(HashAlgorithmName algorithm, byte[] password, byte[] salt, int iterations, int keyBytes)
        {
            byte[] results = new byte[keyBytes];
            using (var mac = HMAC.Create("HMAC" + algorithm.Name))
            {
                mac.Key = password;
                int macSize = mac.HashSize >> 3;
                int loopCount = (keyBytes + macSize - 1) / macSize; //Round up

                for (int i = 0; i < loopCount; i++)
                {
                    /*
                       From RFC 2898

                       PBKDF2 (P, S, c, dkLen)

                       Options:        PRF        underlying pseudorandom function (hLen
                                                  denotes the length in octets of the
                                                  pseudorandom function output)

                       Input:          P          password, an octet string
                                       S          salt, an octet string
                                       c          iteration count, a positive integer
                                       dkLen      intended length in octets of the derived
                                                  key, a positive integer, at most
                                                  (2^32 - 1) * hLen

                       Output:         DK         derived key, a dkLen-octet string


                        F (P, S, c, i) = U_1 \xor U_2 \xor ... \xor U_c

                    where

                       U_1 = PRF (P, S || INT (i)) ,
                       U_2 = PRF (P, U_1) ,
                       ...
                       U_c = PRF (P, U_{c-1}) .

                            Here, INT (i) is a four-octet encoding of the integer i, most
                            significant octet first.

                        DK = T_1 || T_2 ||  ...  || T_l<0..r-1>
                     */

                    byte[] block = new byte[salt.Length + 4];
                    salt.CopyTo(block, 0);
                    BigEndian.CopyBytes(i + 1, block, salt.Length);
                    block = mac.ComputeHash(block);
                    byte[] rv = block;
                    for (int iteration = 2; iteration <= iterations; iteration++)
                    {
                        block = mac.ComputeHash(block);
                        for (int x = 0; x < rv.Length; x++)
                        {
                            rv[x] ^= block[x];
                        }
                    }

                    // Copy the results into the supplied buffer.
                    Array.Copy(rv, 0, results, i * macSize, Math.Min(rv.Length, results.Length - i * macSize));
                }


                return results;
            }
        }

        

    }
}