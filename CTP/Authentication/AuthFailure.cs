using System;

namespace CTP
{
    [CommandName("AuthFailure")]
    public class AuthFailure
        : CommandObject<AuthFailure>
    {
        [CommandField()]
        public string Reason { get; private set; }

        public AuthFailure(string reason)
        {
            Reason = reason;
        }

        private AuthFailure()
        {

        }

        public static explicit operator AuthFailure(CtpCommand obj)
        {
            return FromCommand(obj);
        }
    }
}