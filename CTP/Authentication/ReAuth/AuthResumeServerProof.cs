namespace CTP.SRP
{
    [DocumentName("AuthResumeServerProof")]
    public class AuthResumeServerProof
        : DocumentObject<AuthResumeServerProof>
    {
        /// <summary>
        /// HMAC(HMAC(Derived Key 4, Ticket Salt), Client Challenge || Server Challenge))
        /// </summary>
        [DocumentField()] public byte[] ServerProof { get; private set; }

        private AuthResumeServerProof()
        {

        }

        public static explicit operator AuthResumeServerProof(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}