using System;

namespace CTP.SRP
{
    [DocumentName("AuthFailure")]
    public class AuthFailure
        : DocumentObject<AuthFailure>
    {
        [DocumentField()]
        public string Reason { get; private set; }

        public AuthFailure(string reason)
        {
            Reason = reason;
        }

        private AuthFailure()
        {

        }

        public static explicit operator AuthFailure(CtpDocument obj)
        {
            return FromDocument(obj);
        }
    }
}