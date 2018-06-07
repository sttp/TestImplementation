namespace CTP.Net
{
    [DocumentName("AuthNegotiate")]
    public class AuthNegotiate
        : DocumentObject<AuthNegotiate>
    {
        [DocumentField()]
        public int StreamID;

        public AuthNegotiate(int streamID)
        {
            StreamID = streamID;
        }

        private AuthNegotiate()
        {

        }
        public static explicit operator AuthNegotiate(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}