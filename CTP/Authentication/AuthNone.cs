namespace CTP.SRP
{
    [DocumentName("AuthNone")]
    public class AuthNone
        : DocumentObject<AuthNone>
    {
        public AuthNone()
        {
        }

        public static explicit operator AuthNone(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}