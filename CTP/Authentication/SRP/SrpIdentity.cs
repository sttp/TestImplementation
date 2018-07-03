namespace CTP.SRP
{
    [DocumentName("SrpIdentity")]
    public class SrpIdentity
        : DocumentObject<SrpIdentity>
    {
        [DocumentField()]
        public string UserName { get; private set; }

        public SrpIdentity(string userName)
        {
            UserName = userName;
        }

        private SrpIdentity()
        { }

        public static explicit operator SrpIdentity(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}