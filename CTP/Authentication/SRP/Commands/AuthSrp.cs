namespace CTP.SRP
{
    [DocumentName("AuthSrp")]
    public class AuthSrp
        : DocumentObject<AuthSrp>
    {
        [DocumentField()]
        public string CredentialName { get; private set; }

        public AuthSrp(string credentialName)
        {
            CredentialName = credentialName;
        }

        private AuthSrp()
        {

        }

        public static explicit operator AuthSrp(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}