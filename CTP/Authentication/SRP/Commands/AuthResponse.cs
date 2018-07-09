using System.Numerics;
using System.Security;

namespace CTP.SRP
{
    /// <summary>
    /// The response from the server for authentication
    /// </summary>
    [DocumentName("AuthResponse")]
    public class AuthResponse
        : DocumentObject<AuthResponse>
    {
        /// <summary>
        /// The bit strength of the SRP algorithm. 
        /// </summary>
        [DocumentField()] public int SrpStrength { get; private set; }
        /// <summary>
        /// The salt to use to randomize the password.
        /// </summary>
        [DocumentField()] public byte[] Salt { get; private set; }
        /// <summary>
        /// The public key for the key exchange algorithm.
        /// </summary>
        [DocumentField()] public byte[] PublicB { get; private set; }

        public AuthResponse(SrpStrength strength, byte[] salt, byte[] publicB)
        {
            SrpStrength = (int)strength;
            Salt = salt;
            PublicB = publicB;
        }

        private AuthResponse()
        {

        }

        public BigInteger ComputeX(string credentialName, SecureString secret)
        {
            return SrpSecret.ComputeX(Salt, credentialName, secret).ToUnsignedBigInteger();
        }

        public static explicit operator AuthResponse(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}