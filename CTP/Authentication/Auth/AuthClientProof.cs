using System;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography;

namespace CTP.SRP
{
    /// <summary>
    /// Provides the client poof and finishes the key exchange.
    /// </summary>
    [DocumentName("AuthClientProof")]
    public class AuthClientProof
        : DocumentObject<AuthClientProof>
    {
        /// <summary>
        /// The Public-A from the SRP algorithm. A = g^a
        /// </summary>
        [DocumentField()] public byte[] PublicA { get; private set; }

        /// <summary>
        /// Client proof is HMAC(K,'Client Proof')
        /// </summary>
        [DocumentField()] public byte[] ClientProof { get; private set; }

        /// <summary>
        /// (Optional) Encrypted credentials supplied to the server when changing the password.
        /// IV = HMAC(K,'Password IV')
        /// Key = HMAC(K,'Password Key')
        /// Mode: AES-CBC 256-bit.
        /// Padding: PCKS7.
        /// 
        /// Contents if AuthResponse.RequiresPassword = true;
        /// int8 PasswordLength;
        /// byte[] Password (Encoded as UTF8);
        /// int8 SaltLength;
        /// byte[] Salt;
        /// int16 SrpBitStrength;
        /// 
        /// Contents if AuthResponse.RequiresPassword = false;
        /// int16 VerifierLength;
        /// byte[] Verifier;
        /// int8 SaltLength;
        /// byte[] Salt;
        /// int16 SrpBitStrength;
        /// 
        /// </summary>
        [DocumentField()] public byte[] ClientPassword { get; private set; }

        /// <summary>
        /// (Optional) The HMAC(HMAC(K,'Password MAC'),ClientPassword)
        /// </summary>
        [DocumentField()] public byte[] ClientPasswordMAC { get; private set; }


        public AuthClientProof(byte[] publicA, byte[] clientProof, byte[] clientPassword, byte[] clientPasswordMAC)
        {
            PublicA = publicA;
            ClientProof = clientProof;
            ClientPassword = clientPassword;
            ClientPasswordMAC = clientPasswordMAC;
        }

        private AuthClientProof()
        {

        }

        public static explicit operator AuthClientProof(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}