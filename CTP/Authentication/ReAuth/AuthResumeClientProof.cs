namespace CTP.SRP
{
    [DocumentName("AuthResumeClientProof")]
    public class AuthResumeClientProof
        : DocumentObject<AuthResumeClientProof>
    {
        /// <summary>
        /// HMAC(HMAC(HMAC(K, 'Challenge Response Key'),Ticket Salt), Server Challenge || Client Challenge))
        /// </summary>
        [DocumentField()] public byte[] ClientProof { get; private set; }

        /// <summary>
        /// Data that must be echoed by the server in it's response. (Nonce)
        /// </summary>
        [DocumentField()] public byte[] ClientChallenge { get; private set; }

        public AuthResumeClientProof(byte[] clientProof, byte[] clientChallenge)
        {
            ClientProof = clientProof;
            ClientChallenge = clientChallenge;
        }

        private AuthResumeClientProof()
        {

        }


        public static explicit operator AuthResumeClientProof(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}