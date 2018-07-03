using System.Numerics;
using System.Security;

namespace CTP.SRP
{
    [DocumentName("SrpIdentityLookup")]
    public class SrpIdentityLookup
        : DocumentObject<SrpIdentityLookup>
    {
        [DocumentField()] public int SrpStrength { get; private set; }
        [DocumentField()] public byte[] Salt { get; private set; }
        [DocumentField()] public int IterationCount { get; private set; }
        [DocumentField()] public byte[] PublicB { get; private set; }

        public SrpIdentityLookup(SrpStrength strength, byte[] salt, byte[] publicB, int iterationCount)
        {
            SrpStrength = (int)strength;
            Salt = salt;
            PublicB = publicB;
            IterationCount = iterationCount;
        }

        public BigInteger ComputePassword(string identity, SecureString password)
        {
            return SrpPassword.ComputeX(Salt, identity, password, IterationCount).ToUnsignedBigInteger();
        }

        private SrpIdentityLookup()
        {

        }

        public static explicit operator SrpIdentityLookup(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}