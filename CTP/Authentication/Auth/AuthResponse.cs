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
        [DocumentField()] public int BitStrength { get; private set; }
       
        /// <summary>
        /// The salt to use to randomize the credential.
        /// </summary>
        [DocumentField()] public byte[] Salt { get; private set; }
       
        /// <summary>
        /// Public-B for the key exchange algorithm. B = k*v + g^b
        /// </summary>
        [DocumentField()] public byte[] PublicB { get; private set; }

        /// <summary>
        /// This indicates that the server must receive the raw password to verify it's complexity requirement.
        /// The server will then compute the verifier and store this instead of the password itself.
        /// </summary>
        [DocumentField()] public bool RequiresPassword { get; private set; }

        public AuthResponse(SrpStrength bitStrength, byte[] salt, byte[] publicB, bool requiresPassword)
        {
            BitStrength = (int)bitStrength;
            Salt = salt;
            PublicB = publicB;
            RequiresPassword = requiresPassword;
        }

        private AuthResponse()
        {

        }

        public BigInteger ComputeX(string credentialName, SecureString secret)
        {
            return SrpMethods.ComputeX(Salt, credentialName, secret).ToUnsignedBigInteger();
        }

        public static explicit operator AuthResponse(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}