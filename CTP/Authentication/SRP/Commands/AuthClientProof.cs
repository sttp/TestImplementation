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
        /// The Public-A from the SRP algorithm.
        /// </summary>
        [DocumentField()] public byte[] PublicA { get; private set; }

        /// <summary>
        /// Client proof is Derived Key 1
        /// </summary>
        [DocumentField()] public byte[] ClientProof { get; private set; }

        public AuthClientProof(byte[] publicA, byte[] clientProof)
        {
            PublicA = publicA;
            ClientProof = clientProof;
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