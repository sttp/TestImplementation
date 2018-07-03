namespace CTP.SRP
{
    [DocumentName("AuthSrp")]
    public class AuthSrp
        : DocumentObject<AuthSrp>
    {
        [DocumentField()]
        public string UserName { get; private set; }

        public AuthSrp(string userName)
        {
            UserName = userName;
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