namespace CTP
{
    [CommandName("AuthNone")]
    public class AuthNone
        : CommandObject<AuthNone>
    {
        public AuthNone()
        {
        }

        public static explicit operator AuthNone(CtpCommand obj)
        {
            return FromCommand(obj);
        }
    }
}