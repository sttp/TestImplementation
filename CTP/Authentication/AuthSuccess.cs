using System;

namespace CTP
{
    [CommandName("AuthSuccess")]
    public class AuthSuccess
        : CommandObject<AuthSuccess>
    {
        public AuthSuccess()
        {
        }

        public static explicit operator AuthSuccess(CtpCommand obj)
        {
            return FromCommand(obj);
        }
    }
}