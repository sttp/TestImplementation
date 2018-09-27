using System;

namespace CTP
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