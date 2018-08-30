using System;

namespace CTP.SRP
{
    [DocumentName("AuthSuccess")]
    public class AuthSuccess
        : DocumentObject<AuthSuccess>
    {
        public AuthSuccess()
        {
        }

        public static explicit operator AuthSuccess(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}