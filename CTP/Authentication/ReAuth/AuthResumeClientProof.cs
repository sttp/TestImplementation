namespace CTP.SRP
{
    [DocumentName("AuthResumeClientProof")]
    public class AuthResumeClientProof
        : DocumentObject<AuthResumeClientProof>
    {
        /// <summary>
        /// HMAC(HMAC(Derived Key 4, Ticket Salt), Server Challenge || Client Challenge))
        /// </summary>
        [DocumentField()] public byte[] ClientProof { get; private set; }

        /// <summary>
        /// Data that must be echoed by the server in it's response. (Nonce)
        /// </summary>
        [DocumentField()] public byte[] ClientChallenge { get; private set; }

        private AuthResumeClientProof()
        {

        }

        public static explicit operator AuthResumeClientProof(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}