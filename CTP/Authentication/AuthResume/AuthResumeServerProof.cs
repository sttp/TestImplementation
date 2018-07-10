namespace CTP.SRP
{
    [DocumentName("AuthResumeServerProof")]
    public class AuthResumeServerProof
        : DocumentObject<AuthResumeServerProof>
    {
        /// <summary>
        /// HMAC(HMAC(HMAC(K, 'Challenge Response Key'),Ticket Salt), Client Challenge || Server Challenge))
        /// </summary>
        [DocumentField()] public byte[] ServerProof { get; private set; }

        public AuthResumeServerProof(byte[] serverProof)
        {
            ServerProof = serverProof;
        }

        private AuthResumeServerProof()
        {

        }

        public static explicit operator AuthResumeServerProof(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}