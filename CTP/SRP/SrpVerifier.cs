using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTP.SRP
{
    public class SrpVerifier
    {
        public readonly string Identifier;
        public readonly byte[] Verification;
        public readonly byte[] Salt;
        public readonly SrpStrength SrpStrength;
        public readonly int IterationCount;

        /// <summary>
        /// Creates user credentials
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="verification"></param>
        /// <param name="salt"></param>
        /// <param name="srpStrength"></param>
        /// <param name="iterationCount"></param>
        public SrpVerifier(string identifier, byte[] verification, byte[] salt, SrpStrength srpStrength, int iterationCount)
        {
            Identifier = identifier.Normalize(NormalizationForm.FormKC).Trim();
            Verification = (byte[])verification.Clone();
            Salt = (byte[])salt.Clone();
            SrpStrength = srpStrength;
            IterationCount = iterationCount;
        }

        public SrpVerifier(string identifier, string password)
        {
            Identifier = identifier.Normalize(NormalizationForm.FormKC).Trim();
            Salt = RNG.CreateSalt(64);
            IterationCount = 1;
            SrpStrength = SrpStrength.Bits2048;
            var pwd = new SrpPassword(Salt, Identifier, password, IterationCount);
            Verification = pwd.CreateVerifier(SrpStrength).Verification;
        }

    }

}
