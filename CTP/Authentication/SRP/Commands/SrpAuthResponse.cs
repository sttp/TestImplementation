using System.Numerics;
using System.Security;

namespace CTP.SRP
{
    [DocumentName("SrpAuthResponse")]
    public class SrpAuthResponse
        : DocumentObject<SrpAuthResponse>
    {
        [DocumentField()] public int SrpStrength { get; private set; }
        [DocumentField()] public byte[] Salt { get; private set; }
        [DocumentField()] public byte[] PublicB { get; private set; }

        public SrpAuthResponse(SrpStrength strength, byte[] salt, byte[] publicB)
        {
            SrpStrength = (int)strength;
            Salt = salt;
            PublicB = publicB;
        }

        private SrpAuthResponse()
        {

        }

        public BigInteger ComputeX(string credentialName, SecureString secret)
        {
            return SrpSecret.ComputeX(Salt, credentialName, secret).ToUnsignedBigInteger();
        }

        

        public static explicit operator SrpAuthResponse(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}